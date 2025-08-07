using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace AiService
{
    public class ApiServiceContainer : MonoBehaviour
    {
        public static ApiServiceContainer Instance { get; private set; }

        private IModelApi _modelApi;
        public void Start()
        {
            _modelApi = GetComponent<IModelApi>();
        
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }

            _modelApi.Init();
        }
    
        public async UniTask<Message> SendMessages(Message[] _messageHistory)
        {
            return await _modelApi.SendMessages(_messageHistory);
        }
    
#if UNITY_EDITOR
        [MenuItem("MyMenu/YandexForceAuth")]
        public static void ForceOAuth()
        {
            ((YandexApiService)Instance._modelApi).AuthRequest();
        }
#endif
        public T GetModel<T>()
        {
            return (T)_modelApi;
        }
    }
}