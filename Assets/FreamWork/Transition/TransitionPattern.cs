using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor;

[Serializable]
[CreateAssetMenu(fileName = "TransitionPattern", menuName = "Transition/TransitionPattern")]
public class TransitionPattern : ScriptableObject
{
    [SerializeField]
    public TransitionDictionary tranitionPattern = null;

    public int Count() {
        return tranitionPattern.Count;
    }

    public GameObject Get(int index) {
        return tranitionPattern.Values.ToList()[index];
    }

    public GameObject Get(string name) {
        return tranitionPattern[name];
    }

    public void CreateItemTransitionDictionary(TransitionDictionary tranitionPattern)
    {
        this.tranitionPattern = tranitionPattern;
    }
}

[Serializable]
public class TransitionDictionary : SerializableDictionary<string, GameObject>
{

}