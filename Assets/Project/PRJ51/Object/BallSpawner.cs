using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] NormalBall m_SpawnBall;
    [SerializeField] Transform  m_Hand;
    NormalBall m_HasBall;


    public void SpawnBall() {
        m_HasBall = Instantiate(m_SpawnBall, transform.position, Quaternion.identity);
        m_HasBall.CatchedAction += BallCatched;
    }

    private void BallCatched() {
        m_HasBall = null;
        SpawnBall();
    }

    void Start()
    {
        SpawnBall();
    }

    void Update()
    {
        if (m_HasBall)
        {
            m_HasBall.transform.position = m_Hand.transform.position;
        }
        else {
            SpawnBall();
        }

        
    }
}
