using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SHAgent.Tests
{
    [TestClass]
    public class IntegrationTest
    {
        private Process _serverProcess;
        private Process _clientProcess;
        private ILog _logger;

        [TestInitialize]
        public void Initialize()
        {
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
            var file = new FileInfo(fileName);
            log4net.Config.XmlConfigurator.Configure(file);

            _logger = LogManager.GetLogger(typeof (IntegrationTest));
        }

        [TestMethod]
        public void StartStopServer()
        {
            StartServer();

            Thread.Sleep(10000);

            StopServer();
        }

        private void StopServer()
        {
            _serverProcess.Kill();
        }

        public void TestStartCommand()
        {
            StartServer();
            ExecuteStartCommandForLongRunningCommandLineApp();
        }

        private void ExecuteStartCommandForLongRunningCommandLineApp()
        {
            const string server = "127.0.0.1";
            string port = ConfigurationManager.AppSettings["Port"];
            const string command = "START;un;pw;bla";

            _clientProcess = Process.Start(string.Format("telnet {0} {1} {2}", server, port, command));
        }

        private void StartServer()
        {
            string app = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\shagent\bin\debug\shagent.exe");

            _logger.Debug("Starting server from:" + app);

            _serverProcess = Process.Start(app);
        }
    }
}