using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

namespace ClementTodd
{
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance { get; private set; }

        public enum Language
        {
            English,
            Test
        }
        public Language language = Language.English;

        private string LanguageKey
        {
            get
            {
                switch (language)
                {
                    case Language.English:
                        return "EN";

                    case Language.Test:
                        return "TEST";

                    default:
                        return null;
                }
            }
        }

        public Dictionary<TextAsset, JSONObject> parsedData = new Dictionary<TextAsset, JSONObject>();

        public void Awake()
        {
            Instance = this;
        }

        public string GetString(TextAsset rawData, string key)
        {
            JSONObject data = ParseData(rawData);

            if (data.HasKey(key))
            {
                JSONObject entry = data[key].AsObject;

                if (entry.HasKey(LanguageKey))
                {
                    return entry[LanguageKey].Value;
                }
                else
                {
                    Debug.LogErrorFormat("Language '{0}' not set for key '{1}' in text database.", LanguageKey, key);
                    return "TRANSLATION NOT FOUND";
                }
            }
            else
            {
                Debug.LogErrorFormat("Key '{0}' not found in text database.", key);
                return "KEY NOT FOUND";
            }
        }

        private JSONObject ParseData(TextAsset rawData)
        {
            if (!parsedData.ContainsKey(rawData))
            {
                parsedData.Add(rawData, JSON.Parse(rawData.text).AsObject);
            }
            return parsedData[rawData];
        }
    }
}