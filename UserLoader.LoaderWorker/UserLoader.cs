using UserLoader.Common;
using UserLoader.DbModel.Models;
using UserLoader.Mq;
using UserLoader.Operations;

namespace UserLoader.LoaderWorker
{
    public class UserLoader
    {
        private IMqWorker _mqWorker;
        private IUserWriter _userWriter;
        private ISerializer _serializer;

        public UserLoader(IMqWorker mqWorker, IUserWriter userWriter, ISerializer serializer)
        {
            _mqWorker = mqWorker;
            _userWriter = userWriter;
            _serializer = serializer;
        }

        public void Start()
        {
            _mqWorker.OnMessage += _mqWorker_OnMessage;
        }

        private void _mqWorker_OnMessage(object sender, MqMessage e)
        {
            _userWriter.Insert(_serializer.Deserialize<UserModel>(e.Data));
        }
    }
}
