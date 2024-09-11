using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixeye.Unity;

public abstract class StatusController : MonoBehaviour
{
    [SerializeField]
    public ActionGameCharacterBase m_GameCharacter;

    protected virtual void Awake()
    {
        if (m_GameCharacter == null) m_GameCharacter = GetComponent<ActionGameCharacterBase>();
    }

    public virtual bool IsInvincible() {
        if (m_GameCharacter == null) return false;
        return m_GameCharacter.m_CharacterStatus.IsInvincible;
    }

    public virtual bool IsTempInvincible()
    {
        if (m_GameCharacter == null) return false;
        return m_GameCharacter.m_CharacterStatus.IsTempInvincible;
    }

    public virtual bool IsDead()
    {
        if (m_GameCharacter == null) return false;
        return m_GameCharacter.m_CharacterStatus.IsDead;
    }

    public abstract void ReceiveImpact(ref PlatformActionManager.AttackInfo attackInfo, RaycastHit2D hit, ref PlatformActionManager.ReplyInfo replyInfo, BehaviorImpactSender sender, GameObject g = null);
    public abstract void ReceiveDamage(ref PlatformActionManager.AttackInfo attackInfo, RaycastHit2D hit, ref PlatformActionManager.ReplyInfo replyInfo, BehaviorImpactSender sender, GameObject g = null);

    protected Vector2 CheckDamageDirection(GameObject g)
    {
        return (Vector2)transform.position - (Vector2)g.transform.position;
    }
}
