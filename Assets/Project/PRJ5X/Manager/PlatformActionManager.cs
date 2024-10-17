using UnityEngine;

public class PlatformActionManager : SingletonMonoBehaviourFast<PlatformActionManager>
{
    [System.Serializable]
    public enum AttackSet
    {
        All,
        Player,
        EnemyA,
        EnemyB,
        EnemyC,
    }

    [System.Serializable]
    public enum AttackType
    {
        Default,
        Sword,
        Gun
    }

    [System.Serializable]
    public struct AttackInfo
    {
        [Header("基本情報")]

        public AttackSet                             AttackSet;
        public AttackType                           AttackType;
        [Range(0,100)]      public float            DamageValue;
        [Range(0, 100)]     public float            FlirtEndure;
        public bool                                 IsRadiationDirection;   //中心から放射ベクトルに方向を決定する
        public Vector2                              Direction;
        [Range(-100, 100)]  public float            ImpactValue;
        public bool                                 OverriteVelocity;
        public bool                                 ZeroGravity;
        public float                                ZeroGravityTime;

        [Header("ヒット時エフェクト")]

        public bool HitContinue;

        public bool                                 CreateHitEffect;
        public FactoryManager.EFFECT                HitEffect;
        public FMODUnity.EventReference             HitSound;
        public bool                                 HitStop;
        [Range(0, 5)]       public float            HitStopTime;

        [Header("ヒット時ポストエフェクト")]
        public bool                                 RunHitPostEffect;
        // public VFXController.PostVFX                PostVFXType;
        public float                                PostDuration;
        public float                                PostMagnitude;
    }

    [System.Serializable]
    public struct ReplyInfo {
        public bool m_Hit;
        public bool m_Counter;
    }

    [System.Serializable]
    public struct BaseSenderInfo {

        public SenderForm Form;
        public CircleSenderInfo CircleSenderInfo;
        public RectSenderInfo   RectSenderInfo;

        [Range(0, 10)]  
        public float            Time;
        public bool             UntilMotionEnd;

        public Vector3          LoacalPosition;
        public bool             isCreateInParentTransform;

        [ReadOnly] public Transform        ParentTransform;

        public Quaternion       Rotation;
    }

    [System.Serializable]
    public enum SenderForm
    {
        Circle,
        Rect
    }

    [System.Serializable]
    public struct CircleSenderInfo {
        [Range(0, 10)] 
        public float                Radius;
    }

    [System.Serializable]
    public struct RectSenderInfo
    {
        [Range(0, 10)] public float            Width;
        [Range(0, 10)] public float            Height;
        public float            Angle;
    }

    [SerializeField]
    FactoryManager m_FactoryManager;
    public BehaviorImpactSender m_ImapctSenderPrefab;

    // Experiment
    private bool m_UsePool = true;

    override protected void Awake()
    {
        base.Awake();
        if (m_ImapctSenderPrefab == null) Debug.LogError("Please Set ImpactSenderPrefab");

        FactoryManager m_FactoryManager = FactoryManager.Instance;
    }

    private BehaviorImpactSender TryCreateFromPool(ref BaseSenderInfo baseInfo)
    {
        // return m_FactoryManager.GetObject<BehaviorImpactSender>(FactoryManager.UTILITY.UT000, baseInfo.LoacalPosition, Quaternion.identity);
        return null;
    }

    private BehaviorImpactSender TryCreateFromPool()
    {
        // return m_FactoryManager.GetObject<BehaviorImpactSender>(FactoryManager.UTILITY.UT000);
        return null;
    }

    private BehaviorImpactSender InstantiateImpactSender(ref BaseSenderInfo baseInfo) {
        BehaviorImpactSender g = null;
        if (baseInfo.ParentTransform == null)
        {
            if(m_UsePool) g = TryCreateFromPool(ref baseInfo);
            if (g == null) g = m_FactoryManager.Create(m_ImapctSenderPrefab, baseInfo.LoacalPosition, Quaternion.identity);
        }
        else
        {
            if (m_UsePool) g = TryCreateFromPool();
            if (g == null) g = m_FactoryManager.Create(m_ImapctSenderPrefab);
            
            // 親トランスフォームの下に生成するか否か
            if (baseInfo.isCreateInParentTransform)
            {
                g.transform.parent = baseInfo.ParentTransform;
                g.transform.localPosition = baseInfo.LoacalPosition;
            }
            else {
                g.transform.position = baseInfo.ParentTransform.position + baseInfo.LoacalPosition;
            }
        }

        return g;
    }

    public void CreateImpactSender(ref AttackInfo attackinfo, ref BaseSenderInfo baseInfo, ActionGameCharacterBase characterBase, Animator animator = null) 
    {
        BehaviorImpactSender g = InstantiateImpactSender(ref baseInfo);

        if (g == null) { Debug.Log("This ImpactSender has no BehaviorImpactSender Script"); return; }

        g.InitAllParam(attackinfo, baseInfo, characterBase, animator);
    }
    
    public void CreateImpactSender(ref AttackInfo attackinfo , ref BaseSenderInfo baseInfo, Animator animator = null) {

        BehaviorImpactSender g = InstantiateImpactSender(ref baseInfo);

        if (g == null) { Debug.Log("This ImpactSender has no BehaviorImpactSender Script"); return; }

        g.InitAllParam(attackinfo, baseInfo, null, animator);
    }


    // 簡易版Sender
    PlatformActionManager.AttackInfo m_TempAttackInfo;
    PlatformActionManager.BaseSenderInfo m_TempBaseInfo;
    public void CreateImpactSender(ImpactSenderDataSet data, Transform Parent)
    {
        m_TempAttackInfo = data.AttackInfo;
        m_TempBaseInfo = data.BaseSenderInfo;
        m_TempBaseInfo.ParentTransform = Parent;

        CreateImpactSender(ref m_TempAttackInfo,ref m_TempBaseInfo);
    }

}
