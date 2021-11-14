using ClementTodd.DataManagement;
using System.Collections.Generic;
using UnityEngine;

namespace ClementTodd.Localization
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

        public Dictionary<TextAsset, Dictionary<string, object>> parsedData = new Dictionary<TextAsset, Dictionary<string, object>>();

        public void Awake()
        {
            Instance = this;
        }

        public string GetString(TextAsset rawData, string key)
        {
            var data = JSONParser.ReadObject(rawData.text);

            if (data.ContainsKey(key))
            {
                var entry = data[key] as Dictionary<string, object>;

                if (entry.ContainsKey(LanguageKey))
                {
                    return entry[LanguageKey] as string;
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

        private Dictionary<string, object> ParseData(TextAsset rawData)
        {
            if (!parsedData.ContainsKey(rawData))
            {
                parsedData.Add(rawData, JSONParser.ReadObject(rawData.text));
            }
            return parsedData[rawData];
        }
    }
}