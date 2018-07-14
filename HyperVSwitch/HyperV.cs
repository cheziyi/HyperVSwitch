using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace HyperVSwitch
{
    public static class HyperV
    {
        public static async Task<bool?> GetStatus()
        {
            var startInfo = new ProcessStartInfo
            {
                Arguments = "/enum {default}",
                CreateNoWindow = true,
                FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "bcdedit.exe"),
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            using (var process = Process.Start(startInfo))
            {
                await process.WaitForExitAsync();
                while (!process.StandardOutput.EndOfStream)
                {
                    var line = process.StandardOutput.ReadLine();
                    if (line.StartsWith("hypervisorlaunchtype ", StringComparison.OrdinalIgnoreCase))
                    {
                        return line.IndexOf(" off", StringComparison.OrdinalIgnoreCase) == -1;
                    }
                }
            }
            return null;
        }

        public static async Task<bool> SetStatus(bool active)
        {
            var startInfo = new ProcessStartInfo
            {
                Arguments = "/set {current} hypervisorlaunchtype " + (active ? "auto" : "off"),
                CreateNoWindow = true,
                FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "bcdedit.exe"),
                UseShellExecute = false
            };
            using (var process = Process.Start(startInfo))
            {
                await process.WaitForExitAsync();
                return process.ExitCode == 0;
            }
        }

        public static bool? GetRunning()
        {
            return !SafeNativeMethods.IsProcessorFeaturePresent(ProcessorFeature.PF_VIRT_FIRMWARE_ENABLED);
        }

        public static void RebootMachine()
        {
            if (!SafeNativeMethods.ExitWindowsEx(
                ExitWindows.Reboot,
                ShutdownReason.MajorOperatingSystem | ShutdownReason.MinorReconfig | ShutdownReason.FlagPlanned))
            {
                //int error = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                //string errorMessage = new System.ComponentModel.Win32Exception(error).Message;
                //MessageBox.Show($"Restarting the computer failed. {errorMessage} (Error {error}) Trying another method…", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // ExitWindowsEx fails on Windows 10.
                // Use the system command, there's no feedback from it.
                Process.Start("shutdown.exe", "-r -t 0");
            }
        }
    }
}
