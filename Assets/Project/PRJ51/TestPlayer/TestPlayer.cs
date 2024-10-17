#pragma warning disable 0414

using UnityEngine;
using RedBlueGames.Tools;
using Pixeye.Unity;
using System;

using System.Collections.Generic;
using System.Reflection;
using System.Linq;


public class TestPlayer : PlatformPlayerBase
{
    PlayerManager                      m_PlayerManager;
    private Controller2D.CollisionInfo m_Pre_collisions;
    string[]                           m_NotRigidStates;
    AnimatorStateInfo                  m_AnimatorStateinfo;
    private PlayerController2D         m_PlayerController2D;

    [SerializeField, ReadOnly, Foldout("TestPlayer Param")] float                            m_MaxVelocity;
    [SerializeField, Foldout("TestPlayer Param")] float                                      m_ThrowVelocity = 20.0f;
    [SerializeField, Foldout("SE")]                         private FMODUnity.EventReference m_EnemyStepSE;
    [SerializeField, Foldout("SE")]                         private FMODUnity.EventReference m_ThrowSound;
    [SerializeField, Foldout("SE")]                         private FMODUnity.EventReference m_CatchSound;
    [SerializeField, Foldout("SE")]                         private FMODUnity.EventReference m_CatchSuccessSound;

    protected override void Wake()
    {
        base.Wake();
    }

    public override ActionGameCharacterBase CreateInit()
    {
        // Rigid解除モーション(念のために実装)
        m_NotRigidStates = new string[] {
            "Normal",
            "Hunbari",
            "RunRockOff",
            "RunRockOn",
            "RunBackRockon",
            "AirRotate",
            "AirFall",
            "Land",
            "LandRoll"
        };

        if (PlayerManager.Instance != null) m_PlayerManager = PlayerManager.Instance;
        else Debug.LogError("PlayerManager.Instance が存在しません");
        m_ControlState = ControlState.Default;

        m_PlayerController2D = (PlayerController2D)m_Controller;

        m_Gravity = -(2 * m_MaxJumpHeight) / Mathf.Pow(m_TimeToJumpApex, 2);
        m_MaxJumpVelocity = Mathf.Abs(m_Gravity) * m_TimeToJumpApex;
        m_MinJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(m_Gravity) * m_MinJumpHeight);
        m_Gravitable = true;

        return this;
    }

    protected override void WakeMaterial()
    {
    }

    public override void Dead()
    {

    }

    public override void RockOn()
    {
        base.RockOn();
    }

    #region UPDATE
    protected override void Update()
    {
        base.Update();

        //　モーション状態の初期化(Animatorでいちいち設定するのが面倒なので)
        ForceInitChainState();                      // チェイン状態の解放
        ForceOffRigid();                            // 硬直の解放
        ForceOffIsMotion();                         // モーション状態(移動不可)の解放

        // コントロール
        UpdateControlDirection();                   // デフォルトとロックオンでは方向の決定方法が異なる　
        UpdateControl();                            // 移動量を適用
        UpdateCollisionWithFloorCeil();             // 主に天井や床による速度制限
        UpdateXDirection();

        // アニメーション・効果音
        UpdateBaseAnimatorParam();
        // UpdateAnimatorParam();
        // UpdateAnimation();
        // UpdateSoundEffect();
    }



    private void UpdateControlDirection()
    {
        switch (m_ControlState)
        {
            case ControlState.Default:

                if (m_DirectionalInput.x > 0.01f)
                {
                    m_Direction.Set(1.0f, 0.0f);
                }
                else if (m_DirectionalInput.x < -0.01f)
                {
                    m_Direction.Set(-1.0f, 0.0f);
                }
                Debug.DrawRay(transform.position, m_Direction);

                break;
            case ControlState.RockOn:

                m_AnimatorStateinfo = m_Animator.GetCurrentAnimatorStateInfo(0);
                if (m_AnimatorStateinfo.IsName("WallSliding"))
                {
                    m_Direction = m_DirectionalInput.normalized;
                }
                else
                {
                    if (m_RockOnTarget != null)
                    {
                        Vector3 dir = m_RockOnTarget.transform.position - transform.position;
                        m_Direction = dir.normalized;
                    }
                    else
                    {
                        m_Direction.Set(m_SavedRockOnXDir, 0.0f);
                    }
                    return;

                }

                Debug.DrawRay(transform.position, m_Direction);
                break;

        }
    }

    private void UpdateControl()
    {
        m_Pre_collisions = m_Controller.collisions;
        CalculateVelocity();
        HandleWallSliding();
        Move();
    }

    void CalculateVelocity()
    {
        m_MaxVelocity = 0.0f;
        if (GetRigid() || GetIsMotion())
        {
            m_MaxVelocity = 0.0f;
        }
        else
        {
            switch (m_ControlState)
            {
                case ControlState.Default:
                    m_MaxVelocity = m_DirectionalInput.x * m_MoveSpeed;
                    break;
                case ControlState.RockOn:
                    float gain = 0.5f;
                    m_MaxVelocity = m_DirectionalInput.x * m_MoveSpeed * gain;
                    break;
            }

        }

        if (m_MaxVelocity == m_Velocity.x) { m_VelocityXSmoothing = 0.0f; }

        m_Velocity.x = Mathf.SmoothDamp(m_Velocity.x, m_MaxVelocity, ref m_VelocityXSmoothing, (m_Controller.collisions.below) ? m_AccelerationTimeGrounded : m_AccelerationTimeAirborne);
        if (m_Gravitable) m_Velocity.y += m_Gravity * Time.deltaTime * m_BaseMotionSpeed;
    }

    private void Move()
    {
        m_Controller.Move(m_Velocity * Time.deltaTime * m_BaseMotionSpeed * m_SpeedMultiplier, m_DirectionalInput, false, m_BaseMotionSpeed);
    }

    private void UpdateXDirection()
    {
        if (GetRigid() || GetIsMotion()) return;
        AdjustXDirection();
    }

    // アニメーション時判定
    /*
    [SerializeField, Foldout("PlayerTypeA Param")] LayerMask m_StingerMask;
    private void UpdateAnimatorParam()
    {
        m_AnimatorStateinfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (m_AnimatorStateinfo.IsName("Stinger"))
        {
            Debug.DrawRay(transform.position, Vector2.right * m_XDirection);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * m_XDirection, 0.2f, m_StingerMask);
            if (hit)
            {
                m_Animator.Play("StingerEnd");
            }
        }
    }
    */

    private void UpdateAnimation()
    {

        if (m_XDirection == 1)
        {
            // m_SpriteObj.transform.localRotation = Quaternion.identity;
        }
        else
        {
            // m_SpriteObj.transform.localRotation = Quaternion.AngleAxis(180.0f, Vector3.down) * Quaternion.identity;
        }

        UpdateAnimationSpeed();
    }

    private void UpdateAnimationSpeed()
    {
        m_AnimatorStateinfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (m_AnimatorStateinfo.IsName("RunRockOff") ||
                m_AnimatorStateinfo.IsName("RunRockOn") ||
                m_AnimatorStateinfo.IsName("RunbackRockOn")
           )
        {
            m_Animator.SetFloat("SpeedMultiplier", Mathf.Abs(m_Velocity.x) * m_SpeedMultiplier);
        }
        else
        {
            m_Animator.SetFloat("SpeedMultiplier", 1.0f * m_SpeedMultiplier);
        }

        m_Animator.SetFloat("ShotCycleSpeed", 1.0f * m_SpeedMultiplier);
    }

    private void UpdateSoundEffect()
    {
        if (m_Pre_collisions.below == false && m_Controller.collisions.below)
        {
            // SEManager.Instance.Play(SEPath.SE_LAND);
        }
    }

    private void ForceInitChainState()
    {
        /*
        m_AnimatorStateinfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        foreach (string name in m_NotRigidStates)
        {
            if (m_AnimatorStateinfo.IsName(name))
            {
                m_Animator.SetInteger("ChainState", 0);
            }
        }
        */
    }

    private void ForceOffRigid()
    {
        if (!GetRigid()) return;

        m_AnimatorStateinfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        foreach (string name in m_NotRigidStates)
        {
            if (m_AnimatorStateinfo.IsName(name)) m_Animator.SetBool("Rigid", false);
        }
    }

    private void ForceOffIsMotion()
    {

        if (!GetIsMotion()) return;

        m_AnimatorStateinfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        foreach (string name in m_NotRigidStates)
        {
            if (m_AnimatorStateinfo.IsName(name))
            {
                m_Animator.SetBool("IsMotion", false);
            }
        }
    }

    #endregion

    #region INPUT

    public enum BaseMotionType
    {
        Move,
        Jump,
        OnJump
    }

    public void BaseMotionInput(BaseMotionType baseMotionType, Vector2 input)
    {
        switch (baseMotionType)
        {
            case BaseMotionType.Move:

                InputDirection(input);

                break;
            case BaseMotionType.Jump:

                if (!GetRigid())
                {
                    if (!PlayJump())
                    {
                        EnemyStep();
                    };
                }
                else
                {
                    EnemyStep();
                }

                break;
            case BaseMotionType.OnJump:

                if (!GetRigid()) PlayOnJumpUp();

                break;

            default:
                break;
        }
    }


    // ローリング入力
    public void RollingInput(Vector2 input)
    {
        PlayRolling(input);
    }



    #endregion

    #region ACTION

    public void HnadAction(Vector2 moveValue) {
        if (m_CatchableTarget == null)
        {
            FMODUnity.RuntimeManager.PlayOneShot(m_CatchSound, transform.position);
            if (Catch(CatchUtility.SearchCatchableObject(transform.position, m_CatchRadius))) {
                FMODUnity.RuntimeManager.PlayOneShot(m_CatchSuccessSound, transform.position);
            };
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot(m_ThrowSound, transform.position);
            Throw(moveValue);
        }
    }

    public override void CatchAction(GameObject o)
    {
        o.transform.position = transform.position;
    }

    public override void ThrowAction(Vector2 moveValue)
    {
        m_ThrowProperty.Velocity = moveValue * m_ThrowVelocity;
        m_ThrowProperty.AttackSet = PlatformActionManager.AttackSet.Player;
    }


    // ジャンプ時
    public bool PlayJump()
    {
        //　壁接触時
        if (m_WallSliding)
        {
            if (Mathf.Abs(m_DirectionalInput.x) < 0.05f) //　x軸入力無し
            {
                m_Velocity.x = -m_WallDirX * m_WallJumpOff.x;
                m_Velocity.y = m_WallJumpOff.y;

                return true;
            }
            else if (Mathf.Sign(m_WallDirX) == Mathf.Sign(m_DirectionalInput.x)) // 壁方向
            {
                m_Velocity.x = -m_WallDirX * m_WallJumpClimb.x;
                m_Velocity.y = m_WallJumpClimb.y;

                return true;
            }
            else //逆側
            {
                m_Velocity.x = -m_WallDirX * m_WwallLeap.x;
                m_Velocity.y = m_WwallLeap.y;

                return true;
            }
        }

        //　接地時
        if (m_Controller.collisions.below)
        {
            if (m_Controller.collisions.slidingDownMaxSlope)　// 坂を降りてる
            {
                if (m_DirectionalInput.x != -Mathf.Sign(m_Controller.collisions.slopeNormal.x))
                { // not jumping against max slope
                    m_Velocity.y = m_MaxJumpVelocity * m_Controller.collisions.slopeNormal.y;
                    m_Velocity.x = m_MaxJumpVelocity * m_Controller.collisions.slopeNormal.x;

                    return true;

                }
            }
            else
            {
                m_Velocity.y = m_MaxJumpVelocity;

                return true;
            }
        }

        return false;
    }

    private void EnemyStep()
    {
        if (m_PlayerController2D.m_SubCollisionWithEnemy.collider == null) return;
        m_Velocity.y = m_MaxJumpVelocity;
        EnemyStepImpact(m_PlayerController2D.m_SubCollisionWithEnemy);
        m_Animator.Play("EnemyStep");
        FMODUnity.RuntimeManager.PlayOneShot(m_EnemyStepSE, transform.position);
    }

    protected PlatformActionManager.AttackInfo m_AttackInfo;
    [ReadOnly] protected PlatformActionManager.ReplyInfo m_ReplyInfo;
    protected void EnemyStepImpact(RaycastHit2D hit)
    {
        if (hit.collider.TryGetComponent(out BehaviorImpactReceiver receiver))
        {
            m_AttackInfo.AttackType = PlatformActionManager.AttackType.Default;
            m_AttackInfo.Direction.Set(1.0f, 0.0f);
            m_AttackInfo.ImpactValue = 0.01f;
            m_AttackInfo.DamageValue = 0.0f;
            m_AttackInfo.OverriteVelocity = true;
            m_AttackInfo.ZeroGravity = true;
            m_AttackInfo.ZeroGravityTime = 0.03f;
            receiver.ReceiveImpactGetReply(ref m_AttackInfo, hit, ref m_ReplyInfo, this.gameObject);
        }
    }

    // ジャンプ上昇時
    public void PlayOnJumpUp()
    {
        if (m_Velocity.y > m_MinJumpVelocity)
        {
            m_Velocity.y = m_MinJumpVelocity;
        }
    }

    private void PlayRolling(Vector2 input)
    {
        if (m_Controller.collisions.below && GetRigid() == false)
        {
            if (Mathf.Sign(input.x) == Mathf.Sign(m_Direction.x))
            {
                m_Animator.Play("FrontRolling");
            }
            else
            {
                m_Animator.Play("BackRolling");
            }
        }
    }

    Vector3 WarpOffset = new Vector3(0.1f, 0.0f, 0.0f);
    public void Warp()
    {
        if (m_RockOnTarget != null)
        {
            Vector3 pos = m_RockOnTarget.transform.position;
            transform.position = pos - WarpOffset * m_XDirection;
            m_Velocity.Set(0.0f, 0.0f, 0.0f);
        }
    }

    #endregion

    #region コマンド

    #endregion

    #region ダメージ
    public override void Damage(ref PlatformActionManager.AttackInfo attackInfo, RaycastHit2D hit, GameObject g = null)
    {
        // CheckDamageDirection(hit, g);

        m_DamageDirection = attackInfo.Direction.normalized;
        ForceSetVelocity(attackInfo.Direction * attackInfo.ImpactValue);

        // VFXController.Instance.StartCameraShake(0.10f, 0.1f);
        // VFXController.Instance.StartRadBlur(0.5f, 0.2f, Vector2.zero);

        if (isLanding())
        {
            m_Animator.Play("DamageStand");
        }
        else
        {
            m_Animator.Play("DamageAir");
        }
    }

    public override void ChatchImpactReply(ref PlatformActionManager.ReplyInfo replyInfo)
    {
        if (replyInfo.m_Counter)
        {
            Debug.Log("カウンター受ける");
        }
    }

    #endregion

    #region その他
    
    public override Vector3 GetHandPosition()
    {
        return transform.position;
    }

    #endregion


    #region debug

    private Vector2 RotateVector2(Vector2 v, float value)
    {
        return new Vector2(
                    v.x * Mathf.Cos(value) - v.y * Mathf.Sin(value),
                    v.x * Mathf.Sin(value) + v.y * Mathf.Cos(value)
                );
    }



    #endregion

}