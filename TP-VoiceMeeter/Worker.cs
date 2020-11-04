
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TouchPortalApi.Interfaces;
using TouchPortalApi.Models;
using VoiceMeeterWrapper;

namespace TP_VoiceMeeter
{
    public class Worker : BackgroundService
    {
        private static readonly string floatFormat = "0.#";
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly ILogger<Worker> _logger;
        private readonly IMessageProcessor _messageProcessor;
        private readonly VmClient _vmClient;
        private readonly List<Dictionary<string, float>> _stripState = new List<Dictionary<string, float>>();
        private readonly List<Dictionary<string, float>> _busState = new List<Dictionary<string, float>>();
        private readonly List<float> _inputLevels = new List<float>();
        private readonly List<float> _outputLevels = new List<float>();
        public Worker(IHostApplicationLifetime hostApplicationLifetime, ILogger<Worker> logger, IMessageProcessor messageProcessor)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _logger = logger;
            _messageProcessor = messageProcessor ?? throw new ArgumentNullException(nameof(messageProcessor));
            Console.WriteLine($"{DateTime.Now} Plugin Connecting to VoiceMeeter");
            _vmClient = new VmClient();
            _vmClient.Poll();

            // initialize states
            Dictionary<string, float> initStripState = new Dictionary<string, float>();
            Dictionary<string, float> initBusState = new Dictionary<string, float>();

            foreach (var state in VmData.StripToggles.Union(VmData.StripValues))
            {
                initStripState[state] = 0;
            }

            foreach (var state in VmData.BusToggles.Union(VmData.BusValues))
            {
                initBusState[state] = 0;
            }

            for (int i = 0; i < 8; i++)
            {
                _stripState.Add(new Dictionary<string, float>(initStripState));
                _busState.Add(new Dictionary<string, float>(initBusState));
            }

            for (int i = 0; i < VmData.InputLevels; i++)
            {
                _inputLevels.Add(0);
            }

            for (int i = 0; i < VmData.OutputLevels; i++)
            {
                _outputLevels.Add(0);
            }
        }

        private void UpdateState(bool sendUpdateMessage = true)
        {
            _vmClient.Poll();

            for (int i = 0; i < VmData.InputLevels; i++)
            {
                var level = _vmClient.GetInputLevel(i);
                if(!_inputLevels[i].Equals(level))
                {
                    _inputLevels[i] = level;
                    if (sendUpdateMessage)
                    {
                        _messageProcessor.UpdateState(new StateUpdate() { Id = $"tpvm_input_level{i}", Value = level.ToString(floatFormat) });
                        _messageProcessor.UpdateState(new StateUpdate() { Id = $"tpvm_input_level{i}_int", Value = Math.Round((double)level).ToString() });
                    }
                }
            }

            for (int i = 0; i < VmData.OutputLevels; i++)
            {
                var level = _vmClient.GetOutputLevel(i);
                if (!_outputLevels[i].Equals(level))
                {
                    _outputLevels[i] = level;
                    if (sendUpdateMessage)
                    {
                        _messageProcessor.UpdateState(new StateUpdate() { Id = $"tpvm_output_level{i}", Value = level.ToString(floatFormat) });
                        _messageProcessor.UpdateState(new StateUpdate() { Id = $"tpvm_output_level{i}_int", Value = Math.Round((double)level).ToString() });
                    }
                }
            }

            for (int i = 0; i < 8; i++)
            {
                foreach (var state in _stripState[i].Keys.ToList())
                {
                    var value = _vmClient.GetParam($"Strip[{i}].{state}");
                    if (!_stripState[i][state].Equals(value))
                    {
                        _stripState[i][state] = value;
                        if (sendUpdateMessage)
                            _messageProcessor.UpdateState(new StateUpdate() { Id = $"tpvm_strip{i}_{state.ToLower()}", Value = value.ToString(floatFormat)  });
                    }
                }

                foreach (var state in _busState[i].Keys.ToList())
                {
                    var value = _vmClient.GetParam($"Bus[{i}].{state}");
                    if (!_busState[i][state].Equals(value))
                    {
                        _busState[i][state] = value;
                        if (sendUpdateMessage)
                            _messageProcessor.UpdateState(new StateUpdate() { Id = $"tpvm_bus{i}_{state.ToLower()}", Value = value.ToString(floatFormat) });
                    }
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            bool stopRequested = false;

            // On Plugin Connect Event
            _messageProcessor.OnConnectEventHandler += () => {
                _messageProcessor.UpdateState(new StateUpdate() { Id = "tpvm_connected", Value = "1" });
            };

            // On Action Event
            _messageProcessor.OnActionEvent += (actionId, dataList) => {
                var index = dataList.Where(d => d.Id.Equals("tpvm_index"))?.First()?.Value;
                var strip_setting = dataList.Where(d => d.Id.Equals("tpvm_toggle"));
                var bus_setting = dataList.Where(d => d.Id.Equals("tpvm_setting"));
                var setting = "";
                if (strip_setting.Any())
                    setting = strip_setting.First().Value;
                else if (bus_setting.Any())
                    setting = bus_setting.First().Value;
                var value = dataList.Where(d => d.Id.Equals("tpvm_state"))?.First()?.Value;
                float floatValue = 0;
                float.TryParse(value, out floatValue);

                Console.WriteLine($"TP-VoiceMeeter Bus/Strip[{index}].{setting}={value}");

                switch (actionId)
                {
                    case "tpvm_strip_setting":
                    case "tpvm_strip_toggle":
                        _vmClient.SetParam($"Strip[{index}].{setting}", floatValue);
                        break;
                    case "tpvm_bus_setting":
                    case "tpvm_bus_toggle":
                        _vmClient.SetParam($"Bus[{index}].{setting}", floatValue);
                        break;
                }
            };

            // On List Change Event
            _messageProcessor.OnListChangeEventHandler += (actionId, value) => {
                Console.WriteLine($"{DateTime.Now} Choice Event Fired.");
            };

            // On Plugin Disconnect
            _messageProcessor.OnCloseEventHandler += () => {
                Console.Write($"{DateTime.Now} Plugin Quit Command");
                _messageProcessor.UpdateState(new StateUpdate() { Id = "tpvm_connected", Value = "0" });
                stopRequested = true;
            };

            // Run Listen and pairing
            _ = Task.WhenAll(new Task[] {
                _messageProcessor.Listen(),
                _messageProcessor.TryPairAsync()
            });

            try
            {
                while (!stoppingToken.IsCancellationRequested && !stopRequested)
                {
                    UpdateState();
                    await Task.Delay(100, stoppingToken);
                }
            }
            finally
            {
                _hostApplicationLifetime.StopApplication();
            }
        }
    }
}
