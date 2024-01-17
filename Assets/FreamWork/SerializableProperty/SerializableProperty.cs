using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StringBoolDictionary : SerializableDictionary<string, bool>
{
    public StringBoolDictionary(StringBoolDictionary dict) {
        foreach (var kvp in dict)
        {
            this[kvp.Key] = kvp.Value;
        }
    }

    public StringBoolDictionary()
    {

    }

    public void Init() {
        Clear();
    }
}