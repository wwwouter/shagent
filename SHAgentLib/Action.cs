using System;

namespace SHAgent
{
    public class Action
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string TheAction { get; private set; }
        public string Command { get; private set; }

        public static Action Parse(string action, IConfigurationManager configurationManager)
        {
            var parameters = action.Split(';');

            if (configurationManager.UseRemoteCommand)
            {
                if(!parameters[0].Equals("status", StringComparison.InvariantCultureIgnoreCase))
                    if (parameters.Length != 4)
                        throw new ArgumentException("Invalid parameters: usage: START;<username>;<password>;<command>");    
                else
                    if (parameters.Length != 3 && parameters.Length != 4)
                        throw new ArgumentException("Invalid parameters: usage: STATUS;<username>;<password>[;<command>]");
            }
                
            if (!configurationManager.UseRemoteCommand && parameters.Length != 3)
                throw new ArgumentException("Invalid parameters: usage: START/STATUS;<username>;<password>;<command>");

            var theAction = new Action
            {
                TheAction = parameters[0],
                Username = parameters[1],
                Password = parameters[2],
                Command = configurationManager.UseRemoteCommand && parameters.Length == 4 ? parameters[3] : configurationManager.Command
            };

            return theAction;
        }
    }
}