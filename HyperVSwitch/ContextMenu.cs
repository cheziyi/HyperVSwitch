using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HyperVSwitch
{
    internal class ContextMenu
    {
        private bool? isHyperVActive;

        public async Task<ContextMenuStrip> CreateAsync()
        {
            // Add the default menu options.
            var menu = new ContextMenuStrip();


            // Actions.
            isHyperVActive = await HyperV.GetStatus();

            var status = new ToolStripMenuItem();
            var action = new ToolStripMenuItem();
            status.Click += App_Click;
            action.Click += Action_Click;

            status.Font = new Font(status.Font, status.Font.Style | FontStyle.Bold);

            if (isHyperVActive == true)
            {
                status.Text = "Hyper-V is ACTIVE.";
                status.ForeColor = Color.ForestGreen;
                action.Text = "Deactivate Hyper-V and restart computer";
            }
            else if (isHyperVActive == false)
            {
                status.Text = "Hyper-V is DEACTIVATED.";
                status.ForeColor = Color.Firebrick;
                action.Text = "Activate Hyper-V and restart computer";
            }
            else
            {
                status.Text = "The current state of Hyper-V is UNKNOWN. The Hyper-V role may not be installed on this computer.";
                status.ForeColor = SystemColors.WindowText;
                action.Text = "No action available";
                action.Enabled = false;
            }

            // Add Status.
            menu.Items.Add(status);

            // Separator.
            var sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            // Add Action.
            menu.Items.Add(action);

            // Separator.
            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            // Exit.
            var item = new ToolStripMenuItem { Text = "Exit" };
            item.Click += Exit_Click;
            menu.Items.Add(item);

            return menu;
        }

        private async void Action_Click(object sender, EventArgs e)
        {
            switch (isHyperVActive)
            {
                case true:
                    if (!await HyperV.SetStatus(false))
                    {
                        MessageBox.Show("Deactivating Hyper-V failed.", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                    break;

                case false:
                    if (!await HyperV.SetStatus(true))
                    {
                        MessageBox.Show("Activating Hyper-V failed.", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                    break;
            }

            HyperV.RebootMachine();

            Application.Exit();
        }

        private static void App_Click(object sender, EventArgs e)
        {
            Extensions.ShowMainWindow();
        }

        private static void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}