using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace SHAgent.Tests
{
    [TestClass]
    public class GivenStatusCommandWithNoProcessRunning
    {
        [TestMethod]
        public void ShouldWriteDoneMessage()
        {
            var processManager = Substitute.For<IProcessManager>();
            var shConfigManager = Substitute.For<IConfigurationManager>();
            var messenger = Substitute.For<IMessenger>();

            shConfigManager.ExpectedSourceIpAddress.Returns("127.0.0.1");
            shConfigManager.ExpectedUserName.Returns("username");
            shConfigManager.ExpectedPassword.Returns("password");

            processManager.GetProcessOutput().Returns("ready!");

            CommandHandler commandHandler = new CommandHandler(processManager, shConfigManager, messenger);

            commandHandler.ExecuteCommand(Action.Parse("STATUS;username;password", shConfigManager));

            messenger.Received().SendMessage("DONE;ready!");
        }
    }
}