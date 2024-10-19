using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixeye.Unity;

public class GameTestManager : MonoBehaviour
{
    [SerializeField] PlatformCamera m_Camera;

    [SerializeField, Foldout("BaseInfo")]
    public PlatformCamera.CameraAreaInfo m_CameraAreaInfo;

    [SerializeField, Foldout("Debug View")]
    bool isDrawDebug = false;

    private void Awake()
    {
        m_CameraAreaInfo.m_AreaCenter = transform.position;
        SendCameraInfo();
    }

    void Start()
    {
        PlayerManager.Instance.InstantiatePlayer(FactoryManager.PLAYER.PL000, transform, ref m_Camera);
    }

    void Update()
    {
        
    }

    public void SendCameraInfo()
    {
        if (m_Camera != null) m_Camera.SetCameraArea(m_CameraAreaInfo);
    }

    public void ResetCameraInfo()
    {
        if (m_Camera != null) m_Camera.ResetCameraArea();
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (isDrawDebug) Gizmos.DrawCube((Vector3)m_CameraAreaInfo.m_AreaOffset + transform.position, (Vector3)m_CameraAreaInfo.m_CameraMoveAreaSize + Vector3.forward);
    }
#endif
}
