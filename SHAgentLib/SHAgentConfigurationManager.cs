using System;
using System.Configuration;

namespace SHAgent
{
    public class SHAgentConfigurationManager : IConfigurationManager
    {
        public int Port
        {
            get { return int.Parse(ConfigurationManager.AppSettings["Port"]); }
        }

        public string ExpectedSourceIpAddress
        {
            get { return ConfigurationManager.AppSettings["ExpectedSourceIpAddress"]; }
        }

        public string ExpectedUserName
        {
            get { return ConfigurationManager.AppSettings["ExpectedUserName"]; }
        }

        public string ExpectedPassword
        {
            get { return ConfigurationManager.AppSettings["ExpectedPassword"]; }
        }

        public string Command
        {
            get { return ConfigurationManager.AppSettings["Command"]; }
        }

        public string ServerIpAddress
        {
            get { return ConfigurationManager.AppSettings["ServerIpAddress"]; }
        }

        public bool SourceAddressCheckEnabled
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["SourceAddressCheckEnabled"]); }
        }

        public bool UseRemoteCommand
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["UseRemoteCommand"]); }
        }
    }
}