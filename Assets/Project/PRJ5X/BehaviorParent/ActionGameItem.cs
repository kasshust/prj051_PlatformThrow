using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionGameItem : ActionGameBehavior<ActionGameItem> { 

    [SerializeField] public FactoryManager.ITEM m_ItemEnum;
    private PlatformPlayerBase m_TargetPlayer;
    [SerializeField] LayerMask m_GroundMask;

    private Vector2 m_Velocity = Vector2.zero;
    private bool m_Chase = false;
    private float m_ChaseWaitCount;
    private float m_LiveCount;
    private bool  m_IsLanding = false;
    private int   m_UpdateCounter         = 0;

    static private int      m_UpdateCounterInterval = 15;
    static private float    m_LandCheckDistance     = 0.3f;
    static private float    m_ChaseNearth           = 3.0f;
    static private float    m_GetNearth             = 0.05f;
    static private float    m_MaxVelocity           = 15.0f;
    static private float    m_ChaseWaitCountInit    = 0.5f;
    static private float    m_LiveCountInit         = 30.0f;
    static private float    m_Gravity               = 0.07f;

    float m_Angle = 0.0f;

    /*
    [SerializeField] private Vector3 _centerOffset = Vector3.zero;
    [SerializeField, Range(-1.0f, 1.0f)] private float _ForwardScale = 1.0f;
    [SerializeField, Range(-1.0f, 1.0f)] private float _UpScale = 0.0f;
    [SerializeField, Range(-1.0f, 1.0f)] private float _RightScale = 0.0f;
    private Vector3 _axis = Vector3.zero;
    */
    [SerializeField] private float _period = 2;



    override public void ReleaseObject()
    {
        m_FactoryManager.ReleaseObject(m_ItemEnum, this.gameObject);
    }

    virtual protected void Update()
    {

        TickLive();
        TickChaseCount();
        if(m_UpdateCounter % m_UpdateCounterInterval == 0) CheckChase();
        m_UpdateCounter++;

        // プレイヤー追跡と放置状態
        if (m_Chase)
        {
            ChaseTarget();
            if (IsNearTarget(m_GetNearth))
            {
                Got();
                ReleaseObject();
            }
        }
        else {
            if (!m_IsLanding)
            {
                if (Physics2D.Raycast(transform.position, Vector2.down, m_LandCheckDistance, m_GroundMask))
                {
                    m_IsLanding = true;
                } else {
                    m_Velocity.y -= m_Gravity;
                }
                    m_Velocity = m_Velocity.normalized * Mathf.Min(m_Velocity.magnitude, m_MaxVelocity);
            }
            else m_Velocity.y = 0.0f;

            m_Velocity *= 0.98f;
        }

        transform.position += (Vector3)m_Velocity * Time.deltaTime;
        Rotate();
    }


    private void Rotate() {
        transform.rotation *= Quaternion.AngleAxis(12.0f , Vector3.one);
        /*
        _axis = transform.forward * _ForwardScale + transform.up * _UpScale + transform.right * _RightScale;

        transform.RotateAround(
            transform.position + _centerOffset,
            _axis.normalized,
            360 / _period * Time.deltaTime
        );
        */
    }

    private void TickLive() {
        if (m_LiveCount > 0.0f)
        {
            m_LiveCount = Mathf.Max(m_LiveCount - Time.deltaTime, 0.0f);
        }
        else
        {
            ReleaseObject();
        }
    }

    private void TickChaseCount() {
        if (m_ChaseWaitCount > 0.0f)
        {
            m_ChaseWaitCount = Mathf.Max(m_ChaseWaitCount - Time.deltaTime, 0.0f);
        }
    }

    private void CheckChase()
    {
        if (!m_Chase && m_ChaseWaitCount <= 0.0f)
        {
            if (IsNearTarget(m_ChaseNearth)) m_Chase = true;
        }
    }

    public override ActionGameItem CreateInit()
    {
        m_Velocity.x = Random.Range(-2.0f, 2.0f);
        m_Velocity.y = Random.Range(0.0f, 7.0f);
        m_ChaseWaitCount    = m_ChaseWaitCountInit;
        m_LiveCount         = m_LiveCountInit;
        m_Chase             = false;
        m_IsLanding         = false;
        m_Angle             = 0.0f;
        m_UpdateCounter     = Random.Range(0, m_UpdateCounterInterval);

        return this;
    }

    abstract protected void Got();

    protected override void Wake()
    {
        m_TargetPlayer = PlayerManager.Instance.m_Player;
    }

    Vector2 m_Temp;
    Vector2 m_Normalized;
    protected void ChaseTarget() {
        if (m_TargetPlayer != null) {
            m_Temp = m_TargetPlayer.transform.position - transform.position;
            m_Normalized = m_Temp.normalized;
            m_Velocity += m_Normalized / 7.0f;
            m_Velocity = m_Normalized * Mathf.Min(m_Velocity.magnitude, m_MaxVelocity);
        }
    }

    
    protected bool IsNearTarget(float th) {
        if (m_TargetPlayer != null)
        {
            float sqrdist = Vector3.SqrMagnitude(m_TargetPlayer.transform.position - transform.position);
            if (sqrdist < th) return true;
        }
        return false;
    }
}
