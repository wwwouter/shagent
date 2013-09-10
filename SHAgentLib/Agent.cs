using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using log4net;

namespace SHAgent
{
    public class Agent
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly IProcessManager _processManager;
        TcpListener listener;
        private ILog _logger = LogManager.GetLogger(typeof (Agent));

        public Agent(IConfigurationManager configurationManager, IProcessManager processManager)
        {
            _configurationManager = configurationManager;
            _processManager = processManager;
        }

        public void Start()
        {
            _logger.Debug("Obtaining server ip and port");

            IPAddress ipAddress;
            if (!string.IsNullOrEmpty(_configurationManager.ServerIpAddress))
                ipAddress = IPAddress.Parse(_configurationManager.ServerIpAddress);
            else
                ipAddress = IPAddress.Parse(FindIpAddress());

            var port = _configurationManager.Port;

            try
            {
                _logger.Debug(string.Format("starting listener on ipadress {0} and port {1}", ipAddress, port));

                listener = new TcpListener(ipAddress, port);
                listener.Start();

                _logger.Debug(string.Format("The server is running at port {0}...", port));
                _logger.Debug("The local End point is  :" + listener.LocalEndpoint);
                _logger.Debug("Waiting for a connection.....");

                AcceptSocket();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);

                if (listener != null) listener.Stop();
            }
        }

        private void AcceptSocket()
        {
            _logger.Debug("Accepting socket");

            Socket socket = listener.AcceptSocket();

            if (_configurationManager.SourceAddressCheckEnabled)
                ValidateSourceAddress(socket);

            _logger.Debug("Connection accepted from " + socket.RemoteEndPoint);

            Stream stream = new NetworkStream(socket);
            var sr = new StreamReader(stream);
            var sw = new StreamWriter(stream);
            sw.AutoFlush = true;

            while (true)
            {
                string commandInput = sr.ReadLine();

                if (string.IsNullOrEmpty(commandInput))
                    break;

                try
                {
                    _logger.Debug(string.Format("parsing command:" + commandInput));

                    Action action = Action.Parse(commandInput, _configurationManager);

                    var commandHandler = new CommandHandler(
                        _processManager,
                        _configurationManager,
                        new Messenger(sw));

                    commandHandler.ExecuteCommand(action);

                    break;
                }
                catch (Exception e)
                {
                    _logger.Error(e.Message, e);

                    sw.WriteLine("ERROR: {0}", e.Message);

                    break;
                }
            }

            _logger.Debug("Cleanup socket");

            socket.Disconnect(true);

            AcceptSocket();
        }

        private void ValidateSourceAddress(Socket socket)
        {
            var remoteEndPoint = socket.RemoteEndPoint.ToString();
            var remoteEndPointWithoutPort = remoteEndPoint.Substring(0, remoteEndPoint.LastIndexOf(":"));

            if (remoteEndPointWithoutPort != _configurationManager.ExpectedSourceIpAddress)
            {
                _logger.Warn(string.Format("Sourceaddress {0} requested a socket which has been denied", remoteEndPointWithoutPort));

                socket.Close();

                AcceptSocket();
            }
        }

        private string FindIpAddress()
        {
            string localIP = "?";

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }

            return localIP;
        }

        public void Stop()
        {
            _logger.Info("Stopping agent");

            try
            {
                listener.Stop();
                
                _logger.Info("Listener stopped");
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }
        }
    }
}