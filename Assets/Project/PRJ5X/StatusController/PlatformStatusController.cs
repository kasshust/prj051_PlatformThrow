using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixeye.Unity;

public abstract class PlatformStatusController : MonoBehaviour
{
    [SerializeField]
    public PlatformCharacterBase m_CharacterBase;

    protected virtual void Awake()
    {
        // if (m_CharacterBase == null) Debug.LogError(this.gameObject.name + ": PlatformCharacterBase is Null");
    }

    public virtual bool IsInvincible() {
        if (m_CharacterBase == null) return false;
        return m_CharacterBase.m_CharacterStatus.IsInvincible;
    }

    public virtual bool IsTempInvincible()
    {
        if (m_CharacterBase == null) return false;
        return m_CharacterBase.m_CharacterStatus.IsTempInvincible;
    }

    public virtual bool IsDead()
    {
        if (m_CharacterBase == null) return false;
        return m_CharacterBase.m_CharacterStatus.IsDead;
    }

    public abstract void ReceiveImpact(ref PlatformActionManager.AttackInfo attackInfo, RaycastHit2D hit, ref PlatformActionManager.ReplyInfo replyInfo, BehaviorImpactSender sender, GameObject g = null);
    public abstract void ReceiveDamage(ref PlatformActionManager.AttackInfo attackInfo, RaycastHit2D hit, ref PlatformActionManager.ReplyInfo replyInfo, BehaviorImpactSender sender, GameObject g = null);

    protected Vector2 CheckDamageDirection(GameObject g)
    {
        return (Vector2)transform.position - (Vector2)g.transform.position;
    }
}
