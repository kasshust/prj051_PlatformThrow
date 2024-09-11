using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Pixeye.Unity;

public abstract class ActionGameCharacterBase : ActionGameBehavior<ActionGameCharacterBase>
{
    protected Camera m_MainCamera;
    protected float  m_TimeScale = 1.0f;
    protected float  m_MotionBaseSpeed = 1.0f;

    [SerializeField, Foldout("ActionGameCharacterBase Param")] protected PlatformStatusObject           m_PlatformStatusObject;
    [SerializeField, Foldout("ActionGameCharacterBase Param")] protected Animator                       m_Animator;
    [SerializeField, Foldout("ActionGameCharacterBase Param")] protected float                          m_TempInvincibleTime = 0.2f;

    [SerializeField, Foldout("ActionGameCharacterBase Param"), ReadOnly] public CharacterStatus         m_CharacterStatus;
    [SerializeField, Foldout("ActionGameCharacterBase Param"), ReadOnly] protected float                m_CurrentTempInvincibleTime = 0.0f;
    [SerializeField, Foldout("ActionGameCharacterBase Param"), ReadOnly] protected float                m_SlowMotionGain = 0.02f;
    [SerializeField, Foldout("ActionGameCharacterBase Param"), ReadOnly] protected float                m_BaseMotionSpeed = 1.0f;
    [SerializeField, Foldout("ActionGameCharacterBase Param"), ReadOnly] protected bool                 m_SlowMotion;
    [SerializeField, Foldout("ActionGameCharacterBase Param"), ReadOnly] protected float                m_SlowMotionTime = 0.0f;
    [SerializeField, Foldout("ActionGameCharacterBase Param"), ReadOnly] protected bool                 m_ZeroGravity = false;
    [SerializeField, Foldout("ActionGameCharacterBase Param"), ReadOnly] protected float                m_ZeroGravityTime = 0.0f;
    [SerializeField, Foldout("ActionGameCharacterBase Param"), ReadOnly] public GameObject              m_RockOnTarget;

    [SerializeField, ReadOnly, Foldout("ActionGameCharacterBase Param")] protected Vector3 m_Velocity;
    [SerializeField, ReadOnly, Foldout("ActionGameCharacterBase Param")] public Vector2    m_Direction;
    [SerializeField, ReadOnly, Foldout("ActionGameCharacterBase Param")] public Vector2    m_DamageDirection;


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
        m_BaseMotionSpeed       = 1.0f;
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
        CheckTempInvincible();
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

    private void CheckTempInvincible()
    {
        if (m_CharacterStatus.IsTempInvincible)
        {
            m_CurrentTempInvincibleTime += Time.deltaTime;
            if (m_CurrentTempInvincibleTime >= m_TempInvincibleTime) {
                m_CurrentTempInvincibleTime = 0.0f;
                m_CharacterStatus.IsTempInvincible = false;
            }
        }
    }

    public void SetTempInvincible(bool b) {
        m_CharacterStatus.IsTempInvincible = b;
    }

    public bool IsDead() {
        return m_CharacterStatus.IsDead;
    }

    abstract public void Dead();

    private void UpdateAnimatorSpeed() {
        UpdateHitStopParam();
        if (m_Animator != null) m_Animator.speed = m_BaseMotionSpeed * m_TimeScale;
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

        if (m_SlowMotion) m_BaseMotionSpeed = m_MotionBaseSpeed * m_SlowMotionGain;
        else m_BaseMotionSpeed = m_MotionBaseSpeed;
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
    public GameObject GetTargetClosestObjectNonAlloc(Vector3 position, float radius, LayerMask mask, bool isInCamera = false)
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
    
    public GameObject GetTargetClosestObject(Vector3 position, float radius, LayerMask mask, bool isInCamera = false)
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
        m_RockOnTarget = GetTargetClosestObject(
           position,
           radius,
           mask,
           isInCamera
        );
    }

    public void RockOnTargetNonAlloc(Vector3 position, float radius, LayerMask mask, bool isInCamera = false)
    {
        m_RockOnTarget = GetTargetClosestObjectNonAlloc(
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

    public abstract void Damage(ref PlatformActionManager.AttackInfo attackInfo, RaycastHit2D hit, GameObject g = null);
    abstract public void ChatchImpactReply(ref PlatformActionManager.ReplyInfo replyInfo);
}
