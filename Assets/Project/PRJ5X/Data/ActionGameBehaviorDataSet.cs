using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionGameDataSet", menuName = "ActionGameObject/ActionGameDataSet")]
public class ActionGameBehaviorDataSet : ScriptableObject
{

    [SerializeField] public ActionGameUtilityDictionary     m_ActionGameUtilityDict;
    [SerializeField] public ActionGamePlayerictionary       m_ActionGamePlayerDict;
    [SerializeField] public ActionGameEnemyDictionary       m_ActionGameEnemyDict;
    [SerializeField] public ActionGameBallDictionary        m_ActionGameBallDict;
    [SerializeField] public ActionGameItemDictionary        m_ActionGameItemDict;
    [SerializeField] public ActionGameObjectDictionary      m_ActionGameObjectDict;
    [SerializeField] public ActionGameEffectDictionary      m_ActionGameEffectDict;


}

[System.Serializable]
public class ActionGameUtilityDictionary : SerializableDictionary<FactoryManager.UTILITY, ActionGameUtility> { }

[System.Serializable]
public class ActionGamePlayerictionary : SerializableDictionary<FactoryManager.PLAYER, PlatformPlayerBase> { }

[System.Serializable]
public class ActionGameEnemyDictionary : SerializableDictionary<FactoryManager.ENEMY, PlatformEnemyBase> { }

[System.Serializable]
public class ActionGameBallDictionary : SerializableDictionary<FactoryManager.BALL, ActionGameBullet> { }

[System.Serializable]
public class ActionGameItemDictionary : SerializableDictionary<FactoryManager.ITEM, ActionGameItem> { }

[System.Serializable]
public class ActionGameObjectDictionary : SerializableDictionary<FactoryManager.OBJECT, ActionGameObject> { }

[System.Serializable]
public class ActionGameEffectDictionary : SerializableDictionary<FactoryManager.EFFECT, ActionGameEffect> { }