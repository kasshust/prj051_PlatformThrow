using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonStatusController : PlatformStatusController
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void ReceiveImpact(ref PlatformActionManager.AttackInfo attackInfo, RaycastHit2D hit, ref PlatformActionManager.ReplyInfo replyInfo, BehaviorImpactSender sender, GameObject g = null)
    {
        ReceiveDamage(ref attackInfo, hit, ref replyInfo, sender, g);
        m_CharacterBase.ForceSetVelocity(attackInfo.Direction * attackInfo.ImpactValue);
    }

    public override void ReceiveDamage(ref PlatformActionManager.AttackInfo attackInfo, RaycastHit2D hit, ref PlatformActionManager.ReplyInfo replyInfo, BehaviorImpactSender sender, GameObject g = null)
    {
        m_CharacterBase.SetTempInvincible(true);
        m_CharacterBase.CalHp(-attackInfo.DamageValue);
        m_CharacterBase.CalFlirtEndure(-attackInfo.FlirtEndure);
        m_CharacterBase.Damage(ref attackInfo, hit);
    }
}
