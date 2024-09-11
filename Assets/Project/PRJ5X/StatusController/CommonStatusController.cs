using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonStatusController : StatusController
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void ReceiveImpact(ref PlatformActionManager.AttackInfo attackInfo, RaycastHit2D hit, ref PlatformActionManager.ReplyInfo replyInfo, BehaviorImpactSender sender, GameObject g = null)
    {
        ReceiveDamage(ref attackInfo, hit, ref replyInfo, sender, g);
    }

    public override void ReceiveDamage(ref PlatformActionManager.AttackInfo attackInfo, RaycastHit2D hit, ref PlatformActionManager.ReplyInfo replyInfo, BehaviorImpactSender sender, GameObject g = null)
    {
        m_GameCharacter.SetTempInvincible(true);
        m_GameCharacter.CalHp(-attackInfo.DamageValue);
        m_GameCharacter.CalFlirtEndure(-attackInfo.FlirtEndure);
        m_GameCharacter.Damage(ref attackInfo, hit);
    }
}
