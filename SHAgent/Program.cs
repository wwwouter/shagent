using System;
using System.IO;
using log4net;

namespace SHAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            ILog logger = null;

            try
            {
                log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));

                logger = LogManager.GetLogger("SHAgent");

                logger.Info("Starting SHAgent");

                var shAgentConfigurationManager = new SHAgentConfigurationManager();

                var agent = new Agent(shAgentConfigurationManager, new ProcessManager(shAgentConfigurationManager));

                agent.Start();
            }
            catch (Exception e)
            {
                if(logger != null)
                    logger.Error("Could not start agent", e);
                else
                    Console.WriteLine("Error..... " + e.StackTrace);
            }
        }
    }
}
