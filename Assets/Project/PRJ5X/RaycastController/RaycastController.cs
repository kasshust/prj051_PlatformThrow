using UnityEngine;
using System.Collections;

// [RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
    public const float skinWidth = .025f;
    // const float dstBetweenRays = .04f;


    [SerializeField, Range(1,10)]
    public int m_HorizontalRayCount = 2;
    [SerializeField, Range(1, 10)]
    public int m_VerticalRayCount   = 2;

    [HideInInspector]
    public float m_HorizontalRaySpacing;
    [HideInInspector]
    public float m_VerticalRaySpacing;

    [HideInInspector]
    public BoxCollider2D m_Collider;
    public RaycastOrigins m_RaycastOrigins;

    public virtual void Awake()
    {
        m_Collider = GetComponent<BoxCollider2D>();
        if (m_Collider == null) this.enabled = false;
    }

    public virtual void Start()
    {
        CalculateRaySpacing();
    }

    public void UpdateRaycastOrigins()
    {
        Bounds bounds = m_Collider.bounds;
        bounds.Expand(skinWidth * -2);

        m_RaycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        m_RaycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        m_RaycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        m_RaycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public void CalculateRaySpacing()
    {
        Bounds bounds = m_Collider.bounds;
        bounds.Expand(skinWidth * -2);

        // float boundsWidth = bounds.size.x;
        // float boundsHeight = bounds.size.y;
        // horizontalRayCount = Mathf.RoundToInt(boundsHeight / dstBetweenRays);
        // verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);

        if (m_HorizontalRayCount >= 2) {
            m_HorizontalRaySpacing = bounds.size.y / (m_HorizontalRayCount - 1);
        }else m_HorizontalRaySpacing = bounds.size.y / 2.0f;

        if (m_VerticalRayCount >= 2) {
            m_VerticalRaySpacing = bounds.size.x / (m_VerticalRayCount - 1);
        }m_VerticalRaySpacing = bounds.size.x / 2.0f;

    }

    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}
