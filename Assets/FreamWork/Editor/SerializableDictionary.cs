using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

[CustomPropertyDrawer(typeof(TransitionDictionary))]
[CustomPropertyDrawer(typeof(StringBoolDictionary))]
[CustomPropertyDrawer(typeof(FieldSpriteDictionary))]
[CustomPropertyDrawer(typeof(ActDictionary))]

[CustomPropertyDrawer(typeof(GeneralFlagSet))]
[CustomPropertyDrawer(typeof(GeneralFlagDictionary))]

[CustomPropertyDrawer(typeof(ActionGameUtilityDictionary))]
[CustomPropertyDrawer(typeof(ActionGamePlayerictionary))]
[CustomPropertyDrawer(typeof(ActionGameEnemyDictionary))]
[CustomPropertyDrawer(typeof(ActionGameBallDictionary))]
[CustomPropertyDrawer(typeof(ActionGameItemDictionary))]
[CustomPropertyDrawer(typeof(ActionGameObjectDictionary))]
[CustomPropertyDrawer(typeof(ActionGameEffectDictionary))]

public class SerializableDictionary : SerializableDictionaryPropertyDrawer
{

}


