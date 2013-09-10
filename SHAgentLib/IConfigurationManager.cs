using System;

namespace SHAgent
{
    public interface IConfigurationManager
    {
        int Port { get; }
        string ExpectedSourceIpAddress { get; }
        string ExpectedUserName { get; }
        string ExpectedPassword { get; }
        string Command { get; }
        string ServerIpAddress { get; }
        bool SourceAddressCheckEnabled { get; }
        bool UseRemoteCommand { get; }
    }
}