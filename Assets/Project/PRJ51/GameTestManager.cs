using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTestManager : MonoBehaviour
{
    [SerializeField] PlatformCamera m_Camera;

    void Start()
    {
        PlayerManager.Instance.InstantiatePlayer(FactoryManager.PLAYER.PL000, transform, ref m_Camera);
    }

    void Update()
    {
        
    }
}
