using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBall : MonoBehaviour, ICatchable
{
    GameObject m_Parent;
    [SerializeField] Rigidbody2D m_Rigidbody2D;

    public void Carried()
    {
        transform.position = m_Parent.transform.position;
        m_Rigidbody2D.velocity = Vector2.zero;
        m_Rigidbody2D.angularVelocity = 0.0f;
    }

    public void Catched(GameObject Parent)
    {
        m_Parent = Parent;
    }


    public void Throwed(ref ThrowProperty throwProperty)
    {
        GetComponent<Rigidbody2D>().velocity = throwProperty.Velocity;
        m_Parent = null;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
