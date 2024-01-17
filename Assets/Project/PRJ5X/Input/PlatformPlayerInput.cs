using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixeye.Unity;


public abstract class PlatformPlayerInput : MonoBehaviour
{
    #region input参考
    //  Y
    // X B
    //  A
    #endregion

    public enum InputState {
        None,
        Press,
        Release,
        Hold
    }

    public enum AnimInput
    {
        None,               // 0
        A,                  // 1
        B,                  // 2
        X,                  // 3
        Y,                  // 4
    }

    protected StandardKeyBind m_KeyBind;

    [SerializeField, ReadOnly, Foldout("Barrage")] protected float m_Abarrage = 0.0f;
    [SerializeField, ReadOnly, Foldout("Barrage")] protected float m_Bbarrage = 0.0f;
    [SerializeField, ReadOnly, Foldout("Barrage")] protected float m_Xbarrage = 0.0f;
    [SerializeField, ReadOnly, Foldout("Barrage")] protected float m_Ybarrage = 0.0f;

    [SerializeField, Foldout("Barrage")]           private float m_ResetInterval = 30.0f;
    [SerializeField, ReadOnly, Foldout("Barrage")] private float m_AresetTimer = 0.0f;
    [SerializeField, ReadOnly, Foldout("Barrage")] private float m_BresetTimer = 0.0f;
    [SerializeField, ReadOnly, Foldout("Barrage")] private float m_XresetTimer = 0.0f;
    [SerializeField, ReadOnly, Foldout("Barrage")] private float m_YresetTimer = 0.0f;


    protected virtual void Awake()
    {
        m_KeyBind = GameMainSystem.Instance.GetKeyBind();
    }

    protected void SendRockOnState(PlatformPlayerBase Player)
    {
        if (m_KeyBind.Player.RockOn.ReadValue<float>() != 0.0f)
        {
             Player.RockOn();
        }
        else {
            Player.RockOff();
        }
    }

    protected void SendAnimInput(PlatformPlayerBase Player)
    {
        if (Player == null) return;

        Player.InitAnimInput();

        // Stick
        Vector2 m = m_KeyBind.Player.Move.ReadValue<Vector2>();
        Player.SetAnimInputHorizontal(m[0]);
        Player.SetAnimInputVertical(m[1]);

        // A
        if (CommonInputModule.APressed()) { 
            Player.SetAnimInputPress((int)AnimInput.A); 
            m_Abarrage += 1.0f;
            m_AresetTimer = 0.0f;
        }
        else if (CommonInputModule.AReleased()) { 
            Player.SetAnimInputRelease((int)AnimInput.A); 
            m_Abarrage += 1.0f;
            m_AresetTimer = 0.0f;
        }
        else if (m_KeyBind.Player.Cross.ReadValue<float>() != 0.0f) Player.SetAnimInputHold((int)AnimInput.A);

        // B
        if (CommonInputModule.BPressed()) { 
            Player.SetAnimInputPress((int)AnimInput.B); 
            m_Bbarrage  += 1.0f;
            m_BresetTimer = 0.0f;
        }
        else if (CommonInputModule.BReleased()) {
            Player.SetAnimInputRelease((int)AnimInput.B); 
            m_Bbarrage += 1.0f;
            m_BresetTimer = 0.0f;
        }
        else if (m_KeyBind.Player.Circle.ReadValue<float>() != 0.0f) Player.SetAnimInputHold((int)AnimInput.B);

        // Y
        if (CommonInputModule.yPressed()) { 
            Player.SetAnimInputPress((int)AnimInput.Y); 
            m_Ybarrage += 1.0f;
            m_YresetTimer = 0.0f;
        }
        else if (CommonInputModule.YReleased()) { 
            Player.SetAnimInputRelease((int)AnimInput.Y); 
            m_Ybarrage += 1.0f;
            m_YresetTimer = 0.0f;
        }
        else if (m_KeyBind.Player.Triangle.ReadValue<float>() != 0.0f) Player.SetAnimInputHold((int)AnimInput.Y);

        // X
        if (CommonInputModule.XPressed()) { 
            Player.SetAnimInputPress((int)AnimInput.X); 
            m_Xbarrage += 1.0f;
            m_XresetTimer = 0.0f;
        }
        else if ((CommonInputModule.XReleased())) { 
            Player.SetAnimInputPress((int)AnimInput.X); 
            m_Xbarrage += 1.0f;
            m_XresetTimer = 0.0f;
        }
        else if (m_KeyBind.Player.Square.ReadValue<float>() != 0.0f) Player.SetAnimInputHold((int)AnimInput.X);

        //　連打
        Player.SetAnimInputBarrage(m_Abarrage,m_Bbarrage,m_Xbarrage,m_Ybarrage);
        UpdateBarrage();
    }

    private void UpdateBarrage() {
        m_Abarrage = UpdateBarrage(m_Abarrage);
        m_Bbarrage = UpdateBarrage(m_Bbarrage);
        m_Xbarrage = UpdateBarrage(m_Xbarrage);
        m_Ybarrage = UpdateBarrage(m_Ybarrage);

        UpdateResetTimerBarrage();
    }

    private void UpdateResetTimerBarrage() {
        if (m_AresetTimer >= m_ResetInterval) m_Abarrage = 0.0f;
        if (m_BresetTimer >= m_ResetInterval) m_Bbarrage = 0.0f;
        if (m_XresetTimer >= m_ResetInterval) m_Xbarrage = 0.0f;
        if (m_YresetTimer >= m_ResetInterval) m_Ybarrage = 0.0f;

        m_AresetTimer = Mathf.Clamp(m_AresetTimer + 1f, 0.0f, 60.0f);
        m_BresetTimer = Mathf.Clamp(m_BresetTimer + 1f, 0.0f, 60.0f);
        m_XresetTimer = Mathf.Clamp(m_XresetTimer + 1f, 0.0f, 60.0f);
        m_YresetTimer = Mathf.Clamp(m_YresetTimer + 1f, 0.0f, 60.0f);
    }

    private float UpdateBarrage(float barrage)
    {
        return Mathf.Clamp(barrage - 0.1f, 0 , 10.0f);
    }

    abstract public void Control();
    　
    public void BaseControl(PlatformPlayerBase Player) {
        if (Player == null) return;

        SendRockOnState(Player);　//　ロックオン操作
        SendAnimInput(Player);　　//  Animatorにコントローラー入力を伝える
    }
}
