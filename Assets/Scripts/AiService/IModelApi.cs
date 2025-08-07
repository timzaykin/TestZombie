using Cysharp.Threading.Tasks;

namespace AiService
{
    public interface IModelApi
    {
        void Init();
        UniTask<Message> SendMessages(Message[] _messageHistory);
    }
}