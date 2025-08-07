using System;
using System.Collections.Generic;
using AiService;
using Assets.Overtone.Scripts;
using Cysharp.Threading.Tasks;
using LeastSquares.Overtone;
using UnityEngine;

namespace Character
{
    public class Character : MonoBehaviour
    {
        [SerializeField] public string _baseDescription, _worldDescription;  
        [SerializeField] private TTSPlayer voicePlayer;
        private ApiServiceContainer _apiServiceContainer => ApiServiceContainer.Instance;
        private List<Message> _messageHistory;
        private float _reactionCooldown = 30f;
        private bool _isInputProcess;

        private void Start()
        {
            _messageHistory = new List<Message>();
            voicePlayer.source = GetComponent<AudioSource>();
            voicePlayer.Engine = FindObjectOfType<TTSEngine>();
            voicePlayer.Voice = GetComponent<TTSVoice>();
            CreateSystemMessage();
            ProcessInput("[Ты видишь приключенца который тебе явно чем то не нравится, скажи ему что-то оскорбительное!]");
        }
    
        public void TryToProcessInput(string playerInput)
        {
            if (_isInputProcess) return;
            ProcessInput(playerInput).Forget();
        }

        private void CreateSystemMessage()
        {
            AddMessage(new Message(){role = "system", text = $"{_worldDescription} {_baseDescription}"});
        }
    
        public void AddMessage(Message message)
        {
            _messageHistory.Add(message);
            Debug.Log($"Сообщение добавлено: {message}");
        }
    
        public async UniTask<string> ProcessInput(string playerInput)
        {
            _isInputProcess = true;
            _messageHistory.Add(new Message(){role = "user", text = playerInput});
            var response = await _apiServiceContainer.SendMessages(_messageHistory.ToArray());
            _messageHistory.Add(response);
            await voicePlayer.Speak(response.text);
            await UniTask.Delay(TimeSpan.FromSeconds(_reactionCooldown), ignoreTimeScale: false);
            _isInputProcess = false;
            return response.text;
        }
    
    }
}

