using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CommonInputModule : SingletonMonoBehaviourFast<CommonInputModule>
{

    public bool m_Shakable = true;
    public Gamepad m_GamePadCurrent;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    // Pressed
    public static bool APressed()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.aButton.wasPressedThisFrame;
    }
    public static bool BPressed()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.bButton.wasPressedThisFrame;
    }
    public static bool xPressed()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.xButton.wasPressedThisFrame;
    }
    public static bool yPressed()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.yButton.wasPressedThisFrame;
    }

    public static bool leftrPressed()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.leftTrigger.wasPressedThisFrame;
    }

    public static bool lefshPressed()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.leftShoulder.wasPressedThisFrame;
    }

    public static bool ritrPressed()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.rightTrigger.wasPressedThisFrame;
    }

    public static bool rishPressed()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.rightShoulder.wasPressedThisFrame;
    }

    public static bool SelectPressed()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.selectButton.wasPressedThisFrame;
    }

    public static bool StartPressed()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.startButton.wasPressedThisFrame;
    }

    public static bool RightPressed()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.dpad.right.wasPressedThisFrame;
    }
    public static bool DownPressed()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.dpad.down.wasPressedThisFrame;
    }
    public static bool LeftPressed()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.dpad.left.wasPressedThisFrame;
    }
    public static bool UpPressed()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.dpad.up.wasPressedThisFrame;
    }

    // Released
    public static bool AReleased()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.aButton.wasReleasedThisFrame;
    }
    public static bool BReleased()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.bButton.wasReleasedThisFrame;
    }
    public static bool XReleased()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.xButton.wasReleasedThisFrame;
    }
    public static bool YReleased()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.yButton.wasReleasedThisFrame;
    }

    public static bool LeftrReleased()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.leftTrigger.wasReleasedThisFrame;
    }

    public static bool LefshReleased()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.leftShoulder.wasReleasedThisFrame;
    }

    public static bool RitrReleased()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.rightTrigger.wasReleasedThisFrame;
    }

    public static bool RishReleased()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.rightShoulder.wasReleasedThisFrame;
    }

    public static bool RightReleasedd()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.dpad.right.wasReleasedThisFrame;
    }
    public static bool DownReleased()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.dpad.down.wasReleasedThisFrame;
    }
    public static bool LeftReleased()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.dpad.left.wasReleasedThisFrame;
    }
    public static bool UpReleased()
    {
        if (Gamepad.current == null) return false;
        return Gamepad.current.dpad.up.wasReleasedThisFrame;
    }

    // Motor
    public  void SetMotorSpeed(float LeftMotor, float RightMotor) {
        if (m_Shakable) {
            if (Gamepad.current == null) return;
            Gamepad.current.SetMotorSpeeds(LeftMotor, RightMotor);
        }
    }

    public void SetMotorSpeedTime(float time, float LeftMotor, float RightMotor)
    {
        StartCoroutine(ActivateMotor(time, LeftMotor, RightMotor));
    }

    private IEnumerator ActivateMotor(float time, float LeftMotor, float RightMotor)
    {
        Gamepad.current.SetMotorSpeeds(LeftMotor, RightMotor);
        yield return new WaitForSeconds(time);
        Gamepad.current.SetMotorSpeeds(0.0f, 0.0f);
    }
}
