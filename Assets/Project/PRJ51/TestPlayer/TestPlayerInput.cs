using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TestPlayer))]
public class TestPlayerInput : PlatformPlayerInput
{
    private TestPlayer m_Player;

    override protected void Awake()
    {
        base.Awake();
        m_Player = GetComponent<TestPlayer>();
    }

    override public void Control()
    {
        BaseControl(m_Player);
        
        Vector2 m = m_KeyBind.Player.Move.ReadValue<Vector2>();
        SendBaseAndEscapeMotion(m); // 基本モーション
        SendSpecialMotion(m);       // 固有モーション
    }

    private void SendBaseAndEscapeMotion(Vector2 moveValue)
    {
        m_Player.BaseMotionInput(TestPlayer.BaseMotionType.Move, moveValue);

        if (CommonInputModule.lefshPressed()) {
        }

        if (m_Player.isLanding() && m_Player.m_ControlState == PlatformCharacterBase.ControlState.RockOn && m_KeyBind.Player.Rolling.ReadValue<float>() == 1 && moveValue.magnitude > 0.01f)
        {
            m_Player.RollingInput(moveValue);
        }
        else if (CommonInputModule.APressed())
        {
            m_Player.BaseMotionInput(TestPlayer.BaseMotionType.Jump, moveValue);
        }
        if (m_KeyBind.Player.Cross.ReadValue<float>() == 0.0f)
        {
            m_Player.BaseMotionInput(TestPlayer.BaseMotionType.OnJump, moveValue);
        }
    }

    private bool isOnUp(Vector2 moveValue)
    {
        float th = 0.8f;
        if (moveValue.y > th) return true;
        else return false;
    }

    private bool isOnDown(Vector2 moveValue)
    {
        float th = -0.8f;
        if (moveValue.y < th) return true;
        else return false;
    }

    private bool isOnMissDirectH(Vector2 moveValue) {
        float th = 0.8f;
        if (Mathf.Sign(m_Player.m_XDirection) != Mathf.Sign(moveValue.x) && Mathf.Abs(moveValue.x) > th) return true;
        else return false;
    }

    private bool isOnSameDirectH(Vector2 moveValue)
    {
        float th = 0.8f;
        if (Mathf.Sign(m_Player.m_XDirection) == Mathf.Sign(moveValue.x) && Mathf.Abs(moveValue.x) > th) return true;
        else return false;
    }

    private void SendSpecialMotion(Vector2 moveValue) {

        //　△+後方 
        if (CommonInputModule.yPressed() && isOnMissDirectH(moveValue) && m_Player.m_ControlState == PlatformCharacterBase.ControlState.RockOn)
        {
        }

        //　△+前方
        if (CommonInputModule.yPressed() && isOnSameDirectH(moveValue) && m_Player.m_ControlState == PlatformCharacterBase.ControlState.RockOn) {
        }

        //　△
        if (CommonInputModule.yPressed())
        {
        }

        
        //  〇+後方
        if (CommonInputModule.BPressed() && isOnMissDirectH(moveValue))
        {
        }

        //  〇+前方
        if (CommonInputModule.BPressed() && isOnSameDirectH(moveValue))
        {
        }

        //  〇+下
        if (CommonInputModule.BPressed() && isOnDown(moveValue))
        {
        }

        //  〇+上
        if (CommonInputModule.BPressed() && isOnUp(moveValue))
        {
        }

        // 〇
        if (CommonInputModule.BPressed() )
        {
        }

        // 〇
        if (CommonInputModule.XPressed())
        {
            m_Player.HnadAction(moveValue);
        }

    }
}