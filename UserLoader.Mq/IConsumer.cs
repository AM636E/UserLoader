using System;

namespace UserLoader.Mq
{
    public interface IConsumer
    {
        void Start();
        event EventHandler<MqMessage> OnMessage;
    }
}
