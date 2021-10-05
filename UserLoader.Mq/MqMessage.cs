namespace UserLoader.Mq
{
    public class MqMessage
    {
        public MqMessage(string rawData)
        {
            Data = rawData;
        }

        public string Data { get; }
    }
}