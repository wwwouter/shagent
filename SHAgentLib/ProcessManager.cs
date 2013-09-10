using System.Diagnostics;
using System.Text;
using log4net;

namespace SHAgent
{
    public class ProcessManager : IProcessManager
    {
        private readonly IConfigurationManager _configurationManager;
        private ILog _logger = LogManager.GetLogger(typeof (ProcessManager));
        private Process _process;
        private StringBuilder processOutput = new StringBuilder();

        public ProcessManager(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public void StartProcess(Action action)
        {
            _logger.Debug(string.Format("Starting process:{0}", _configurationManager.Command));

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = action.Command,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                //CreateNoWindow = true
            };

            _process = new Process();
            _process.StartInfo = startInfo;

            _process.StartInfo.FileName = action.Command;
            _process.Start();
        }

        public bool IsProcessRunning(Action action)
        {
            _logger.Debug("Checking if process is running");

            bool isRunning = _process != null && !_process.HasExited;

            _logger.Debug(string.Format("Proces is currently {0}running", isRunning ? "not " : ""));

            return isRunning;
        }

        public string GetProcessOutput()
        {
            while (!_process.StandardOutput.EndOfStream)
            {
                string line = _process.StandardOutput.ReadLine();

                processOutput.AppendLine(line);
            }

            return processOutput.ToString();
        }
    }
}