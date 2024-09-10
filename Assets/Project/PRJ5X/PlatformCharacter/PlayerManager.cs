using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pixeye.Unity;

public class PlayerManager : SingletonMonoBehaviourFast<PlayerManager>
{

    [SerializeField]
    bool m_Operational = true;

    [SerializeField]
    PlatformPlayerBase m_PlayerPrefab;

    [SerializeField, ReadOnly]
    public PlatformPlayerBase m_Player;

    [SerializeField/*, ReadOnly*/]
    PlatformPlayerInput m_Input;

    [SerializeField]
    protected PlatformStatusObject m_PlatformStatusObject;

    [SerializeField, ReadOnly]
    public CharacterStatus m_PlayerStatus;

    [SerializeField, Foldout("SE")] private FMODUnity.EventReference m_GetMoneySE;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }


    public PlatformPlayerBase InstantiatePlayer(FactoryManager.PLAYER ePlayer, Transform playerSponeTransform, ref PlatformCamera camera)
    {
        if (m_PlayerPrefab == null) {
            Debug.LogError("PlayerPrefabが設定されていません");
            return null;
        }

        if (m_Player == null)
        {
            // m_Player = FactoryManager.Instance.GetObject<PlatformPlayerBase>(ePlayer, playerSponeTransform.position, Quaternion.identity);
            // 普通に生成してしまう
            m_Player = Instantiate(m_PlayerPrefab, playerSponeTransform.position, Quaternion.identity);
            m_Input = m_Player.gameObject.GetComponent<PlatformPlayerInput>();
            camera.SetCharacterBase(m_Player);
            return m_Player;
        }
        else {
            Debug.LogError("プレイヤーが既に生成されています");
        }
        return null;
    }

    protected void Start()
    {
        InitStatus();
    }

    protected void Update()
    {
        if (m_Input != null) {
            if (m_Operational) m_Input.Control();
            else m_Player.InitInputDirection();
        }
    }

    public void InitStatus()
    {
        m_PlayerStatus = m_PlatformStatusObject.m_PlatformStatus;
        SetOperational(true);
        Debug.Log("Playerの情報を初期化しました");
    }

    public void CalHp(float value)
    {
        m_PlayerStatus.CalHp(value);
    }

    public void CalHpMax(float value)
    {
        m_PlayerStatus.CalMaxHp(value);
    }

    public void CalFlirtEndure(float value)
    {
        m_PlayerStatus.CalFlirtEndure(value);
    }

    public void SetOperational(bool b) {
        m_Operational = b;
    }

    public bool IsDead() {
        if (m_PlayerStatus.Hp.Value <= 0.0f) return true;
        else return false;
    }

    public void GetMoney(int m) {
        Debug.Log(m.ToString() + "ゲット");
        FMODUnity.RuntimeManager.PlayOneShot(m_GetMoneySE, Camera.main.transform.position);
    }

}
