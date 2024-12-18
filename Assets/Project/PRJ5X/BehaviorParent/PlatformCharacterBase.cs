﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixeye.Unity;
using RedBlueGames.Tools;

[RequireComponent(typeof(Controller2D))]
public abstract class PlatformCharacterBase : ActionGameCharacterBase, ICatcher
{
    [SerializeField, ReadOnly]
    private PlatformCharacterAI m_CharAI;

    public enum ControlState
    {
        Default,
        RockOn,
    }

    public enum StatusState
    {
        Default,
        Down,
        Guard,
        Counter,
    }

    protected Controller2D m_Controller;
    public Controller2D GetController2D() { return m_Controller; }


    protected ICatchable m_CatchableTarget;

    [SerializeField, Foldout("PlatformCharacterBase Param")]            protected bool                  m_Gravitable = true;
    
    [SerializeField, ReadOnly, Foldout("PlatformCharacterBase Param")]  protected Vector2               m_DirectionalInput;
    [SerializeField, ReadOnly, Foldout("PlatformCharacterBase Param")]  public Vector2                  m_GuardDirection;
    [SerializeField, ReadOnly, Foldout("PlatformCharacterBase Param")]  public int                      m_XDirection = 1;
    [SerializeField, ReadOnly, Foldout("PlatformCharacterBase Param")]  public ControlState             m_ControlState;
    [SerializeField, ReadOnly, Foldout("PlatformCharacterBase Param")]  public StatusState              m_StatusState;
    [SerializeField, ReadOnly, Foldout("PlatformCharacterBase Param")]  protected float                 m_Gravity;
    [SerializeField, ReadOnly, Foldout("PlatformCharacterBase Param")]  protected float                 m_MaxJumpVelocity;
    [SerializeField, ReadOnly, Foldout("PlatformCharacterBase Param")]  protected float                 m_MinJumpVelocity;
    [SerializeField, ReadOnly, Foldout("PlatformCharacterBase Param")]  protected float                 m_VelocityXSmoothing;
    [SerializeField, ReadOnly, Foldout("PlatformCharacterBase Param")]  protected Material              m_SharedMaterial;
    [SerializeField, ReadOnly, Foldout("PlatformCharacterBase Param")]  protected MaterialPropertyBlock m_MaterialPropertyBlock;
    [SerializeField, Foldout("PlatformCharacterBase Param")]            public    float                 m_CatchRadius = 0.5f;

    
    [Foldout("Setup PlatformMoveParam")]        public float    m_MaxJumpHeight = 1;
    [Foldout("Setup PlatformMoveParam")]        public float    m_MinJumpHeight =  .4f;
    [Foldout("Setup PlatformMoveParam")]        public float    m_TimeToJumpApex = .4f;
    [Foldout("Setup PlatformMoveParam")]        public float    m_AccelerationTimeAirborne = .25f;
    [Foldout("Setup PlatformMoveParam")]        public float    m_AccelerationTimeGrounded = .19f;
    [Foldout("Setup PlatformMoveParam")]        public float    m_MoveSpeed = 4;
    [Foldout("Setup PlatformMoveParam")]        public Vector2  m_WallJumpClimb;
    [Foldout("Setup PlatformMoveParam")]        public Vector2  m_WallJumpOff;
    [Foldout("Setup PlatformMoveParam")]        public Vector2  m_WwallLeap;
    [Foldout("Setup PlatformMoveParam")]        public float    m_WallSlideSpeedMax = 3;
    [Foldout("Setup PlatformMoveParam")]        public float    m_WallStickTime = .25f;
    [Foldout("Setup PlatformMoveParam")]        protected float m_TimeToWallUnstick;
    [Foldout("Setup PlatformMoveParam")]        protected bool  m_WallSliding;
    [Foldout("Setup PlatformMoveParam")]        protected int   m_WallDirX;



    public override ActionGameCharacterBase CreateInit()
    {
        base.CreateInit();
        if(m_CharAI != null) m_CharAI.ResetAI();
        m_ControlState  = ControlState.Default;
        m_StatusState   = StatusState.Default;

        m_Gravity = -(2 * m_MaxJumpHeight) / Mathf.Pow(m_TimeToJumpApex, 2);
        m_MaxJumpVelocity = Mathf.Abs(m_Gravity) * m_TimeToJumpApex;
        m_MinJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(m_Gravity) * m_MinJumpHeight);
        // m_Gravitable = true;

        m_Velocity.Set(0.0f, 0.0f, 0.0f);
        InitInputDirection();

        return this;
    }

    override protected  void Wake()
    {
        base.Wake();
        m_Controller    = GetComponent<Controller2D>();
        m_CharAI        = GetComponent<PlatformCharacterAI>();
        WakeMaterial();
    }

    virtual protected void WakeMaterial() {
    
    }

    protected override void Update()
    {
        base.Update();
        UpdateCatchableObject();
    }

    public override void ForceSetVelocityX(float velocityX)
    {
        m_Velocity.x = velocityX;
    }

    public override void ForceSetVelocityY(float velocityY)
    {
        m_Velocity.y = velocityY;
    }

    public override void ForceSetVelocity(Vector2 velocity)
    {
        m_Velocity = velocity;
    }

    protected void UpdateAI() {
        if (m_CharAI != null) m_CharAI.UpdateAI();
    }

    public void ResetStopAI() {
        if (m_CharAI != null)
        {
            m_CharAI.ResetAI();
            m_CharAI.Stop();
            InitInputDirection();
        }
    }

    public void ResumeAI()
    {
        if (m_CharAI != null) m_CharAI.Resume();
    }

    public virtual void ChangeStatusState(StatusState state)
    {
        m_StatusState = state;

        switch (state)
        {
            case PlatformCharacterBase.StatusState.Default:
                ResumeAI();
                break;
            case PlatformCharacterBase.StatusState.Down:
                ResetStopAI();
                break;
            default:
                break;
        }
    }

    public void InitInputDirection()
    {
        m_DirectionalInput.Set(0.0f, 0.0f);
    }

    public void InputDirection(Vector2 input)
    {
        m_DirectionalInput = input;
    }

    // 回転は未実装
    override public void ForceSetAngularVelocity(float velocity)
    {
    }

    public void ForceSetGravitable(bool b)
    {
        m_Gravitable = b;
    }

    public bool isLanding()
    {
        return m_Controller.collisions.below;
    }

    public void AdjustXDirection()
    {
        switch (m_ControlState)
        {
            case ControlState.Default:

                if (m_DirectionalInput.x > 0.01f)
                {
                    m_XDirection = 1;
                }
                else if (m_DirectionalInput.x < -0.01f)
                {
                    m_XDirection = -1;
                }

                break;
            case ControlState.RockOn:
                if (m_Direction.x > 0.01f)
                {
                    m_XDirection = 1;
                }
                else if (m_Direction.x < -0.01f)
                {
                    m_XDirection = -1;
                }

                break;
        }
    }

    protected virtual void UpdateBaseAnimatorParam()
    {
        //　壁すり
        m_Animator.SetBool("WallSliding", m_WallSliding);

        //　ロックオン
        if (m_ControlState == ControlState.RockOn) m_Animator.SetBool("Rockon", true);
        else m_Animator.SetBool("Rockon", false);

        //　空中or地面
        if (m_Controller.collisions.below) m_Animator.SetBool("Landing", true);
        else m_Animator.SetBool("Landing", false);

        //　左右に動いているかどうか
        if (Mathf.Abs(m_Velocity.x) > 0.01 || Mathf.Abs(m_Velocity.y) > 0.01)
        {
            m_Animator.SetBool("Move", true);
        }
        else m_Animator.SetBool("Move", false);

        // 上昇or下降
        if (m_Velocity.y > 0.1)
        {
            m_Animator.SetBool("Fall", false);
            m_Animator.SetBool("JumpUp", true);
        }
        else if (m_Velocity.y < -0.1)
        {
            m_Animator.SetBool("Fall", true);
            m_Animator.SetBool("JumpUp", false);
        }
        else
        {
            m_Animator.SetBool("JumpUp", false);
            m_Animator.SetBool("Fall", false);
        }

        // ウォークバック 向いている方向と進行方向が異なる
        if (Mathf.Sign(m_Direction.x) != Mathf.Sign(m_DirectionalInput.x)) m_Animator.SetBool("WalkBack", true);
        else m_Animator.SetBool("WalkBack", false);

        // 踏ん張り用
        if (Mathf.Sign(m_Velocity.x) != Mathf.Sign(m_DirectionalInput.x) && Mathf.Abs(m_DirectionalInput.x) > 0.01f) m_Animator.SetBool("MissDirect", true);
        else m_Animator.SetBool("MissDirect", false);
    }

    //　キャッチ ＆　スロー
    virtual public void UpdateCatchableObject()
    {
        if (m_CatchableTarget != null)
        {
            m_CatchableTarget.Carried();
        }
    }
    public bool Catch(GameObject o) {

        if (o == null) return false;

        ICatchable c = o.GetComponent<ICatchable>();
        if (c == null) return false;
        if (!c.IsCatchable()) return false;

        m_CatchableTarget = c;
        m_CatchableTarget.Catched(this);

        CatchAction(o);

        return true;
    }

    protected ThrowProperty m_ThrowProperty;
    public void Throw(Vector2 MoveValue)
    {
        if (m_CatchableTarget == null) return;

        ThrowAction(MoveValue);
        m_CatchableTarget.Throwed(ref m_ThrowProperty);
        m_CatchableTarget = null;
        
    }

    public abstract Vector3 GetHandPosition();
    public abstract void CatchAction(GameObject o);
    public abstract void ThrowAction(Vector2 MoveValue);


    protected void UpdateCollisionWithFloorCeil()
    {
        if (m_Controller.collisions.above)
        {
            if (m_Velocity.y > 0.0f) m_Velocity.y = 0;
        }


        if (m_Controller.collisions.below)
        {
            if (m_Controller.collisions.slidingDownMaxSlope)
            {
                m_Velocity.y += m_Controller.collisions.slopeNormal.y * -m_Gravity * Time.deltaTime * m_BaseMotionSpeed;
            }
            else
            {
                if (m_Velocity.y < 0.0f) m_Velocity.y = 0;
            }
        }
    }

    protected void CheckDamageDirection(RaycastHit2D hit, GameObject g = null) {
        if (g != null) m_DamageDirection = (Vector2)transform.position - (Vector2)g.transform.position;
        else m_DamageDirection = (Vector2)transform.position - hit.point;

        m_DamageDirection.y = 0.0f;
        m_DamageDirection.Normalize();
    }

    protected void HandleWallSliding()
    {
        if (GetIsMotion()) return;

        m_WallDirX = (m_Controller.collisions.left) ? -1 : 1;
        m_WallSliding = false;

        if ((m_Controller.collisions.left || m_Controller.collisions.right) && !m_Controller.collisions.below && m_Velocity.y < 0)
        {
            m_WallSliding = true;

            if (m_Velocity.y < -m_WallSlideSpeedMax)
            {
                m_Velocity.y = -m_WallSlideSpeedMax;
            }

            if (m_TimeToWallUnstick > 0)
            {
                m_VelocityXSmoothing = 0;
                m_Velocity.x = 0;

                if (m_DirectionalInput.x != m_WallDirX && m_DirectionalInput.x != 0)
                {
                    m_TimeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    m_TimeToWallUnstick = m_WallStickTime;
                }
            }
            else
            {
                m_TimeToWallUnstick = m_WallStickTime;
            }

        }
    }

    
}
