using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace SHAgent.Tests
{
    [TestClass]
    public class GivenStartCommandWithCorrectParametersAndNoProcessRunning
    {
        [TestMethod]
        public void ShouldStartProcess()
        {
            var processManager = Substitute.For<IProcessManager>();
            var shConfigManager = Substitute.For<IConfigurationManager>();
            shConfigManager.ExpectedSourceIpAddress.Returns("127.0.0.1");
            shConfigManager.ExpectedUserName.Returns("username");
            shConfigManager.ExpectedPassword.Returns("password");

            CommandHandler commandHandler = new CommandHandler(processManager, shConfigManager, Substitute.For<IMessenger>());

            commandHandler.ExecuteCommand(Action.Parse("START;username;password", shConfigManager));

            processManager.Received().StartProcess(Arg.Any<Action>());
        }
    }

    [TestClass]
    public class GivenStartCommandWithProcessRunning
    {
        [TestMethod]
        public void ShouldNotStartProcess()
        {
            var processManager = Substitute.For<IProcessManager>();
            processManager.IsProcessRunning(Arg.Any<Action>()).Returns(true);

            var shConfigManager = Substitute.For<IConfigurationManager>();
            shConfigManager.ExpectedSourceIpAddress.Returns("127.0.0.1");
            shConfigManager.ExpectedUserName.Returns("username");
            shConfigManager.ExpectedPassword.Returns("password");

            CommandHandler commandHandler = new CommandHandler(processManager, shConfigManager, Substitute.For<IMessenger>());

            try
            {
                commandHandler.ExecuteCommand(Action.Parse("START;username;password", shConfigManager));
            }
            catch (Exception e)
            {
                e.Message.Should().Be("Process is allready running.");
            }
            processManager.DidNotReceive().StartProcess(Arg.Any<Action>());
        }
    }
}
