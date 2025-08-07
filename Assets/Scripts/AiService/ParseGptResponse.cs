using UnityEngine;

namespace AiService
{
    public class ParseGptResponse : MonoBehaviour
    {
        public static T Parse<T>(string jsonResponse)
        {
            T response = JsonUtility.FromJson<T>(jsonResponse);
            return response;
        }
    }
}