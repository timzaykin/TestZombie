using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace AiService
{
    public class MistralRequest : ModelRequest
    {
        private string token => "";
        private string url = "https://api.mistral.ai/v1/agents/completions";
        private string agentId = "ag:cfdd468c:20241221:untitled-agent:459d7f75";

        private Message ToMessage(MistralCompletionResponse responseStr)
        {
            Debug.Log(responseStr);
            return responseStr.choices[0].message.ToMessage();
        }

        protected override string GenerateJson(Message[] messagesHistory)
        {
            var msgHistory = new List<MistralMessage>();
            Array.ForEach(messagesHistory,x => msgHistory.Add(new MistralMessage(x)));
        
            var data = new MistralGptRequest {messages = msgHistory.ToArray(), agent_id = agentId};
            var json = JsonUtility.ToJson(data);
            Debug.Log(json);
            return json;
        }

        protected override async UniTask<Message> PostRequest(string jsonData)
        {
            var request = CreateWebRequest(jsonData);
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                return ToMessage(ParseGptResponse.Parse<MistralCompletionResponse>(request.downloadHandler.text));
            }
            else
            {
                Debug.LogError("Error: " + request.error);
                throw new Exception("Error: " + request.error);
            }
        }

        protected override UnityWebRequest CreateWebRequest(string jsonData)
        {
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", $"application/json");
            request.SetRequestHeader("Authorization", $"Bearer {token}");

            return request;
        }
    }

    [Serializable]
    public class MistralGptRequest
    {
        //public string model;
        public MistralMessage[] messages;
        public string agent_id;
    }

    [Serializable]
    public class Choice
    {
        public int index;
        public MistralMessage message;
        public string finish_reason;
    }

    [Serializable]
    public class Usage
    {
        public int prompt_tokens;
        public int total_tokens;
        public int completion_tokens;
    }

    [Serializable]
    public class MistralCompletionResponse
    {
        public string id;
        public string @object; // Используем @ перед ключевым словом
        public long created;
        public string model;
        public List<Choice> choices;
        public Usage usage;
    }

    [Serializable]
    public class MistralMessage
    {
        public string role;
        public string content ;

        public MistralMessage(Message msg)
        {
            role = msg.role;
            content = msg.text;
        }

        public Message ToMessage()
        {
            return new Message() {role = this.role, text = content};
        }
    }
}