using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixeye.Unity;
using CleverCrow.Fluid.BTs.Trees;

abstract public class ActionGameCharacterAI : MonoBehaviour
{
    [SerializeField, Foldout("ActionGameCharacterAI Param")] 
    protected BehaviorTree  m_BehaviorTree;
    private bool m_IsUpdateAI = true;

    [SerializeField]
    private bool m_ForceStopAI = false;

    [SerializeField, Range(1, 20), Foldout("ActionGameCharacterAI Param")]
    private int m_TreeUpdateInterval = 3;
    private int m_UpdateCount = 0;


    protected void Awake() {
        BuildTree();
    }

    protected abstract void BuildTree();

    private void Start()
    {
        Init();
    }

    public void UpdateAI() {

        if (m_ForceStopAI) return;

        if (m_BehaviorTree != null)
        {
            if (m_IsUpdateAI) { 
                if (m_UpdateCount % m_TreeUpdateInterval == 0)
                {
                    PreStep();
                    m_BehaviorTree.Tick();
                    Step();
                    m_UpdateCount = 0;
                }
                m_UpdateCount++;
            }
        }
    }

    public void Stop()
    {
        m_IsUpdateAI = false;
    }

    public void Resume()
    {
        m_IsUpdateAI = true;
    }

    public void ResetAI() {
        if (m_BehaviorTree != null) m_BehaviorTree.Reset();
    }


    virtual protected void PreStep()
    {

    }

    virtual protected void Step() {
    
    } 


    abstract protected void Init();

}
