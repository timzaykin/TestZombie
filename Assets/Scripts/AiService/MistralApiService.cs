using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AiService
{
    public class MistralApiService : MonoBehaviour, IModelApi
    {
        [SerializeField] private ModelRequest _gptRequest;
        public void Init()
        {
        
        }

        public async UniTask<Message> SendMessages(Message[] _messageHistory)
        {
            return await _gptRequest.SendMessagesRequest(_messageHistory);
        }
    }
}