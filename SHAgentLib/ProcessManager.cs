using System;
using System.Diagnostics;
using System.Linq;
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

            string command = RemovePathAndExtension(action.Command);

            Process process = Process.GetProcesses().FirstOrDefault(pp => pp.ProcessName.StartsWith(command, StringComparison.InvariantCultureIgnoreCase));

            bool isProcessRunning = process != null;

            _logger.Debug(string.Format("Proces with processname {0} is currently {1}running", command, !isProcessRunning ? "not " : ""));

            return isProcessRunning;
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

        private string RemovePathAndExtension(string command)
        {
            var result = command.Substring(_configurationManager.Command.LastIndexOf(@"\") + 1);

            return result.Substring(0, result.LastIndexOf("."));
        }
    }
}