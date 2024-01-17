using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ThrowProperty
{
    public Vector2 Velocity;
}

interface ICatchable
{
    public void Catched(GameObject Parent);
    public void Throwed(ref ThrowProperty throwProperty);
    public void Carried();
}
