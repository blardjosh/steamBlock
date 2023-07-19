using NetFwTypeLib;
using System.Data;

namespace steamBlock
{

    internal static class Program
    {


        /*
         * Searches through existing firewall rules to see if a rule with a name matching 
         * ruleName exists.
         */
        public static bool RuleExists(string ruleName, INetFwPolicy2 policy)
        {
            foreach (INetFwRule rule in policy.Rules)
            {
                if (string.Equals(rule.Name, ruleName))
                {
                    return true;
                }
            }
            return false;
        }

        /*
         * Assuming ruleName given matches an existing rule, returns true if rule is set to
         * allow, and returns false if rule is set to block.
         */

        public static bool RuleAction(string ruleName, INetFwPolicy2 policy)
        {
            foreach (INetFwRule rule in policy.Rules)
            {
                if (string.Equals(rule.Name, ruleName))
                {
                    if (rule.Action == NET_FW_ACTION_.NET_FW_ACTION_BLOCK)
                    {
                        return false;
                    }
                }
            }
            return true; 
        }

        /*
         * Returns a checkmark if status is true, and a no symbol if false,
         * and null string if status is null.
         */

        public static string VisStatus(bool status)
        {
            if (status)
            {
                return "Allow";
            }
            else if (status == false)
            {
                return "Block";
            }
            return "";
        }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}