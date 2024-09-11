using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixeye.Unity;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
public class CommonEnemyAI : EnemyAI
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

    [SerializeField, Foldout("CommonEnemyAI Param")] protected AIType m_AIType;
    EnemyBase m_Enemy;

    

    protected override void Init()
    {
        if (m_Enemy == null) m_Enemy = GetComponent<EnemyBase>();
        BuildTree();
    }

    protected override void BuildTree()
    {
        m_BehaviorTree = null;

        switch (m_AIType)
        {
            case AIType.Type00:
                ChasePlayerTree();
                break;
        }

    }

    protected void ChasePlayerTree()
    {
        m_BehaviorTree = new BehaviorTreeBuilder(gameObject)
        .RepeatForever()
            .Sequence()
                .Do("ChasePlayer", () => {
                    if (ChasePlayer()) return TaskStatus.Success;
                    else return TaskStatus.Continue;
                })
                .WaitTime(0.05f)
            .End()
        .End()
        .Build();
    }


    Vector2 m_TempVector = new Vector2();
    private bool ChasePlayer()
    {
        if (PlayerManager.Instance.m_Player != null)
        {
            PlatformPlayerBase p = PlayerManager.Instance.m_Player;
            m_TempVector = (Vector2)(p.transform.position - m_Enemy.transform.position);

            m_Enemy.m_Direction = m_TempVector.normalized;
            m_Enemy.Move(m_Enemy.m_Direction * 0.1f);

            if (m_TempVector.magnitude < 0.2f) return true;
        }
        return false;
    }


}
