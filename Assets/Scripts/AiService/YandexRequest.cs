using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace AiService
{
    public class YandexRequest : ModelRequest
    {

        private string folderId = "";
        private string iamToken => ApiServiceContainer.Instance.GetModel<YandexApiService>().IAMToken;
        private string url = "https://llm.api.cloud.yandex.net/foundationModels/v1/completion";

        private Message ToMessage(YandexGptResponse responseStr)
        {
            var alternatives = responseStr.result.alternatives;
            return alternatives[^1].message;
        }

        protected override string GenerateJson(Message[] _messagesHistory)
        {
            var data = new YandexGptRequest();
            data.modelUri = "gpt://b1gaknnrlpmhjan1fs0d/yandexgpt/rc";
            data.completionOptions = new YandexGptRequestOptions {maxTokens = 500, temperature = 0.8f};
            data.messages = _messagesHistory;

            var json = JsonUtility.ToJson(data);
            Debug.Log(json);
            return json;
        }

        protected override async UniTask<Message> PostRequest(string jsonData)
        {

 
            var request = CreateWebRequest(jsonData);

      
            await request.SendWebRequest();

            // Проверяем результат
            if (request.result == UnityWebRequest.Result.Success)
            {
                return ToMessage(ParseGptResponse.Parse<YandexGptResponse>(request.downloadHandler.text));
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

            // Устанавливаем заголовки
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {iamToken}");
            request.SetRequestHeader("x-folder-id", folderId);
            return request;
        }
    }
}