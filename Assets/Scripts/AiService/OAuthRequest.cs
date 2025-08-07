using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace AiService
{
    public class OAuthRequest : MonoBehaviour
    {

        public bool TokenExpired =>IsTokenExpired();
        public string IAMToken = "";
        public DateTime ExpiresAt;
    
        private const string IamTokenKey = "IamToken";
        private const string ExpiresAtKey = "ExpiresAt";

        public void Init()
        {
            var response = LoadFromPlayerPrefs();
            if (response == null || string.IsNullOrEmpty(response.iamToken)) return;
            IAMToken = response.iamToken;
            ExpiresAt = response.expiresAt;
        }

        public async UniTask<TokenResponse> SendRequest() {

            string url = "https://iam.api.cloud.yandex.net/iam/v1/tokens";
        
            // Тело запроса
            string jsonData = "{\"yandexPassportOauthToken\":\"y0_AgAAAAAOObUcAATuwQAAAAEbrkbiAAC5jKsac39AsIVr91Qitdq0dOixOQ\"}";

            // Создаем запрос
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Отправляем запрос и ждем ответа
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Response: " + request.downloadHandler.text);
                var response = JsonUtility.FromJson<TokenResponse>(request.downloadHandler.text);
                IAMToken = response.iamToken;
                ExpiresAt = response.expiresAt;
                SaveToPlayerPrefs();
                return response;
            }
            else
            {
                Debug.LogError("Error: " + request.error);
                throw new Exception("Error: " + request.error);
            }
        }
    
        private bool IsTokenExpired()
        {
            return IAMToken == "" || DateTime.UtcNow > ExpiresAt;
        }
        private void SaveToPlayerPrefs()
        {
            PlayerPrefs.SetString(IamTokenKey, IAMToken);
            PlayerPrefs.SetString(ExpiresAtKey, ExpiresAt.ToString("o")); // ISO 8601 формат
            PlayerPrefs.Save();
            Debug.Log("Token and expiration date saved to PlayerPrefs.");
        }
    
        private TokenResponse LoadFromPlayerPrefs()
        {
            string iamToken = PlayerPrefs.GetString(IamTokenKey, null);
            string expiresAtString = PlayerPrefs.GetString(ExpiresAtKey, null);

            if (string.IsNullOrEmpty(iamToken) || string.IsNullOrEmpty(expiresAtString))
            {
                Debug.LogWarning("No saved token or expiration date found in PlayerPrefs.");
                return null;
            }

            if (!DateTime.TryParse(expiresAtString, null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime expiresAt))
            {
                Debug.LogError("Failed to parse expiration date from PlayerPrefs.");
                return null;
            }

            return new TokenResponse
            {
                iamToken = iamToken,
                expiresAt = expiresAt
            };
        }
    }

    [Serializable]
    public class TokenResponse
    {
        public string iamToken;
        public DateTime expiresAt;
    }
}