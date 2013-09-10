using System;
using log4net;

namespace SHAgent
{
    public class CommandHandler
    {
        private readonly IProcessManager _processManager;
        private readonly IConfigurationManager _shConfigManager;
        private readonly IMessenger _messenger;
        private ILog _logger = LogManager.GetLogger(typeof (CommandHandler));

        public CommandHandler(IProcessManager processManager, IConfigurationManager shConfigManager, IMessenger messenger)
        {
            _processManager = processManager;
            _shConfigManager = shConfigManager;
            _messenger = messenger;
        }

        public void ExecuteCommand(Action action)
        {
            _logger.Debug("Executing command");

            ValidateCredentials(action);

            if (action.TheAction.Equals("START", StringComparison.InvariantCultureIgnoreCase))
                Start(action);
            else if (action.TheAction.Equals("STATUS", StringComparison.InvariantCultureIgnoreCase))
                Status(action);
        }

        private void ValidateCredentials(Action action)
        {
            _logger.Debug("Validating credentials");

            if (action.Username != _shConfigManager.ExpectedUserName || action.Password != _shConfigManager.ExpectedPassword)
                throw new Exception("invalid credentials");
        }

        private void Start(Action action)
        {
            _logger.Debug("Executing start command");

            if (_processManager.IsProcessRunning(action))
                throw new Exception("Process is allready running.");

            _processManager.StartProcess(action);
            _messenger.SendMessage("STARTED");
        }

        private void Status(Action action)
        {
            _logger.Debug("Executing status command");

            if(_processManager.IsProcessRunning(action))
                _messenger.SendMessage("BUSY");
            else
                _messenger.SendMessage("DONE;" + _processManager.GetProcessOutput());
        }
    }
}