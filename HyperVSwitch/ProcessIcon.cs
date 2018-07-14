using System;
using System.Diagnostics;
using System.Windows.Forms;
using HyperVSwitch.Properties;

namespace HyperVSwitch
{
    internal class ProcessIcon : IDisposable
    {
        readonly NotifyIcon ni;

        public ProcessIcon()
        {
            ni = new NotifyIcon();
        }

        public async void Display()
        {
            ni.DoubleClick += NiOnDoubleClick;
            ni.Icon = Resources.TrayIcon;
            ni.Text = Resources.AppName;
            ni.Visible = true;

            ni.ContextMenuStrip = await new ContextMenu().CreateAsync();
        }

        private static void NiOnDoubleClick(object sender, EventArgs e)
        {
            Extensions.ShowMainWindow();
        }

        public void Dispose()
        {
            ni.Dispose();
        }
    }
}