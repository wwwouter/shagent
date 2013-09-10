using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace SHAgent.Tests
{
    [TestClass]
    public class GivenStatusCommandWithRemoteCommandButNoCommandProvided
    {
        [TestMethod]
        public void ShouldReceiveDoneMessage()
        {
            var processManager = Substitute.For<IProcessManager>();
            var shConfigManager = Substitute.For<IConfigurationManager>();
            var messenger = Substitute.For<IMessenger>();

            shConfigManager.ExpectedSourceIpAddress.Returns("127.0.0.1");
            shConfigManager.ExpectedUserName.Returns("username");
            shConfigManager.ExpectedPassword.Returns("password");
            shConfigManager.UseRemoteCommand.Returns(true);

            processManager.GetProcessOutput().Returns("ready!");

            CommandHandler commandHandler = new CommandHandler(processManager, shConfigManager, messenger);

            Action action = Action.Parse("STATUS;username;password", shConfigManager);
            commandHandler.ExecuteCommand(action);

            messenger.Received().SendMessage("DONE;ready!");
        }
    }
}