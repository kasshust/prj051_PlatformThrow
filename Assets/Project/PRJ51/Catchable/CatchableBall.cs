using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BallState
{
    Default,
    Carried,
    Throwed,
    Bound
}

public abstract class CatchableBall : MonoBehaviour, ICatchable
{
    protected GameObject m_Parent;

    [SerializeField] public BallState m_State = BallState.Default;
    [SerializeField] protected int m_Level = 0;

    [SerializeField, ReadOnly] protected PlatformActionManager.AttackInfo m_AttackInfo;
    protected PlatformActionManager.ReplyInfo  m_ReplyInfo;

    public abstract void Carried();
    public abstract void Catched(GameObject Parent);
    public abstract bool IsCatchable();
    public abstract void Throwed(ref ThrowProperty throwProperty);
}
