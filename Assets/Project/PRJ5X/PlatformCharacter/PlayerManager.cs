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
            
            // プールを使わないで生成
            m_Player = Instantiate(m_PlayerPrefab, playerSponeTransform.position, Quaternion.identity);
            m_Input = m_Player.gameObject.GetComponent<PlatformPlayerInput>();
            camera.SetCharacterBase(m_Player);

            // ステータス上書き
            m_Player.m_CharacterStatus = m_PlatformStatusObject.m_PlatformStatus;

            return m_Player;
        }
        else {
            Debug.LogError("プレイヤーが既に生成されています");
        }
        return null;
    }

    protected void Start()
    {
    }

    protected void Update()
    {
        if (m_Input != null) {
            if (m_Operational) m_Input.Control();
            else m_Player.InitInputDirection();
        }

        if (m_Player != null) {
            if (IsDead())
            {
                Debug.Log("プレイヤーが死んでいます");
                SetOperational(false);
            }
        }
    }

    public void InitStatus()
    {
        SetOperational(true);
        Debug.Log("Playerの情報を初期化しました");
    }

    public void SetOperational(bool b) {
        m_Operational = b;
    }

    public bool IsDead() {
        if (m_Player.m_CharacterStatus.Hp.Value <= 0.0f) return true;
        else return false;
    }

    public void GetMoney(int m) {
        Debug.Log(m.ToString() + "ゲット");
        FMODUnity.RuntimeManager.PlayOneShot(m_GetMoneySE, Camera.main.transform.position);
    }

}
