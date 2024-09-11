using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixeye.Unity;

public abstract class PlatformEnemyBase : PlatformCharacterBase {

    // [SerializeField, Foldout("PlatformEnemyBase Param")]
    // public CharModelObject m_CharModelObj;
    [SerializeField, Foldout("PlatformEnemyBase Param", true)]
    public FactoryManager.ENEMY m_EnemyEnum;
    public bool m_ForceDestroy = false;
    [SerializeField,ReadOnly, Foldout("PlatformEnemyBase Param")] float m_TargetVelocity;

    private static int m_EffectNum = 4;

    [SerializeField]
    LayerMask RevCharMask;

    protected override void Update()
    {
        base.Update();
    }

    public override void Dead()
    {
        // SpawnMoney();
        CreateEffect();
    }

    Vector3 m_RandomRange;
    private void CreateEffect()
    {
        for (int i = 0; i < m_EffectNum; i++)
        {
            m_RandomRange.Set(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0.0f);
            // ActionGameEffect o = FactoryManager.Instance.GetObject<ActionGameEffect>(FactoryManager.EFFECT.EF030, transform.position + m_RandomRange, Quaternion.identity);
            // o.SetEffectInfo(Vector2.zero, transform.position);
        }
    }

    private void SpawnMoney() {
        if (m_PlatformStatusObject != null) {
            // PlatformItemManager.Instance.SpawnMoney(m_PlatformStatusObject.m_PlatformStatus.Money, transform.position);
        }
    }

    public void Activate(bool b) {
        gameObject.SetActive(b);
        // if (b && m_CharModelObj != null) m_CharModelObj.InitAllParts();
    }

    public override ActionGameCharacterBase CreateInit()
    {
        base.CreateInit();
        // if(m_CharModelObj != null) m_CharModelObj.InitAllParts();
        return this;
    }

    override public void ReleaseObject()
    {
        if (m_ForceDestroy) {
            Destroy(gameObject);
            return;
        }

        // m_CharModelObj.InitAllParts();
        m_FactoryManager.ReleaseObject(m_EnemyEnum, this.gameObject);
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        /*
        if (LayerMaskExtensions.Contains(RevCharMask, collision.gameObject.layer))
        {
            Vector2 Rev = (transform.position - collision.transform.position) / 3.0f;
            m_Velocity += (Vector3)Rev;
        }
        */
    }

    protected void UpdateXDirection()
    {
        if (GetRigid() || GetIsMotion()) return;
        AdjustXDirection();
    }

    virtual protected void CalculateVelocity()
    {
        m_TargetVelocity = 0.0f;
        if (GetRigid() || GetIsMotion())
        {
            m_TargetVelocity = 0.0f;
        }
        else
        {   
            m_TargetVelocity = m_DirectionalInput.x * m_MoveSpeed;
        }

        if (m_TargetVelocity == m_Velocity.x) { m_VelocityXSmoothing = 0.0f; }

        m_Velocity.x = Mathf.SmoothDamp(m_Velocity.x, m_TargetVelocity, ref m_VelocityXSmoothing, (m_Controller.collisions.below) ? m_AccelerationTimeGrounded / m_BaseMotionSpeed : m_AccelerationTimeAirborne / m_BaseMotionSpeed);
        if (m_Gravitable) m_Velocity.y += m_Gravity * Time.deltaTime * m_BaseMotionSpeed;
    }

    protected void NormalizeModelRotation()
    {
        if (m_XDirection == 1)
        {
            // m_CharModelObj.transform.localRotation = Quaternion.identity;
        }
        else
        {
            // m_CharModelObj.transform.localRotation = Quaternion.AngleAxis(180.0f, Vector3.down) * Quaternion.identity;
        }
        
    }


}
