﻿using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizeText : MonoBehaviour
{
    public TextAsset data;
    public string key;

    void Start()
    {
        GetComponent<Text>().text = LocalizationManager.Instance.GetString(data, key);
    }
}
