using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
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
            //StartServer();

            StartSlowComandLineApp();

            Thread.Sleep(1000);

            StopServer();
        }

        private void StopServer()
        {
            _serverProcess.Kill();
        }

        public void StartSlowComandLineApp()
        {
            const string server = "127.0.0.1";
            int port = int.Parse(ConfigurationManager.AppSettings["Port"]);
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\TestConsoleApp\bin\debug\TestConsoleApp.exe");

            string command = "START;un;pw;" + path;
            
            try
            {
                using (TcpClient tcpClient = new TcpClient())
                {
                    const int maxBuffer = 100;

                    _logger.Debug("Connecting..");
                    tcpClient.Connect(server, port);
                    _logger.Debug("Connected");
 
                    Stream networkStream = tcpClient.GetStream();
 
                    byte[] sendBuffer = new ASCIIEncoding().GetBytes(command);
                    _logger.Debug("Transmitting..\n");
                    networkStream.Write(sendBuffer, 0, sendBuffer.Length);

                    _logger.Debug("Receive acknowledgement from server..");
                    byte[] receiveBuffer = new byte[maxBuffer];
                    int k = networkStream.Read(receiveBuffer, 0, maxBuffer);
 
                    for (int i = 0; i < k; i++)
                        _logger.Debug(Convert.ToChar(receiveBuffer[i]));
                 }
            }
            catch (Exception e)
            {
                _logger.Error(string.Format("Error: {0}", e.StackTrace));
            }
        }

        private void StartServer()
        {
            string app = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\shagent\bin\debug\shagent.exe");

            _logger.Debug("Starting server from:" + app);

            var processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = app;
            //processStartInfo.UseShellExecute = false;
            //processStartInfo.RedirectStandardOutput = false;
            

            _serverProcess = Process.Start(processStartInfo);
        }
    }
}