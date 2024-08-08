using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ThrowProperty
{
    public PlatformActionManager.AttackSet AttackSet;
    public Vector2 Velocity;
}

public interface ICatchable
{
    public void Catched(GameObject Parent);
    public void Throwed(ref ThrowProperty throwProperty);
    public void Carried();
    public bool IsCatchable();
}
