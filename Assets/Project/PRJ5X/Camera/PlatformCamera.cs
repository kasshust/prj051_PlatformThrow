using UnityEngine;
using System.Collections;
using RedBlueGames.Tools;

[RequireComponent(typeof(Camera))]
public class PlatformCamera : MonoBehaviour
{

    [SerializeField, ReadOnly]
    CameraAreaInfo m_CameraAreaInfo;

    public enum FlowState {
        None,
        Player,
        RockOn
    }
    public FlowState m_FlowState = FlowState.Player;

    public PlatformCharacterBase m_CharacterBase;
    private Controller2D m_Target;

    public float m_VerticalOffset;
    public float m_DepthOffset = 10.0f;
    public float m_LookAheadDstX;
    public float m_LookSmoothTimeX;
    public float m_VerticalSmoothTime;
    public float m_DepthSmoothTime;
    public Vector2 m_FocusAreaSize;
    public Vector2 m_FixedAreaOffset;
    public float   m_FixedZoomOffset;
    FocusArea focusArea;

    float m_CurrentLookAheadX;
    float m_TargetLookAheadX;
    float m_LookAheadDirX;

    Vector3 m_Velocity;
    public float m_RockOnSmoothTime;

    
    private Camera m_Camera;

    private Vector2 m_LastAreaPos;
    private Vector2 m_CameraWorldSize;
    private WorldPoints m_WorldPoints;
    // float smoothLookVelocityX;
    // float smoothVelocityY;

    bool lookAheadStopped;

    public float m_InputGain = 0.2f;



    private void Awake()
    {
        m_Camera = GetComponent<Camera>();
    }

    public Camera GetCurrentCamera() {
        return m_Camera;
    }

    public void SetCharacterBase(PlatformCharacterBase characterBase) {
        m_CharacterBase = characterBase;
        Init();
    }

    private void Init()
    {
        if (m_CharacterBase != null)
        {
            m_Target = m_CharacterBase.GetController2D();
            focusArea = new FocusArea(m_Target.m_Collider.bounds, m_FocusAreaSize);
        }
        m_Velocity = Vector3.zero;
    }

    void Start()
    {
        // Debug以外はOFFに
        Init();
    }

    private void UpdateFocusCont2D() {

        // FocusPosの計算
        Vector3 focusPosition = focusArea.centre + Vector2.up * m_VerticalOffset;

        if (focusArea.velocity.x != 0)
        {
            m_LookAheadDirX = Mathf.Sign(focusArea.velocity.x);
            if (Mathf.Sign(m_Target.playerInput.x) == Mathf.Sign(focusArea.velocity.x) && m_Target.playerInput.x != 0)
            {
                lookAheadStopped = false;
                m_TargetLookAheadX = m_LookAheadDirX * m_LookAheadDstX;
            }
            else
            {
                if (!lookAheadStopped)
                {
                    lookAheadStopped = true;
                    m_TargetLookAheadX = m_CurrentLookAheadX + (m_LookAheadDirX * m_LookAheadDstX - m_CurrentLookAheadX) / 4f;
                }
            }
        }
        m_CurrentLookAheadX = m_TargetLookAheadX;

        // 進行方向にオフセット
        focusPosition += (Vector3)(Vector2.right * m_CurrentLookAheadX);
        

        if (Mathf.Sign(m_Target.playerInput.x) == Mathf.Sign(focusArea.velocity.x) && m_Target.playerInput.x != 0) {
            focusPosition.x += m_Target.playerInput.x * m_InputGain;
        }



        focusPosition.z = Vector3.forward.z * -m_DepthOffset;

        // クランプ
        focusPosition = ClampFocusPosition(focusPosition, m_CameraAreaInfo);

        Vector3 CurrentPos;
        CurrentPos.x = Mathf.SmoothDamp(transform.position.x, focusPosition.x, ref m_Velocity.x,        m_LookSmoothTimeX);
        CurrentPos.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref m_Velocity.y,        m_VerticalSmoothTime);
        CurrentPos.z = Mathf.SmoothDamp(transform.position.z, focusPosition.z, ref m_Velocity.z,        m_DepthSmoothTime);

        DebugUtility.DrawCircle(focusPosition, 0.3f, Color.green, 6);

        transform.position = CurrentPos;
    }

    private void UpdateFocusLockOn() {

        // FocusPosの計算
        Vector3 focusPosition = (m_CharacterBase.transform.position + m_CharacterBase.m_RockOnTarget.transform.position) / 2.0f;
        float distance = Vector2.Distance(m_CharacterBase.m_RockOnTarget.transform.position, focusPosition);
        focusPosition.z = Vector3.forward.z * (-m_DepthOffset - (distance));

        // クランプ
        focusPosition = ClampFocusPosition(focusPosition, m_CameraAreaInfo);

        Vector3 CurrentPos;
        CurrentPos = Vector3.SmoothDamp(transform.position, focusPosition, ref m_Velocity, m_RockOnSmoothTime);
        transform.position = CurrentPos;

        DebugUtility.DrawCircle(focusPosition, 0.3f, Color.green, 6);
    }

    private Vector3 ClampFocusPosition(Vector3 focusPosition, CameraAreaInfo cameraAreaInfo) {
        
        if(!cameraAreaInfo.m_IsClampCamera) return focusPosition;

        GetCameraWorldSize(ref m_CameraWorldSize, -transform.position.z);
        GetWorldPoints(ref m_WorldPoints, -transform.position.z);

        DebugUtility.DrawBox(m_WorldPoints.LeftTop, m_WorldPoints.RightBottom, Color.red);

        //　クランプ処理
        float sizeX = cameraAreaInfo.m_CameraMoveAreaSize.x / 2.0f - m_CameraWorldSize.x / 2.0f;
        float sizeY = cameraAreaInfo.m_CameraMoveAreaSize.y / 2.0f - m_CameraWorldSize.y / 2.0f;

        if (sizeX < 0) sizeX = 0;
        if (sizeY < 0) sizeY = 0;

        m_LastAreaPos = cameraAreaInfo.m_AreaPos + m_FixedAreaOffset;

        focusPosition.x = Mathf.Clamp(focusPosition.x, m_LastAreaPos.x - sizeX, m_LastAreaPos.x + sizeX);
        focusPosition.y = Mathf.Clamp(focusPosition.y, m_LastAreaPos.y - sizeY, m_LastAreaPos.y + sizeY);
        focusPosition.z += cameraAreaInfo.m_ZoomOffset + m_FixedZoomOffset;

        DebugUtility.DrawCircle(m_LastAreaPos, 0.1f,Color.red);
        DebugUtility.DrawBox(new Vector2(m_LastAreaPos.x - sizeX, m_LastAreaPos.y + sizeY), new Vector2(m_LastAreaPos.x + sizeX, m_LastAreaPos.y - sizeY), Color.blue);

        return focusPosition;
    }

    struct WorldPoints {
        public Vector2 LeftTop;
        public Vector2 RightTop;
        public Vector2 LeftBottom;
        public Vector2 RightBottom;
    }

    Vector3 tempVector;
    private void GetWorldPoints(ref WorldPoints worldPoints, float depth) {
        tempVector.Set(Screen.width, Screen.height, depth);
        worldPoints.RightTop = m_Camera.ScreenToWorldPoint(tempVector);

        tempVector.Set(0.0f, Screen.height, depth);
        worldPoints.LeftTop = m_Camera.ScreenToWorldPoint(tempVector);

        tempVector.Set(0.0f, 0.0f, depth);
        worldPoints.LeftBottom = m_Camera.ScreenToWorldPoint(tempVector);

        tempVector.Set(Screen.width, 0.0f, depth);
        worldPoints.RightBottom = m_Camera.ScreenToWorldPoint(tempVector);
    }

    private void GetCameraWorldSize(ref Vector2 Size, float depth) {
        tempVector.Set(Screen.width, Screen.height, depth);
        Vector2 RightTop = m_Camera.ScreenToWorldPoint(tempVector);

        tempVector.Set(0.0f, 0.0f, depth);
        Vector2 LeftBottom = m_Camera.ScreenToWorldPoint(tempVector);

        Size.x = RightTop.x - LeftBottom.x;
        Size.y = RightTop.y - LeftBottom.y;
    }

    public void SetCameraArea(CameraAreaInfo info) {
        m_CameraAreaInfo = info;
    }

    public void ResetCameraArea()
    {
        m_CameraAreaInfo.m_IsClampCamera = false;
    }

    private void UpdateState() {
        if (m_CharacterBase != null) {
            if (m_CharacterBase.m_ControlState == PlatformCharacterBase.ControlState.RockOn) m_FlowState = FlowState.RockOn;
            else m_FlowState = FlowState.Player;
        }

    }

    void LateUpdate()
    {
        if (m_Target == null) return;

        focusArea.Update(m_Target.m_Collider.bounds);
        UpdateState();
        switch (m_FlowState)
        {
            case FlowState.Player:
                UpdateFocusCont2D();
                break;

            case FlowState.RockOn:
                if(m_CharacterBase.m_RockOnTarget != null) UpdateFocusLockOn();
                else UpdateFocusCont2D();

                break;
            default:

                break;
        }
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(focusArea.centre, m_FocusAreaSize);
    }

    [System.Serializable]
    public struct CameraAreaInfo
    {
        public bool m_IsClampCamera;
        [ReadOnly] public Vector2 m_AreaCenter;
        public Vector2 m_AreaOffset;
        public Vector2 m_CameraMoveAreaSize;
        public Vector2 m_AreaPos { get { return m_AreaCenter + m_AreaOffset; } }
        public float m_ZoomOffset;
    }

    struct FocusArea
    {
        public Vector2 centre;
        public Vector2 velocity;
        float left, right;
        float top, bottom;


        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector2.zero;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void Update(Bounds targetBounds)
        {
            float shiftX = 0;
            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            float shiftY = 0;
            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;
            centre.Set((left + right) / 2, (top + bottom) / 2);
            velocity.Set(shiftX, shiftY);
        }
    }

}