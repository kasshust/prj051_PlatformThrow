using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusPanel : MonoBehaviour
{
    
    [SerializeField] Text m_Money;
   
    [SerializeField] Text m_WorkEfficiency;

    [SerializeField] Slider m_HpSlider;
    [SerializeField] Text   m_Hp;

    [SerializeField] Slider m_StressSlider;
    [SerializeField] Text   m_Stress;

    [SerializeField] Slider m_MovePointSlider;
    [SerializeField] Text m_MovePoint;


    private void Start()
    {
        UpdateStatus();
    }

    public void UpdateStatus() {
        PRJ080Data.Param hp         = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_HP;
        int money                   = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_Money;
        PRJ080Data.Param stress     = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_Stress;
        PRJ080Data.Param movepoint  = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_MovePoint;
        PRJ080Data.Param efficiency = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_WorkEfficiency;

        //　お金
        m_Money.text = money.ToString() + "円";

        // 効率
        m_WorkEfficiency.text = efficiency.value.ToString() + " / " + efficiency.max_value.ToString();

        // 体力
        m_Hp.text           =       hp.value.ToString() + " / " + hp.max_value.ToString() ;
        m_HpSlider.value    =      (float)hp.value / (float)hp.max_value;

        // ストレス        
        m_Stress.text           =  stress.value.ToString() + " / " + stress.max_value.ToString();
        m_StressSlider.value    = (float)stress.value / (float)stress.max_value;

        // 行動Pt
        m_MovePointSlider.maxValue      = movepoint.max_value;
        m_MovePointSlider.value         = movepoint.value;
        m_MovePoint.text                = movepoint.value.ToString();
    }

    public void Update() {
        UpdateStatus();
    }
}
