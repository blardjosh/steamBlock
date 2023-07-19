using NetFwTypeLib;
using System;
using System.Data;

namespace steamBlock
{
    public partial class MainForm : Form
    {
        private const string FwID = "{304CE942-6E39-40D8-943A-B913C40C9CD4}";

        public MainForm()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
                INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(tNetFwPolicy2);


                INetFwRule fwRule = (INetFwRule2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
                fwRule.Name = "steamBlockApp";
                fwRule.ApplicationName = @"C:\Program Files (x86)\Steam\steam.exe";
                fwRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
                fwRule.Enabled = true;
                fwRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
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
                StatusLabel1.Text = r.ToString();
            }
        }
    }
}