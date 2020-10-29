using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VoiceMeeterWrapper;
using System.Windows.Forms;

namespace EntryTPGenerator
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (TextReader tr = new StreamReader("base.json"))
            {
                var json = tr.ReadToEnd();
                dynamic entry = JToken.Parse(json);

                for (int i = 0; i < VmData.InputLevels; i++)
                {
                    var st = new JObject {
                            { "id", $"tpvm_input_level{i}" },
                            { "type", "text" },
                            { "desc", $"TP-VoiceMeeter Strip InputLevel[{i}]" },
                            { "default", "" }
                        };

                    ((JArray)entry.categories[0].states).Add(st);
                }

                for (int i = 0; i < VmData.OutputLevels; i++)
                {
                    var st = new JObject {
                            { "id", $"tpvm_output_level{i}" },
                            { "type", "text" },
                            { "desc", $"TP-VoiceMeeter Strip OutputLevel[{i}]" },
                            { "default", "" }
                        };

                    ((JArray)entry.categories[0].states).Add(st);
                }

                for (int i = 0; i < 8; i++)
                {
                    foreach (string stripToggle in VmData.StripToggles)
                    {
                        var ev = new JObject {
                            { "id", $"tpvm_strip{i}_{stripToggle.ToLower()}_event" },
                            { "name", $"TP-VoiceMeeter Strip[{i}].{stripToggle}" },
                            { "format", $"TP-VoiceMeeter Strip[{i}].{stripToggle}=$val" },
                            { "type", "communicate" },
                            { "valueType", "choice" },
                            { "valueChoices", new JArray { "0", "1" } },
                            { "valueStateId", $"tpvm_strip{i}_{stripToggle.ToLower()}" }
                        };
                        var st = new JObject {
                            { "id", $"tpvm_strip{i}_{stripToggle.ToLower()}" },
                            { "type", "choice" },
                            { "desc", $"TP-VoiceMeeter Strip[{i}].{stripToggle}" },
                            { "default", "0" },
                            { "valueChoices", new JArray { "0", "1" } }
                        };

                        ((JArray)entry.categories[1].events).Add(ev);
                        ((JArray)entry.categories[1].states).Add(st);
                    }
                    foreach (string stripValue in VmData.StripValues)
                    {
                        var st = new JObject {
                            { "id", $"tpvm_strip{i}_{stripValue.ToLower()}" },
                            { "type", "text" },
                            { "desc", $"TP-VoiceMeeter Strip[{i}].{stripValue}" },
                            { "default", "" }
                        };

                        ((JArray)entry.categories[1].states).Add(st);
                    }
                    foreach (string busToggle in VmData.BusToggles)
                    {
                        var ev = new JObject {
                            { "id", $"tpvm_bus{i}_{busToggle.ToLower()}_event" },
                            { "name", $"TP-VoiceMeeter Bus[{i}].{busToggle}" },
                            { "format", $"TP-VoiceMeeter Bus[{i}].{busToggle}=$val" },
                            { "type", "communicate" },
                            { "valueType", "choice" },
                            { "valueChoices", new JArray { "0", "1" } },
                            { "valueStateId", $"tpvm_bus{i}_{busToggle.ToLower()}" }
                        };
                        var st = new JObject {
                            { "id", $"tpvm_bus{i}_{busToggle.ToLower()}" },
                            { "type", "choice" },
                            { "desc", $"TP-VoiceMeeter Bus[{i}].{busToggle}" },
                            { "default", "0" },
                            { "valueChoices", new JArray { "0", "1" } }
                        };

                        ((JArray)entry.categories[2].events).Add(ev);
                        ((JArray)entry.categories[2].states).Add(st);
                    }
                    foreach (string busValue in VmData.BusValues)
                    {
                        var st = new JObject {
                            { "id", $"tpvm_bus{i}_{busValue.ToLower()}" },
                            { "type", "text" },
                            { "desc", $"TP-VoiceMeeter Bus[{i}].{busValue}" },
                            { "default", "" }
                        };

                        ((JArray)entry.categories[2].states).Add(st);
                    }
                }

                string entryJson = ((JToken)entry).ToString();
                Clipboard.SetText(entryJson);
            }
        }
    }
}
