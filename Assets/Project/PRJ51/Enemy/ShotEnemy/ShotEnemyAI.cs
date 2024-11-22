using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixeye.Unity;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
public class ShotEnemyAI : EnemyAI
{

    public enum AIType
    {
        Type00,
        Type01,
        Type02,
        Type03,
        Type04,
        Type05,
        Type06,
        Type07,
        Type08,
        Type09,
    }

    [SerializeField, Foldout("ShotEnemyAI Param")] protected AIType m_AIType;
    ShotEnemy m_Enemy;
    


    protected override void Init()
    {
        if (m_Enemy == null) m_Enemy = GetComponent<ShotEnemy>();
        BuildTree();
    }

    protected override void BuildTree()
    {
        m_BehaviorTree = null;

        switch (m_AIType)
        {
            case AIType.Type00:
                ShotCycleTree();
                break;
        }

    }

    protected void ShotCycleTree()
    {
        m_BehaviorTree = new BehaviorTreeBuilder(gameObject)
        .RepeatForever()
            .Sequence()
                .Do("ShotCycle", () => {
                    if (Shot()) return TaskStatus.Success;
                    else return TaskStatus.Continue;
                })
                .WaitTime(1.0f)
            .End()
        .End()
        .Build();
    }

    Vector2 m_TempVector = new Vector2();
    protected bool Shot() {
        if (PlayerManager.Instance.m_Player != null)
        {
            PlatformPlayerBase p = PlayerManager.Instance.m_Player;
            m_TempVector = (Vector2)(p.transform.position - m_Enemy.transform.position);

            m_Enemy.m_Direction = m_TempVector.normalized;
            m_Enemy.Shot(m_TempVector.normalized);

            return true;
        }
        return false;
    }


}