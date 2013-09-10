using System.IO;

namespace SHAgent
{
    public class Messenger : IMessenger
    {
        private readonly StreamWriter _streamWriter;

        public Messenger(StreamWriter streamWriter)
        {
            _streamWriter = streamWriter;
        }

        public void SendMessage(string message)
        {
            _streamWriter.WriteLine(message);
        }
    }
}