using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorImpactSender : ActionGameUtility
{
    [SerializeField, ReadOnly]
    float m_ExistTime;

    public LayerMask                        m_CollisionMask;

    [ReadOnly]
    public PlatformActionManager.BaseSenderInfo m_BaseInfo;
    
    [ReadOnly]
    public PlatformActionManager.AttackInfo m_AttackInfo;

    [ReadOnly]
    public PlatformActionManager.ReplyInfo  m_ReplyInfo;

    [ReadOnly]
    public Animator m_Animator;

    [ReadOnly]
    public ActionGameCharacterBase m_CharacterBase;

    private AnimatorStateInfo m_AnimatorStateInfo;
    private AnimatorStateInfo m_CurrentAnimatorStateInfo;

    BehaviorImpactReceiver TempReceiver;

    // 同時にヒット可能な数
    private RaycastHit2D[] hits = new RaycastHit2D[5];

    protected override void Wake()
    {
    }

    override public ActionGameUtility CreateInit()
    {
        if (m_Animator != null) m_AnimatorStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        return this;
    }

    public void InitAllParam(PlatformActionManager.AttackInfo attackinfo, PlatformActionManager.BaseSenderInfo baseInfo, ActionGameCharacterBase characterBase, Animator animator = null) {
        m_BaseInfo      = baseInfo;
        m_AttackInfo    = attackinfo;
        m_Animator      = animator;
        m_CharacterBase = characterBase;
        m_ExistTime     = m_BaseInfo.Time;
    }

    public void SendImpact(ref PlatformActionManager.AttackInfo attackInfo ,ref PlatformActionManager.BaseSenderInfo baseInfo,  RaycastHit2D hit) {
        TempReceiver = hit.collider.gameObject.GetComponent<BehaviorImpactReceiver>();
        if (TempReceiver != null)
        {
            bool bhit = TempReceiver.ReceiveImpactGetReply(ref attackInfo, hit, this, ref m_ReplyInfo, hit.collider.gameObject);
            if (bhit)
            {
                m_CharacterBase.ChatchImpactReply(ref m_ReplyInfo);
                FireHitEffects(ref attackInfo, ref baseInfo, hit);
            }
        }
    }

    private void FireHitEffects(ref PlatformActionManager.AttackInfo attackInfo, ref PlatformActionManager.BaseSenderInfo baseInfo, RaycastHit2D hit) {

        FireHitSound(ref attackInfo, ref baseInfo, hit);
        FireHitVFX(ref attackInfo, ref baseInfo, hit);
        FireHitStop(ref attackInfo, ref baseInfo, hit);
        FireHitPstVFX(ref attackInfo);
    }

    private void FireHitPstVFX(ref PlatformActionManager.AttackInfo attackInfo)
    {
        // if (attackInfo.RunHitPostEffect) VFXController.Instance.FirePostVFX(attackInfo.PostVFXType, attackInfo.PostDuration, attackInfo.PostMagnitude);
    }

    private void FireHitSound(ref PlatformActionManager.AttackInfo attackInfo, ref PlatformActionManager.BaseSenderInfo baseInfo, RaycastHit2D hit)
    {
        if (!attackInfo.HitSound.IsNull) FMODUnity.RuntimeManager.PlayOneShot(attackInfo.HitSound, transform.position);
    }

    private void FireHitVFX(ref PlatformActionManager.AttackInfo attackInfo, ref PlatformActionManager.BaseSenderInfo baseInfo, RaycastHit2D hit)
    {
        if (attackInfo.CreateHitEffect)
        {
            // ActionGameEffect e = m_FactoryManager.GetObject<ActionGameEffect>(attackInfo.HitEffect, hit.point, Quaternion.identity);
            // e.SetEffectInfo(attackInfo.Direction, hit.point);
        }
    }

    private void FireHitStop(ref PlatformActionManager.AttackInfo attackInfo, ref PlatformActionManager.BaseSenderInfo baseInfo, RaycastHit2D hit) {
        if (attackInfo.HitStop)
        {
            if(m_CharacterBase != null) m_CharacterBase.StartHitStop(attackInfo.HitStopTime);
        }
    }

    public void Update()
    {
        switch (m_BaseInfo.Form)
        {
            case PlatformActionManager.SenderForm.Circle:
                UpdateCircleRayCastNonAlloc();
                break;
            case PlatformActionManager.SenderForm.Rect:
                UpdateRectRayCastNonAlloc();
                break;
            default:
                break;
        }

        if (!m_BaseInfo.UntilMotionEnd) UpdateExistTime();
        else CheckMotionEnd();

    }

    private void CheckMotionEnd() {
        if (m_Animator != null) {
            m_CurrentAnimatorStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
            if (m_AnimatorStateInfo.fullPathHash != m_CurrentAnimatorStateInfo.fullPathHash) ReleaseObject();
        } else ReleaseObject();
    }

    public void UpdateExistTime() {

        if (m_CharacterBase == null)
        {
            m_ExistTime -= Time.deltaTime;
        }
        else if (!m_CharacterBase.IsSlowMotion()) {
            m_ExistTime -= Time.deltaTime;
        }

        if (m_ExistTime <= 0.0f) {
            ReleaseObject();
        }
    }



    private void UpdateRayCast(RaycastHit2D[] hits) {
        foreach (RaycastHit2D hit in hits)
        {
            if (hit) SendImpact(ref m_AttackInfo, ref m_BaseInfo, hit);
        }
    }

    private void UpdateCircleRayCast() {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, m_BaseInfo.CircleSenderInfo.Radius, Vector2.zero, Mathf.Infinity, m_CollisionMask);
        UpdateRayCast(hits);
    }

    private void UpdateRectRayCast()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(m_BaseInfo.RectSenderInfo.Width, m_BaseInfo.RectSenderInfo.Height), m_BaseInfo.RectSenderInfo.Angle, Vector2.zero, Mathf.Infinity, m_CollisionMask);
        UpdateRayCast(hits);
    }

    private void UpdateRayCastNonAlloc(int results, RaycastHit2D[] hits)
    {
        for (int i = 0; i < results; i++)
        {
            SendImpact(ref m_AttackInfo, ref m_BaseInfo, hits[i]);
        }
    }

    private void UpdateCircleRayCastNonAlloc()
    {
        int results = Physics2D.CircleCastNonAlloc(transform.position, m_BaseInfo.CircleSenderInfo.Radius, Vector2.zero, hits, Mathf.Infinity, m_CollisionMask);
        UpdateRayCastNonAlloc(results, hits);
    }

    private void UpdateRectRayCastNonAlloc()
    {
        int results = Physics2D.BoxCastNonAlloc(transform.position, new Vector2(m_BaseInfo.RectSenderInfo.Width, m_BaseInfo.RectSenderInfo.Height), m_BaseInfo.RectSenderInfo.Angle, Vector2.zero, hits, Mathf.Infinity, m_CollisionMask);
        UpdateRayCastNonAlloc(results, hits);
    }


    void OnDrawGizmos()
    {
        switch (m_BaseInfo.Form)
        {
            case PlatformActionManager.SenderForm.Circle:
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, m_BaseInfo.CircleSenderInfo.Radius);
                
                break;
            case PlatformActionManager.SenderForm.Rect:
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(transform.position, new Vector3(m_BaseInfo.RectSenderInfo.Width, m_BaseInfo.RectSenderInfo.Height, 1.0f));
                break;
            default:
                break;
        }

        Gizmos.color = Color.blue;
        DrawRay(transform.position, m_AttackInfo.Direction.normalized * m_AttackInfo.ImpactValue);
        
    }

    private void DrawRay(Vector3 pos, Vector3 dir)
    {
        Ray ray = new Ray();
        ray.origin = pos;
        ray.direction = dir;
        Gizmos.DrawRay(ray);
    }


}
