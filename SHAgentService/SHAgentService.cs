using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using log4net;
using log4net.Config;
using SHAgent;

namespace SHAgentService
{
    public partial class SHAgentService : ServiceBase
    {
        private Agent _agent;
        private Thread _thread;
        private ILog _logger;

        public SHAgentService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _logger = null;

            try
            {
                XmlConfigurator.Configure(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config")));
                _logger = LogManager.GetLogger("SHAgent");
                _logger.Info("Starting SHAgent");

                var shAgentConfigurationManager = new SHAgentConfigurationManager();

                _agent = new Agent(shAgentConfigurationManager, new ProcessManager(shAgentConfigurationManager));

                _thread = new Thread(_agent.Start);
                _thread.Start();
            }
            catch (Exception e)
            {
                if (_logger != null)
                    _logger.Error("Could not start agent", e);
                else
                    Console.WriteLine("Error..... " + e.StackTrace);
            }
        }

        protected override void OnStop()
        {
            try
            {
                _agent.Stop();
                _thread.Abort();
            }
            catch (Exception e)
            {
                if(_logger != null)
                    _logger.Error(e.Message, e);
            }
        }
    }
}
