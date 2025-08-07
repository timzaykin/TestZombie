using System;

namespace AiService
{
    [Serializable]
    public class YandexGptRequest
    {
        public string modelUri;
        public YandexGptRequestOptions completionOptions;
        public Message[] messages;
    }

    [Serializable]
    public class YandexGptRequestOptions
    {
        public int maxTokens;
        public float temperature;

    }

    [Serializable]
    public class YandexGptAlternative
    {
        public Message message;
        public string status;
    }

    [Serializable]
    public class YandexGptUsage
    {
        public int inputTextTokens;
        public int completionTokens;
        public int totalTokens;
    }

    [Serializable]
    public class YandexGptResult
    {
        public YandexGptAlternative[] alternatives;
        public YandexGptUsage usage;
        public string modelVersion;
    }

    [Serializable]
    public class YandexGptResponse
    {
        public YandexGptResult result;
    }
}