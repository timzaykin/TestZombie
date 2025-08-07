using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AiService
{
    public class YandexApiService : MonoBehaviour, IModelApi
    {
        public string IAMToken => _oAuthRequest.IAMToken;
        
        [SerializeField] private OAuthRequest _oAuthRequest;
        [SerializeField] private ModelRequest _gptRequest; 
    
        public void Init()
        {
            _oAuthRequest.Init();
            if (_oAuthRequest.TokenExpired)
            {
                AuthRequest();
            }
        }

        public void AuthRequest()
        {
            _oAuthRequest.SendRequest();
        }

        public async UniTask<Message> SendMessages(Message[] _messageHistory)
        {
            return await _gptRequest.SendMessagesRequest(_messageHistory);
        }
    }
}