using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Pixeye.Unity;

public abstract class ActionGameCharacterBase : ActionGameBehavior<ActionGameCharacterBase>
{
    [SerializeField, Foldout("ActionGameCharacterBase Param")]
    protected PlatformStatusObject m_PlatformStatusObject;

    [SerializeField, ReadOnly, Foldout("ActionGameCharacterBase Param")]
    public CharacterStatus       m_CharacterStatus;

    [SerializeField, Foldout("ActionGameCharacterBase Param")]
    protected Animator  m_Animator;

    protected float     m_TimeScale = 1.0f;
    protected float     m_MotionBaseSpeed = 1.0f;
    [SerializeField, ReadOnly, Foldout("ActionGameCharacterBase Param")] protected float     m_MotionSpeed = 1.0f;

    [SerializeField, ReadOnly, Foldout("ActionGameCharacterBase Param")] protected bool      m_SlowMotion;
    [SerializeField, ReadOnly, Foldout("ActionGameCharacterBase Param")] protected float     m_SlowMotionGain = 0.02f;
    [SerializeField, ReadOnly, Foldout("ActionGameCharacterBase Param")] protected float     m_SlowMotionTime = 0.0f;

    [SerializeField, ReadOnly, Foldout("ActionGameCharacterBase Param")]
    protected bool  m_ZeroGravity = false;

    [SerializeField, ReadOnly, Foldout("ActionGameCharacterBase Param")]
    protected float m_ZeroGravityTime = 0.0f;

    protected Camera m_MainCamera;


    protected override void Wake()
    {
        m_MainCamera = Camera.main;
    }

    Vector3 tempVector;
    Vector3 RightTop;
    Vector3 LeftBottom;
    protected bool CheckObjectInCamera(Transform t) {
        if (m_MainCamera == null) m_MainCamera = Camera.main;

        tempVector.Set(0.0f, 0.0f, -m_MainCamera.transform.position.z);
        LeftBottom = m_MainCamera.ScreenToWorldPoint(tempVector);

        tempVector.Set(Screen.width, Screen.height, -m_MainCamera.transform.position.z);
        RightTop = m_MainCamera.ScreenToWorldPoint(tempVector);

        if (
                t.position.x >= LeftBottom.x && t.position.x <= RightTop.x &&
                t.position.y >= LeftBottom.y && t.position.y <= RightTop.y
            )   return true; 
        else    return false;
           
    }

    public override ActionGameCharacterBase CreateInit() {
        m_SlowMotion        = false;
        m_ZeroGravity       = false;
        m_MotionSpeed       = 1.0f;
        m_SlowMotionTime    = 0.0f;
        m_ZeroGravityTime   = 0.0f;
        return this;
    }

    protected override void AdditonalCreateInit()
    {
        InitStatus();
    }

    protected void InitStatus() {
        if (m_PlatformStatusObject != null) m_CharacterStatus = m_PlatformStatusObject.m_PlatformStatus;
        else { 
            Debug.LogError(this.gameObject.name + " : PlatformStatusObjectが設定されていません　仮ステータスを作成します");
            m_CharacterStatus = new CharacterStatus();
        }
    }

    protected virtual void Update()
    {
        CheckAlive();
        CheckRockOnTarget();
        UpdateAnimatorSpeed();
        UpdateZeroGravityParam();
    }

    private void CheckAlive() {
        if (!m_CharacterStatus.IsDead && m_CharacterStatus.Hp.Value <= 0) {
            m_CharacterStatus.IsDead = true;
            Dead();
        }
    }

    public bool IsDead() {
        return m_CharacterStatus.IsDead;
    }

    abstract public void Dead();

    private void UpdateAnimatorSpeed() {
        UpdateHitStopParam();
        if (m_Animator != null) m_Animator.speed = m_MotionSpeed * m_TimeScale;
    }

    #region ステータス計算
    public void CalHp(float value) {
        m_CharacterStatus.CalHp(value);
    }

    public void CalHpMax(float value)
    {
        m_CharacterStatus.CalMaxHp(value);
    }

    public void CalFlirtEndure(float value)
    {
        m_CharacterStatus.CalFlirtEndure(value);
    }

    #endregion

    #region ZeroGravity
    private void UpdateZeroGravityParam()
    {
        if (m_ZeroGravity)
        {
            if (m_ZeroGravityTime > 0.0f) m_ZeroGravityTime = m_ZeroGravityTime - Time.deltaTime;

            if (m_ZeroGravityTime <= 0.0f)
            {
                m_ZeroGravityTime = 0.0f;
                m_ZeroGravity = false;
                
            }
        }
    }

    virtual protected void EndHitStop() { 
    
    }

    public void StartZeroGravity(float time)
    {
        m_ZeroGravity = true;
        m_ZeroGravityTime = time;
    }

    public void ForceEndZeroGravity()
    {
        m_ZeroGravity = false;
        m_ZeroGravityTime = 0.0f;
    }

    #endregion

    #region SlowMotion

    public bool IsSlowMotion() {
        return m_SlowMotion;
    }

    private void UpdateHitStopParam() {

        if (m_SlowMotion) {
            if (m_SlowMotionTime > 0.0f) m_SlowMotionTime = m_SlowMotionTime - Time.deltaTime;
            
            if (m_SlowMotionTime <= 0.0f)
            {
                m_SlowMotionTime = 0.0f;
                m_SlowMotion = false;
                EndHitStop();
            }
        }

        if (m_SlowMotion) m_MotionSpeed = m_MotionBaseSpeed * m_SlowMotionGain;
        else m_MotionSpeed = m_MotionBaseSpeed;
    }

    public virtual void StartHitStop(float time) {
        m_SlowMotion = true;
        m_SlowMotionTime = time;
    }

    public void ForceEndHitStop()
    {
        m_SlowMotion = false;
        m_SlowMotionTime = 0.0f;
    }

    #endregion

    #region Animator
    public void InitAnimInput()
    {
        SetAnimInputPress(0);
        SetAnimInputRelease(0);
        InitAllInput();
    }

    private void InitAllInput()
    {
        m_Animator.SetInteger("InputA", (int)PlatformPlayerInput.InputState.None);
        m_Animator.SetInteger("InputB", (int)PlatformPlayerInput.InputState.None);
        m_Animator.SetInteger("InputX", (int)PlatformPlayerInput.InputState.None);
        m_Animator.SetInteger("InputY", (int)PlatformPlayerInput.InputState.None);
    }

    public void SetAnimInputPress(int num)
    {
        m_Animator.SetInteger("InputPress", num);

        switch ((PlatformPlayerInput.AnimInput)num)
        {
            case PlatformPlayerInput.AnimInput.A:
                m_Animator.SetInteger("InputA", (int)PlatformPlayerInput.InputState.Press);
                break;
            case PlatformPlayerInput.AnimInput.B:
                m_Animator.SetInteger("InputB", (int)PlatformPlayerInput.InputState.Press);
                break;
            case PlatformPlayerInput.AnimInput.X:
                m_Animator.SetInteger("InputX", (int)PlatformPlayerInput.InputState.Press);
                break;
            case PlatformPlayerInput.AnimInput.Y:
                m_Animator.SetInteger("InputY", (int)PlatformPlayerInput.InputState.Press);
                break;
            default:
                break;
        }
    }

    public void SetAnimInputRelease(int num)
    {
        m_Animator.SetInteger("InputRelease", num);

        switch ((PlatformPlayerInput.AnimInput)num)
        {
            case PlatformPlayerInput.AnimInput.A:
                m_Animator.SetInteger("InputA", (int)PlatformPlayerInput.InputState.Release);
                break;
            case PlatformPlayerInput.AnimInput.B:
                m_Animator.SetInteger("InputB", (int)PlatformPlayerInput.InputState.Release);
                break;
            case PlatformPlayerInput.AnimInput.X:
                m_Animator.SetInteger("InputX", (int)PlatformPlayerInput.InputState.Release);
                break;
            case PlatformPlayerInput.AnimInput.Y:
                m_Animator.SetInteger("InputY", (int)PlatformPlayerInput.InputState.Release);
                break;
            default:
                break;
        }
    }
    public void SetAnimInputHold(int num)
    {
        // m_Animator.SetInteger("InputHold", num);

        switch ((PlatformPlayerInput.AnimInput)num)
        {
            case PlatformPlayerInput.AnimInput.A:
                m_Animator.SetInteger("InputA", (int)PlatformPlayerInput.InputState.Hold);
                break;
            case PlatformPlayerInput.AnimInput.B:
                m_Animator.SetInteger("InputB", (int)PlatformPlayerInput.InputState.Hold);
                break;
            case PlatformPlayerInput.AnimInput.X:
                m_Animator.SetInteger("InputX", (int)PlatformPlayerInput.InputState.Hold);
                break;
            case PlatformPlayerInput.AnimInput.Y:
                m_Animator.SetInteger("InputY", (int)PlatformPlayerInput.InputState.Hold);
                break;
            default:
                break;
        }
    }

    public void SetAnimInputBarrage(float A, float B, float X, float Y) {
        m_Animator.SetFloat("InputABarrage", A);
        m_Animator.SetFloat("InputBBarrage", B);
        m_Animator.SetFloat("InputXBarrage", X);
        m_Animator.SetFloat("InputYBarrage", Y);
    }


    public void SetAnimInputHorizontal(float value)
    {
        m_Animator.SetFloat("InputHorizontal", value);
    }
    public void SetAnimInputVertical(float value)
    {
        m_Animator.SetFloat("InputVertical", value);
    }

    public void SetAnimInputBarrage() {
    
    }

    public bool GetRigid()
    {
        return false;
        // return m_Animator.GetBool("Rigid");
    }

    public bool GetIsMotion()
    {
        return false;
        // return m_Animator.GetBool("IsMotion");
    }

    #endregion

    #region ロックオン
    [ReadOnly, Foldout("ActionGameCharacterBase Param")] public GameObject m_RockOnTarget;

    // Deactiveの場合ロックオンをはずす
    private void CheckRockOnTarget()
    {
        if (m_RockOnTarget != null)
        {
            if (m_RockOnTarget.activeInHierarchy == false)
            {
                m_RockOnTarget = null;
                ReRockOnTarget();
            }
        }
    }

    private RaycastHit2D[] hits = new RaycastHit2D[5];
    public GameObject GetTargetClosestCharNonAlloc(Vector3 position, float radius, LayerMask mask, bool isInCamera = false)
    {
        int results = Physics2D.CircleCastNonAlloc(position, radius, Vector2.zero, hits, Mathf.Infinity, mask);
        
        if (results > 0)
        {
            float min_target_distance = float.MaxValue;
            GameObject target = null;

            foreach (var hit in hits)
            {
                if (isInCamera)
                {
                    if (!CheckObjectInCamera(hit.transform)) continue;
                }

                float target_distance = Vector3.Distance(transform.position, hit.transform.position);

                if (target_distance < min_target_distance)
                {
                    min_target_distance = target_distance;
                    target = hit.transform.gameObject;
                }
            }

            return target;
        }
        else
        {
            return null;
        }
    }
    
    public GameObject GetTargetClosestChar(Vector3 position, float radius, LayerMask mask, bool isInCamera = false)
    {
        var hits = Physics2D.CircleCastAll(position, radius, Vector2.zero, 100.0f, mask);

        if (hits.Count() > 0)
        {
            float min_target_distance = float.MaxValue;
            GameObject target = null;

            foreach (var hit in hits)
            {
                if (isInCamera) {
                    if (!CheckObjectInCamera(hit.transform)) continue;
                }

                float target_distance = Vector3.Distance(transform.position, hit.transform.position);

                if (target_distance < min_target_distance)
                {
                    min_target_distance = target_distance;
                    target = hit.transform.gameObject;
                }
            }

            return target;
        }
        else
        {
            return null;
        }
    }

    protected virtual void ReRockOnTarget() {
    
    }
    
    public void RockOnTarget(Vector3 position, float radius, LayerMask mask, bool isInCamera = false)
    {
        m_RockOnTarget = GetTargetClosestChar(
           position,
           radius,
           mask,
           isInCamera
        );
    }

    public void RockOnTargetNonAlloc(Vector3 position, float radius, LayerMask mask, bool isInCamera = false)
    {
        m_RockOnTarget = GetTargetClosestCharNonAlloc(
           position,
           radius,
           mask,
           isInCamera
        );
    }

    public void RockOffTarget()
    {
        m_RockOnTarget = null;
    }
    #endregion

    abstract public void ForceSetVelocityX(float velocityX);
    abstract public void ForceSetVelocityY(float velocityY);
    abstract public void ForceSetVelocity(Vector2 velocity);
    abstract public void ForceSetAngularVelocity(float velocity);

    virtual public void  AddVelocity(Vector2 velocity) { }

    abstract public void ChatchImpactReply(ref PlatformActionManager.ReplyInfo replyInfo);
}
