using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharacterStatus {
    [System.Serializable]
    public struct FloatParam {
        public float Value;
        public float MaxValue;
    }

    public bool IsDead;
    public bool IsInvincible;
    public bool IsTempInvincible;
    public FloatParam       Hp;
    public FloatParam       FlirtEndure;
    public bool             SuperArmer;
    public FloatParam       Strengthen;
    public bool             IsGuard;
    public int              Money;

    public void CalHp(float value) {
        Hp.Value = Mathf.Clamp(Hp.Value + value, 0.0f, Hp.MaxValue);
    }

    public void CalStrengthen(float value)
    {
        Strengthen.Value = Mathf.Clamp(Strengthen.Value + value, 0.0f, Strengthen.MaxValue);
    }

    public void CalMaxHp(float value)
    {
        Hp.MaxValue = Mathf.Clamp(Hp.MaxValue + value, 0.0f, 999999.0f);
    }

    public void CalMaxStrengthen(float value)
    {
        Strengthen.MaxValue = Mathf.Clamp(Strengthen.MaxValue + value, 0.0f, 999999.0f);
    }

    public void CalFlirtEndure(float value)
    {
        FlirtEndure.Value = Mathf.Clamp(FlirtEndure.Value + value, 0.0f, FlirtEndure.MaxValue);
    }

}

[System.Serializable]
[CreateAssetMenu(fileName = "PlatformStatusObject", menuName = "ActionGameObject/Status/PlatformStatusObject")]
public class PlatformStatusObject : ScriptableObject
{
    [SerializeField]
    public CharacterStatus m_PlatformStatus;
}
