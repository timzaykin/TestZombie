using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace AiService
{
    public abstract class ModelRequest : MonoBehaviour
    {
        public async UniTask<Message> SendMessagesRequest(Message[] _messagesHistory)
        {
            var json = GenerateJson(_messagesHistory);
            return await PostRequest(json);
        }
        
        protected abstract string GenerateJson(Message[] _messagesHistory);
        protected abstract  UniTask<Message> PostRequest(string jsonData);
        protected abstract UnityWebRequest CreateWebRequest(string jsonData);
    }
}