using Microsoft.Win32;
using System;

namespace VoiceMeeterWrapper
{
    public class VmClient : IDisposable
    {
        private Action _onClose = null;
        private string GetVoicemeeterDir()
        {
            const string regKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            const string uninstKey = "VB:Voicemeeter {17359A74-1236-5467}";
            var key = $"{regKey}\\{uninstKey}";
            var regBase32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            var regBase64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            var k = regBase32.OpenSubKey(key).GetValue("UninstallString");
            if (k == null)
            {
                k = regBase64.OpenSubKey(key).GetValue("UninstallString");
                if (k == null) throw new Exception("Voicemeeter not found");
            }
            return System.IO.Path.GetDirectoryName(k.ToString());
        }
        public VmClient()
        {
            //Find Voicemeeter dir.
            var vmDir = GetVoicemeeterDir();
            var vmDll = Environment.Is64BitProcess ? "VoicemeeterRemote64.dll" : "VoicemeeterRemote.dll";
            Console.WriteLine($"VoiceMeeter found at {vmDir}\\{vmDll}");
            VoiceMeeterRemote.LoadDll(System.IO.Path.Combine(vmDir, vmDll));
            var lr = VoiceMeeterRemote.Login();
            switch (lr)
            {
                case VbLoginResponse.OK:
                    Console.WriteLine("Attached.");
                    break;
                case VbLoginResponse.AlreadyLoggedIn:
                    Console.WriteLine("Attached. Was already logged in");
                    break;
                case VbLoginResponse.OkVoicemeeterNotRunning:
                    //Launch.
                    Console.WriteLine("Attached. VM Not running.");
                    break;
                default:
                    throw new InvalidOperationException("Bad response from voicemeeter: " + lr);
            }
        }
        public float GetParam(string n)
        {
            float output = -1;
            VoiceMeeterRemote.GetParameter(n, ref output);
            return output;
        }
        public float GetInputLevel(int index)
        {
            float output = -1;
            VoiceMeeterRemote.GetGetLevel(0, index, ref output);
            return AmplitudeToDb(output);
        }
        public float GetOutputLevel(int index)
        {
            float output = -1;
            VoiceMeeterRemote.GetGetLevel(1, index, ref output);
            return AmplitudeToDb(output);
        }
        public float AmplitudeToDb(float amp)
        {
            if (amp > 0)
                return (float)(Math.Log10(amp) * 20.0f);
            else
                return float.NegativeInfinity;
        }
        public void SetParam(string n,float v)
        {
            VoiceMeeterRemote.SetParameter(n, v);
        }
        public void RunCommands(string n)
        {
            VoiceMeeterRemote.SetParameters(n);
        }
        public bool Poll()
        {
            return VoiceMeeterRemote.IsParametersDirty() == 1;
        }
        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                Console.WriteLine($"VmClient Disposing {disposing}");
                _onClose?.Invoke();
                VoiceMeeterRemote.Logout();
            }
            disposed = true;
        }
        ~VmClient() { Dispose(false); }
        public void OnClose(Action a)
        {
            _onClose = a;
        }
    }
}
