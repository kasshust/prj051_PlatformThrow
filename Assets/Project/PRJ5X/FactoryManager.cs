using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FactoryManager : SingletonMonoBehaviourFast<FactoryManager>
{
    #region FactoryType オブジェクトとの紐づけ
    public enum UTILITY
    {
        UT000,
        UT001,
        UT002,
        UT003,
        UT004,
        UT005,
        UT006,
        UT007,
        UT008,
        UT009,
        UT010,
    }

    public enum PLAYER
    {
        PL000,
        PL001,
        PL002,
        PL003,
        PL004,
        PL005,
        PL006,
        PL007,
        PL008,
        PL009,
        PL010,
    }

    public enum ENEMY
    {
        EM000,
        EM001,
        EM002,
        EM003,
        EM004,
        EM005,
        EM006,
        EM007,
        EM008,
        EM009,

        EM010,
        EM011,
        EM012,
        EM013,
        EM014,
        EM015,
        EM016,
        EM017,
        EM018,
        EM019,

        EM020,
        EM021,
        EM022,
        EM023,
        EM024,
        EM025,
        EM026,
        EM027,
        EM028,
        EM029,

        EM030,
        EM031,
        EM032,
        EM033,
        EM034,
        EM035,
        EM036,
        EM037,
        EM038,
        EM039,

        EM040,
        EM041,
        EM042,
        EM043,
        EM044,
        EM045,
        EM046,
        EM047,
        EM048,
        EM049,
    }

    public enum BALL
    {
        BL000,
        BL001,
        BL002,
        BL003,
        BL004,
        BL005,
        BL006,
        BL007,
        BL008,
        BL009,

        BL010,
        BL011,
        BL012,
        BL013,
        BL014,
        BL015,
        BL016,
        BL017,
        BL018,
        BL019,

        BL020,
        BL021,
        BL022,
        BL023,
        BL024,
        BL025,
        BL026,
        BL027,
        BL028,
        BL029,
    }

    public enum ITEM
    {
        IM000,
        IM001,
        IM002,
        IM003,
        IM004,
        IM005,
        IM006,
        IM007,
        IM008,
        IM009,

        IM010,
        IM011,
        IM012,
        IM013,
        IM014,
        IM015,
        IM016,
        IM017,
        IM018,
        IM019,

        IM020,
        IM021,
        IM022,
        IM023,
        IM024,
        IM025,
        IM026,
        IM027,
        IM028,
        IM029,
    }

    public enum OBJECT
    {
        OB000,
        OB001,
        OB002,
        OB003,
        OB004,
        OB005,
        OB006,
        OB007,
        OB008,
        OB009,
        OB010,
    }

    public enum EFFECT
    {
        EF000,
        EF001,
        EF002,
        EF003,
        EF004,
        EF005,
        EF006,
        EF007,
        EF008,
        EF009,

        EF010,
        EF011,
        EF012,
        EF013,
        EF014,
        EF015,
        EF016,
        EF017,
        EF018,
        EF019,

        EF020,
        EF021,
        EF022,
        EF023,
        EF024,
        EF025,
        EF026,
        EF027,
        EF028,
        EF029,

        EF030,
        EF031,
        EF032,
        EF033,
        EF034,
        EF035,
        EF036,
        EF037,
        EF038,
        EF039,

        EF040,
        EF041,
        EF042,
        EF043,
        EF044,
        EF045,
        EF046,
        EF047,
        EF048,
        EF049,
    }
    #endregion

    [SerializeField]
    ActionGameBehaviorDataSet m_ActionGameBehaviorDataSet;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    //  Instantiate
    public T Create<T>(T original) where T : Object{
        return Instantiate<T>(original);
    }
    public T Create<T>(T original, Vector3 position, Quaternion rotation) where T : Object
    {
        return Instantiate<T>(original, position, rotation);
    }
    public T Create<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
    {
        return Instantiate<T>(original, position, rotation, parent);
    }

    public void ReleaseObject(UTILITY type, GameObject g) {
        // ObjectPool p = GetObjectPool(type);
        // p.Release(g);
    }
    public void ReleaseObject(PLAYER type, GameObject g)
    {
        // ObjectPool p = GetObjectPool(type);
        // p.Release(g);
    }
    public void ReleaseObject(ENEMY type, GameObject g)
    {
        // ObjectPool p = GetObjectPool(type);
        // p.Release(g);
    }
    public void ReleaseObject(ITEM type, GameObject g)
    {
        // ObjectPool p = GetObjectPool(type);
        // p.Release(g);
    }
    public void ReleaseObject(OBJECT type, GameObject g)
    {
        // ObjectPool p = GetObjectPool(type);
        // p.Release(g);
    }
    public void ReleaseObject(BALL type, GameObject g)
    {
        // ObjectPool p = GetObjectPool(type);
        // p.Release(g);
    }
    public void ReleaseObject(EFFECT type, GameObject g)
    {
        // ObjectPool p = GetObjectPool(type);
        // p.Release(g);
    }

    /*
    public T GetObject<T>(UTILITY type, Vector3 position, Quaternion rotation) where T : ActionGameUtility
    {
        return (T)GetObjectPool(type).GetObjectComponent<T>(position, rotation).Init();
    }
    public T GetObject<T>(PLAYER type, Vector3 position, Quaternion rotation) where T : PlatformPlayerBase
    {
        return (T)GetObjectPool(type).GetObjectComponent<T>(position, rotation).Init();
    }
    public T GetObject<T>(ENEMY type, Vector3 position, Quaternion rotation) where T : PlatformEnemyBase
    {
        return (T)GetObjectPool(type).GetObjectComponent<T>(position, rotation).Init();
    }

    public T GetObject<T>(BALL type, Vector3 position, Quaternion rotation) where T : ActionGameBullet
    {
        return (T)GetObjectPool(type).GetObjectComponent<T>(position, rotation).Init();
    }
    public T GetObject<T>(ITEM type, Vector3 position, Quaternion rotation) where T : ActionGameItem
    {
        return (T)GetObjectPool(type).GetObjectComponent<T>(position, rotation).Init();
    }
    public T GetObject<T>(OBJECT type, Vector3 position, Quaternion rotation) where T : ActionGameObject
    {
        return (T)GetObjectPool(type).GetObjectComponent<T>(position, rotation).Init();
    }
    public T GetObject<T>(EFFECT type, Vector3 position, Quaternion rotation) where T : ActionGameEffect
    {
        return (T)GetObjectPool(type).GetObjectComponent<T>(position, rotation).Init();
    }

    public T GetObject<T>(UTILITY type) where T : ActionGameUtility
    {
        return (T)GetObjectPool(type).GetObjectComponent<T>().Init();
    }
    public T GetObject<T>(PLAYER type) where T : PlatformPlayerBase
    {
        return (T)GetObjectPool(type).GetObjectComponent<T>().Init();
    }
    public T GetObject<T>(ENEMY type) where T : PlatformEnemyBase
    {
        return (T)GetObjectPool(type).GetObjectComponent<T>().Init();
    }
    public T GetObject<T>(BALL type) where T : ActionGameBullet
    {
        return (T)GetObjectPool(type).GetObjectComponent<T>().Init();
    }
    public T GetObject<T>(ITEM type) where T : ActionGameItem
    {
        return (T)GetObjectPool(type).GetObjectComponent<T>().Init();
    }
    public T GetObject<T>(OBJECT type) where T : ActionGameObject
    {
        return (T)GetObjectPool(type).GetObjectComponent<T>().Init();
    }
    public T GetObject<T>(EFFECT type) where T : ActionGameEffect
    {
        return (T)GetObjectPool(type).GetObjectComponent<T>().Init();
    }
    */

    /*
    public ObjectPool GetObjectPool(UTILITY type) {
        if (m_ObjectPoolDataSet.m_ObjectPoolUtilityDict.ContainsKey(type)) {
            return m_ObjectPoolDataSet.m_ObjectPoolUtilityDict[type];
        }
        return null;
    }
    public ObjectPool GetObjectPool(PLAYER type)
    {
        if (m_ObjectPoolDataSet.m_ObjectPoolPlayerDict.ContainsKey(type))
        {
            return m_ObjectPoolDataSet.m_ObjectPoolPlayerDict[type];
        }
        return null;
    }
    public ObjectPool GetObjectPool(ENEMY type)
    {
        if (m_ObjectPoolDataSet.m_ObjectPoolEnemyDict.ContainsKey(type))
        {
            return m_ObjectPoolDataSet.m_ObjectPoolEnemyDict[type];
        }
        return null;
    }
    public ObjectPool GetObjectPool(BALL type) {
        if (m_ObjectPoolDataSet.m_ObjectPoolBallDict.ContainsKey(type))
        {
            return m_ObjectPoolDataSet.m_ObjectPoolBallDict[type];
        }
        return null;
    }
    public ObjectPool GetObjectPool(ITEM type)
    {
        if (m_ObjectPoolDataSet.m_ObjectPoolItemDict.ContainsKey(type))
        {
            return m_ObjectPoolDataSet.m_ObjectPoolItemDict[type];
        }
        return null;
    }
    public ObjectPool GetObjectPool(OBJECT type)
    {
        if (m_ObjectPoolDataSet.m_ObjectPoolObjectDict.ContainsKey(type))
        {
            return m_ObjectPoolDataSet.m_ObjectPoolObjectDict[type];
        }
        return null;
    }
    public ObjectPool GetObjectPool(EFFECT type)
    {
        if (m_ObjectPoolDataSet.m_ObjectPoolEffectDict.ContainsKey(type))
        {
            return m_ObjectPoolDataSet.m_ObjectPoolEffectDict[type];
        }
        return null;
    }
    */
    
}
