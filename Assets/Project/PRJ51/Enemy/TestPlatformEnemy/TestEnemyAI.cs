﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixeye.Unity;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;

public class TestEnemyAI : PlatformEnemyAI
{
    [SerializeField, Foldout("TestEnemyAI Param"), ReadOnly]
    TestEnemy m_Enemy;
    private float m_EscapeDist = 1.0f;

    protected override void BuildTree()
    {
        m_BehaviorTree = null;

        switch (m_AIType)
        {
            case AIType.Type00:
                ChaseBallTree();
                break;
            case AIType.Type01:
                DefaultTree();
                break;
            case AIType.Type02:
                ActiveTree();
                break;
            default:
                DefaultTree();
                break;
        }
    }

    protected void DefaultTree()
    {

        //　うろうろ
        var NoneC = new BehaviorTreeBuilder(gameObject)
        .Sequence()
        /*
            .Do("EscapeFromTarget", () => {
                if (EscapeFromTarget(m_EscapeDist)) return TaskStatus.Success;
                else return TaskStatus.Continue;
            })
        */
            .Do("ChaseTarget", () => {
                if (ChasePlayer()) return TaskStatus.Success;
                else return TaskStatus.Continue;
            })
        .End();

        // 攻撃
        var ActionA = new BehaviorTreeBuilder(gameObject)
        .Sequence()
                 .Do("Alert", () => {
                    Alert();
                    return TaskStatus.Success;
                 })
                .WaitTime(0.2f)
                .Do("Wait End Motion", () => {
                    if (IsRigid()) return TaskStatus.Continue;
                    else return TaskStatus.Success;
                })
                .Do("ChaseTarget", () => {
                    if (ChasePlayer()) return TaskStatus.Success;
                    else return TaskStatus.Continue;
                })
                .Do("BlowAttack", () => {
                    BlowAttack();
                    return TaskStatus.Success;
                })
                .WaitTime(0.2f)
                .Do("Wait End Motion", () => {
                    if (IsRigid()) return TaskStatus.Continue;
                    else return TaskStatus.Success;
                })
                .Do("EscapeFromTarget", () => {
                    if (EscapeFromTarget(m_EscapeDist)) return TaskStatus.Success;
                    else return TaskStatus.Continue;
                })
        .End();

        m_BehaviorTree = new BehaviorTreeBuilder(gameObject)
        .RepeatForever()
            .Sequence()

                .Do("SelectActionType", () => {

                    // 発火確率
                    if (Random.Range(0.0f, 1.0f) > 0.5f)
                        m_ActionType = ActionType.Action;
                    else m_ActionType = ActionType.None;

                    // m_ActionType = EnumUtility.GetRandom<ActionType>();

                    return TaskStatus.Success;
                })

                .Selector()
                    .Sequence()
                        .Do("None", () => {
                            if (m_ActionType == ActionType.None) return TaskStatus.Success;
                            else return TaskStatus.Failure;
                        })
                        .SelectorRandom()
                            .Splice(NoneC.Build())
                        .End()

                    .End()
                    .Sequence()
                        .Do("Action", () => {
                            if (m_ActionType == ActionType.Action) return TaskStatus.Success;
                            else return TaskStatus.Failure;
                        })
                        .SelectorRandom()
                            .Splice(ActionA.Build())
                        .End()

                    .End()
                .End()

            .End()

        .End()
        .Build();
    }

    protected void ActiveTree()
    {
        //　待機
        var NoneB = new BehaviorTreeBuilder(gameObject)
        .Sequence()
            .WaitTime(0.5f)
        .End();

        // 攻撃
        var ActionA = new BehaviorTreeBuilder(gameObject)
        .Sequence()
                 .Do("Alert", () => {
                     Alert();
                     return TaskStatus.Success;
                 })
                .WaitTime(0.2f)
                .Do("Wait End Motion", () => {
                    if (IsRigid()) return TaskStatus.Continue;
                    else return TaskStatus.Success;
                })
                .Do("ChaseTarget", () => {
                    if (ChasePlayer()) return TaskStatus.Success;
                    else return TaskStatus.Continue;
                })
                .Do("BlowAttack", () => {
                    BlowAttack();
                    return TaskStatus.Success;
                })
                .WaitTime(0.2f)
                .Do("Wait End Motion", () => {
                    if (IsRigid()) return TaskStatus.Continue;
                    else return TaskStatus.Success;
                })
                .Do("EscapeFromTarget", () => {
                    if (EscapeFromTarget(m_EscapeDist)) return TaskStatus.Success;
                    else return TaskStatus.Continue;
                })
        .End();

        m_BehaviorTree = new BehaviorTreeBuilder(gameObject)
        .RepeatForever()
            .Sequence()

                .Do("SelectActionType", () => {

                    // 発火確率
                    if (Random.Range(0.0f, 1.0f) > 0.1f)
                        m_ActionType = ActionType.Action;
                    else m_ActionType = ActionType.None;

                    // m_ActionType = EnumUtility.GetRandom<ActionType>();

                    return TaskStatus.Success;
                })

                .Selector()
                    .Sequence()
                        .Do("None", () => {
                            if (m_ActionType == ActionType.None) return TaskStatus.Success;
                            else return TaskStatus.Failure;
                        })
                        .SelectorRandom()
                            .Splice(NoneB.Build())
                        .End()

                    .End()
                    .Sequence()
                        .Do("Action", () => {
                            if (m_ActionType == ActionType.Action) return TaskStatus.Success;
                            else return TaskStatus.Failure;
                        })
                        .SelectorRandom()
                            .Splice(ActionA.Build())
                        .End()

                    .End()
                .End()

            .End()

        .End()
        .Build();
    }

    protected void ChaseBallTree()
    {
        m_BehaviorTree = new BehaviorTreeBuilder(gameObject)
        .RepeatForever()
            .Sequence()
                .Do("ChaseBall", () => {
                    if (ChaseBall()) return TaskStatus.Success;
                    else return TaskStatus.Continue;
                })
                .WaitTime(0.05f)
                .Do("Catch", () => {
                    Catch();
                    return TaskStatus.Success;
                })
                .WaitTime(0.05f)
                .Do("Throw", () => {
                    Throw();
                    return TaskStatus.Success;
                })
                .WaitTime(0.05f)

            .End()
        .End()
        .Build();
    }

    protected override void Init()
    {
        m_Enemy = (TestEnemy)m_CharacterBase;
    }


    protected override void PreStep()
    {
        m_Enemy.InitInputDirection();
    }

    private void Jump()
    {
        m_Enemy.PlayJump();
    }

    private void Catch() {
        m_Enemy.Catch(CatchUtility.SearchCatchableObject(transform.position, m_Enemy.m_CatchRadius));
    }

    private void Throw() {

        if (PlayerManager.Instance.m_Player == null) return;
        Vector3 pos = PlayerManager.Instance.m_Player.transform.position;
        Vector2 dir = pos - transform.position;

        m_Enemy.Throw(dir.normalized);

        // m_Enemy.Throw(Vector2.right * m_Enemy.m_XDirection);
    }


    private bool ChasePlayer()
    {
        if (m_CharacterBase.m_RockOnTarget == null) RockOnPlayer();

        if (m_CharacterBase.m_RockOnTarget != null)
        {
            m_Enemy.m_Direction = (Vector2)(m_CharacterBase.m_RockOnTarget.transform.position - m_CharacterBase.transform.position);
            m_Enemy.InputDirection(m_Enemy.m_Direction.normalized);
            if (GetTargetDistance() < 0.2f) return true;
        }
        return false;
    }

    private bool ChaseBall()
    {
        if (m_CharacterBase.m_RockOnTarget == null) RockOnBall();

        if (m_CharacterBase.m_RockOnTarget != null)
        {
            m_Enemy.m_Direction = (Vector2)(m_CharacterBase.m_RockOnTarget.transform.position - m_CharacterBase.transform.position);
            m_Enemy.InputDirection(m_Enemy.m_Direction.normalized);
            if (GetTargetDistance() < 0.8f) {
                return true;
            }
        }

        return false;
    }

    public bool BlowAttack() {
        if (m_CharacterBase.m_RockOnTarget == null) RockOnPlayer();

        if (m_CharacterBase.m_RockOnTarget != null)
        {
            // if(!m_Enemy.GetIsMotion()) return m_Enemy.PlayBlowAttack();
        }
        return false;
    }

    public bool Alert()
    {
        // if (!m_Enemy.GetIsMotion()) return m_Enemy.PlayAlert();
        return false;
    }

    public bool EscapeFromTarget(float th)
    {
        if (m_CharacterBase.m_RockOnTarget == null) RockOnPlayer();

        if (m_CharacterBase.m_RockOnTarget != null)
        {
            m_Enemy.m_Direction = (Vector2)(m_CharacterBase.m_RockOnTarget.transform.position - m_CharacterBase.transform.position);
            m_Enemy.m_Direction.x = -m_Enemy.m_Direction.x;
            m_Enemy.InputDirection(m_Enemy.m_Direction.normalized);

            float distance = GetTargetDistance();
            if (float.IsNaN(distance)) return false;

            if (distance > th) return true;
        }
        return false;
    }
}
