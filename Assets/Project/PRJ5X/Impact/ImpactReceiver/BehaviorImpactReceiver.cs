using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BehaviorImpactReceiver : MonoBehaviour
{
    [SerializeField]
    private PlatformStatusController m_Status;

    [SerializeField]
    PlatformActionManager.AttackSet m_AttackSet;

    [SerializeField]
    Queue<Guid> m_ImpactID;


    public void Awake()
    {
        if(m_Status == null)Debug.LogWarning("Status is Null");
        m_ImpactID = new Queue<Guid>();
    }

    private bool HaveID(ref PlatformActionManager.AttackInfo attackInfo, BehaviorImpactSender sender) {

        if (sender == null) return false;

        if (!attackInfo.HitContinue) {

            if (m_ImpactID.Contains(sender.m_ID)) return true;

            m_ImpactID.Enqueue(sender.m_ID);
            if (m_ImpactID.Count > 6) m_ImpactID.Dequeue();
        }
        return false;
    }


    /*
    private bool Impact(ref PlatformActionManager.AttackInfo attackInfo, RaycastHit2D hit, GameObject g = null)
    {
        if (m_Status.IsInvincible()) return false;
        if (m_Status.IsDead())       return false;

        if (attackInfo.Attacker == PlatformActionManager.Attacker.All)
        {

        }
        else if (attackInfo.Attacker == m_Attacker) return false;


        if (m_Status != null) m_Status.ReceiveImpact(ref attackInfo, hit, ref null, null, g = null);
        return true;
    }

    public bool ReceiveImpact(ref PlatformActionManager.AttackInfo attackInfo, RaycastHit2D hit, GameObject g = null)
    {
        return Impact(ref attackInfo, hit, g);
    }

    public bool ReceiveImpact(ref PlatformActionManager.AttackInfo attackInfo, RaycastHit2D hit, BehaviorImpactSender sender)
    {
        if (HaveID(ref attackInfo, sender)) return false;
        return Impact(ref attackInfo, hit);
    }
    */

    private bool ImpactGetReply(ref PlatformActionManager.AttackInfo attackInfo, RaycastHit2D hit, ref PlatformActionManager.ReplyInfo replyInfo, BehaviorImpactSender sender, GameObject g = null)
    {
        if (m_Status == null) return false;

        if (m_Status.IsTempInvincible())return false;
        if (m_Status.IsInvincible())    return false;
        if (m_Status.IsDead())          return false;

        if (attackInfo.AttackSet == PlatformActionManager.AttackSet.All)
        {

        }
        else if (attackInfo.AttackSet == m_AttackSet) return false;


        if (m_Status != null) m_Status.ReceiveImpact(ref attackInfo, hit, ref replyInfo, sender, g);
        return true;
    }

    public bool ReceiveImpactGetReply(ref PlatformActionManager.AttackInfo attackInfo, RaycastHit2D hit, BehaviorImpactSender sender, ref PlatformActionManager.ReplyInfo replyInfo, GameObject g = null)
    {
        if (HaveID(ref attackInfo, sender)) return false;
        return ImpactGetReply(ref attackInfo, hit, ref replyInfo, sender, g);
    }

    /*
    public bool ReceiveImpact(ref PlatformActionManager.AttackInfo attackInfo, RaycastHit2D hit, BehaviorImpactSender sender, ref PlatformActionManager.ReplyInfo replyInfo, GameObject g = null)
    {
        if (HaveID(ref attackInfo, sender)) return false;
        return ImpactGetReply(ref attackInfo, hit, ref replyInfo, sender, g);
    }
    */

    public bool ReceiveImpactGetReply(ref PlatformActionManager.AttackInfo attackInfo, RaycastHit2D hit, ref PlatformActionManager.ReplyInfo replyInfo, GameObject g = null)
    {
        return ImpactGetReply(ref attackInfo, hit, ref replyInfo, null, g);
    }
}
