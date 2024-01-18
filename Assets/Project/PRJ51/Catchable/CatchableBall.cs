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

    [SerializeField] protected BallState m_State = BallState.Default;
    [SerializeField] protected int m_Level = 0;

    public abstract void Carried();
    public abstract void Catched(GameObject Parent);
    public abstract bool IsCatchable();
    public abstract void Throwed(ref ThrowProperty throwProperty);
}
