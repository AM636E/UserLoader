using LanguageExt;

using UserLoader.Common;
using UserLoader.DbModel.Models;
using UserLoader.Mq;

namespace UserLoader.Operations
{
    public class MqUserWriter : IUserWriter
    {
        private readonly IMqWorker _mqWorker;
        private readonly ISerializer _serializer;

        public MqUserWriter(IMqWorker mqWorker, ISerializer serializer)
        {
            _mqWorker = mqWorker;
            _serializer = serializer;
        }

        public Try<UserModel> Insert(UserModel model) => () =>
        {
            _mqWorker.SendMessage(_serializer.Serialize(model));

            return model;
        };
    }
}
