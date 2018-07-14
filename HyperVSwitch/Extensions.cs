using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HyperVSwitch.Properties;

namespace HyperVSwitch
{
    internal static class Extensions
    {
        static bool isMainWindowOpen = false;
        static bool isAboutOpen = false;

        public static Task WaitForExitAsync(this Process process, CancellationToken cancellationToken = default(CancellationToken))
        {
            var tcs = new TaskCompletionSource<object>();
            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) => tcs.TrySetResult(null);
            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(tcs.SetCanceled);
            }
            return tcs.Task;
        }

        public static void ShowMainWindow()
        {
            if (isMainWindowOpen) return;

            isMainWindowOpen = true;
            new MainForm().ShowDialog();
            isMainWindowOpen = false;
        }

        public static void ShowAbout()
        {
            if (isAboutOpen) return;

            isAboutOpen = true;
            const string message = "Hyper-V Switch allows you to enable or disable permanent virtualisation with Hyper-V without uninstalling it so that you can use Hyper-V and other virtualisation solutions like VMware or VirtualBox easily. This setting is stored in the boot configuration so that the computer must be restarted to apply the new setting.\n\n" +
                                   "For more information please click on the link to open the website.\n\n" +
                                   "Available keyboard shortcuts:\n\n" +
                                   "Escape: Close program\n" +
                                   "Shift+Click: Change state but skip restart (you need to restart manually)";
            MessageBox.Show(message, Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            isAboutOpen = false;
        }
    }
}
