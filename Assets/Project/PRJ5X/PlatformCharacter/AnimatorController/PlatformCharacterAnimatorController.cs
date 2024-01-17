using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlatformCharacterAnimatorController<T> : MonoBehaviour where T : PlatformCharacterBase
{
    [SerializeField, ReadOnly] protected Animator   m_Animator;
    [SerializeField] protected T                    m_PlatformCharacter;
    [SerializeField] protected Transform[]          m_ParentTransforms;

    public T GetPlatformCharacter() {
        return m_PlatformCharacter;
    }

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        if (m_Animator == null) Debug.LogWarning("AnimatorControllerにアニメーターが設定されていません");
    }

    public void OffRigid()
    {
        m_Animator.SetBool("Rigid", false);
    }

    public void OnRigid()
    {
        m_Animator.SetBool("Rigid", true);
    }

    public void SetChainMotion(AnimationEvent e)
    {
        m_Animator.SetInteger("ChainMotion", e.intParameter);
    }

    public void SetVelocityX(AnimationEvent e)
    {
        float dir = (float)m_PlatformCharacter.m_XDirection;
        m_PlatformCharacter.ForceSetVelocityX(dir * e.floatParameter);
    }

    public void SetVelocityY(AnimationEvent e)
    {
        m_PlatformCharacter.ForceSetVelocityY(e.floatParameter);
    }

    public void SetVelocityDirection(AnimationEvent e)
    {
        m_PlatformCharacter.ForceSetVelocity(e.floatParameter * m_PlatformCharacter.m_Direction);
    }

    public void SetGravitable(AnimationEvent e)
    {
        if (e.intParameter == 0) m_PlatformCharacter.ForceSetGravitable(false);
        else m_PlatformCharacter.ForceSetGravitable(true);
    }

    public void SetGravitable(bool b)
    {
        m_PlatformCharacter.ForceSetGravitable(b);
    }



    public void SetMotorSpeedTime(AnimationEvent e)
    {
        CommonInputModule.Instance.SetMotorSpeedTime(e.floatParameter, 0.6f, 0.6f);
    }

    public void AdjustXDirection()
    {
        m_PlatformCharacter.AdjustXDirection();
    }

    public void SetHitStop() {
        
    }



    private void AdjustInfo(ImpactSenderDataSet data, ref PlatformActionManager.AttackInfo attackInfo, ref PlatformActionManager.BaseSenderInfo baseInfo) {
        if (data.AdjustDirXDirection)
        {
            attackInfo.Direction.x *= m_PlatformCharacter.m_XDirection;
        }

        if (!data.IsChild)
        {
            baseInfo.LoacalPosition = m_PlatformCharacter.transform.position + baseInfo.LoacalPosition;
            
            if (data.AdjustPosXDirection)
            {
                baseInfo.LoacalPosition.x *= m_PlatformCharacter.m_XDirection;
            }
        }
        else
        {
            int index = data.ParentNum;
            if (index < 0 || index > m_ParentTransforms.Length)
            {
                Debug.LogWarning("AnimatorControllerのParentNumの値が不正です");
                return;
            }

            Transform t = m_ParentTransforms[index];
            if (t == null)
            {
                Debug.LogWarning("AnimatorControllerのTransformがNULLです");
                return;
            }
            baseInfo.ParentTransform = t;
        }
    }

    public void CreateSenderImpact(ImpactSenderDataSet data)
    {
        PlatformActionManager.AttackInfo        attackInfo          = data.AttackInfo;
        PlatformActionManager.BaseSenderInfo    baseInfo            = data.BaseSenderInfo;
        AdjustInfo(data, ref attackInfo, ref baseInfo);
        PlatformActionManager.Instance.CreateImpactSender(ref attackInfo, ref baseInfo, m_PlatformCharacter, m_Animator);
    }


}
