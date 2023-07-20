using NetFwTypeLib;
using System;
using System.Data;

namespace steamBlock
{
    public partial class MainForm : Form
    {
        private const string FwID = "{304CE942-6E39-40D8-943A-B913C40C9CD4}";
        private INetFwRule fwRule;
        private INetFwPolicy2 fwPolicy2;
        private Type tNetFwPolicy2;
        private bool ruleExists;
        private bool ruleAction;

        private void updateStatusBar(bool status)
        {
            if (status)
            {
                statusStrip1.BackColor = Color.FromArgb(192, 255, 192);
                StatusLabel1.Text = "Steam connection is currently being Allowed.";
            }
            else
            {
                statusStrip1.BackColor = Color.FromArgb(255, 192, 192);
                StatusLabel1.Text = "Steam connection is currently being Blocked.";
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
                fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(tNetFwPolicy2);


                fwRule = (INetFwRule2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
                fwRule.Name = "steamBlockApp";
                fwRule.ApplicationName = steamBlock.Properties.Settings.Default.saveAppDir;
                fwRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
                fwRule.Enabled = true;
                fwRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
                fwRule.Protocol = 256; // ANY
                fwRule.Profiles = (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL;

                ruleExists = Program.RuleExists(fwRule.Name, fwPolicy2);
                ruleAction = Program.RuleAction(fwRule.Name, fwPolicy2);

                if (!fwRule.ApplicationName.Equals(""))
                {
                    setDirectoryToolStripMenuItem.Text = fwRule.ApplicationName;
                }

                if (ruleExists)
                {
                    updateStatusBar(ruleAction);
                }
                else
                {
                    fwPolicy2.Rules.Add(fwRule);
                    updateStatusBar(ruleAction);
                }
            }
            catch (Exception r)
            {
                StatusLabel1.Text = "Error, try launching as administrator!";
            }
        }

        private void setDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
        }

        private void openFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            fwRule.ApplicationName = openFileDialog.FileName;
            setDirectoryToolStripMenuItem.Text = fwRule.ApplicationName;
            fwPolicy2.Rules.Item(fwRule.Name).ApplicationName = fwRule.ApplicationName;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            steamBlock.Properties.Settings.Default.saveAppDir = fwRule.ApplicationName;
            steamBlock.Properties.Settings.Default.Save();
        }

        private void toggleActionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fwRule.Action == NET_FW_ACTION_.NET_FW_ACTION_ALLOW)
            {
                fwRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
                toggleActionToolStripMenuItem.Text = "&Block Steam";
                updateStatusBar(false);
            } 
            else
            {
                fwRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
                toggleActionToolStripMenuItem.Text = "&Allow Steam";
                updateStatusBar(true);
            }

            fwPolicy2.Rules.Item(fwRule.Name).Action = fwRule.Action;
        }
    }
}