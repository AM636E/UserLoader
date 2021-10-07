using System;
using System.Collections.Generic;
using System.Text;

namespace UserLoader.Mq
{
    public interface IPublisher
    {
        void SendMessage(string message);
    }
}
