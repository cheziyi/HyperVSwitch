using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace HyperVSwitch
{
    public partial class MainForm : Form
    {
        #region Private fields

        private bool? isHyperVActive;
        private bool? isHyperVRunning;
        private bool justRestart;

        #endregion Private fields

        #region Constructors

        public MainForm()
        {
            InitializeComponent();

            statusLabel.Text = "Detecting current state…\nThis may take a few seconds.";
            statusLabel.ForeColor = Color.Gray;
            actionButton.Visible = false;
            infoLabel.Text = "© 2018 Calvin Che, GNU GPL v3";
        }

        #endregion Constructors

        #region Window event handlers

        private async void MainForm_Load(object sender, EventArgs args)
        {
            isHyperVActive = await HyperV.GetStatus();
            isHyperVRunning = HyperV.GetRunning();

            actionButton.Visible = true;
            actionButton.Focus();

            if (isHyperVActive == true)
            {
                statusLabel.Text = "Hyper-V is ACTIVE.";
                statusLabel.ForeColor = Color.ForestGreen;
                actionButton.Text = "Deactivate Hyper-V and restart computer";
                if (isHyperVRunning == false)
                {
                    statusLabel.Text += " However, it is currently NOT running, so a restart may be pending.";
                    justRestart = true;
                    actionButton.Text = "Restart computer";
                }
            }
            else if (isHyperVActive == false)
            {
                statusLabel.Text = "Hyper-V is DEACTIVATED.";
                statusLabel.ForeColor = Color.Firebrick;
                actionButton.Text = "Activate Hyper-V and restart computer";
                if (isHyperVRunning == true)
                {
                    statusLabel.Text += " However, it is currently running, so a restart may be pending.";
                    justRestart = true;
                    actionButton.Text = "Restart computer";
                }
            }
            else
            {
                statusLabel.Text = "The current state of Hyper-V is UNKNOWN. The Hyper-V role may not be installed on this computer.";
                statusLabel.ForeColor = SystemColors.WindowText;
                actionButton.Text = "No action available";
                actionButton.Enabled = false;
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs args)
        {
            if (args.KeyCode == Keys.Escape && args.Modifiers == 0)
            {
                Application.Exit();
            }
            if (args.KeyCode == Keys.F1 && args.Modifiers == 0)
            {
                Extensions.ShowAbout();
            }
        }

        #endregion Window event handlers

        #region Control event handlers

        private async void ActionButton_Click(object sender, EventArgs args)
        {
            bool shiftKeyPressed = ModifierKeys == Keys.Shift;
            actionButton.Enabled = false;
            try
            {
                if (!justRestart)
                {
                    if (isHyperVActive == true)
                    {
                        if (!await HyperV.SetStatus(false))
                        {
                            MessageBox.Show("Deactivating Hyper-V failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else if (isHyperVActive == false)
                    {
                        if (!await HyperV.SetStatus(true))
                        {
                            MessageBox.Show("Activating Hyper-V failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        return;   // Should not happen
                    }
                }

                if (!shiftKeyPressed)
                {
                    HyperV.RebootMachine();
                }
            }
            finally
            {
                actionButton.Enabled = true;
            }

            // System is restarted. Prevent further actions
            Application.Exit();
        }

        private void InfoLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs args)
        {
            Process.Start("http://unclassified.software/");
        }

        #endregion Control event handlers
    }
}
