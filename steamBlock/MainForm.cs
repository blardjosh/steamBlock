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
                fwRule.ApplicationName = @"C:\Program Files (x86)\Steam\steam.exe";
                fwRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
                fwRule.Enabled = true;
                fwRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
                fwRule.Protocol = 256; // ANY
                fwRule.Profiles = (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL;


                bool ruleExists = Program.RuleExists(fwRule.Name, fwPolicy2);
                bool ruleAction = Program.RuleAction(fwRule.Name, fwPolicy2);



                if (ruleExists)
                {
                    StatusLabel1.Text = Program.VisStatus(ruleAction);
                }
                else
                {
                    fwPolicy2.Rules.Add(fwRule);
                    StatusLabel1.Text = Program.VisStatus(ruleAction);
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
            setDirectoryToolStripMenuItem.Text = openFileDialog.FileName;
            fwPolicy2.Rules.Item(fwRule.Name).ApplicationName = setDirectoryToolStripMenuItem.Text;
        }
    }
}