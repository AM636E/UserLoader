using System;

namespace UserLoader.Mq
{
    public interface IMqWorker
    {
        event EventHandler<MqMessage> OnMessage;
        void SendMessage(string message);
    }
}