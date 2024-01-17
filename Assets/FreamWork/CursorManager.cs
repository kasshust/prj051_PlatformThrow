using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private List<CursorAnimation> cursorAnimationList;

    private CursorAnimation cursorAnimation;

    [Header("デバッグ情報")]

    [SerializeField, ReadOnly]
    private int currentFrame;

    [SerializeField, ReadOnly]
    private float frameTimer;

    [SerializeField, ReadOnly]
    private int frameCount;


    public enum CursorType
    {
        Default,
        Check
    }

    [System.Serializable]
    public class CursorAnimation
    {
        public CursorType cursorType;
        public Texture2D[] cursortextureArray;
        public float frameRate;
        public Vector2 offset;

    }

    private void Awake()
    {
        if (cursorAnimationList.Count == 0) Debug.LogError("カーソルの設定ができていません");
        SetActiveCursorType(CursorType.Default);
    }

    private void Update()
    {
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            frameTimer += cursorAnimation.frameRate;
            currentFrame = (currentFrame + 1) % frameCount;
            Cursor.SetCursor(cursorAnimation.cursortextureArray[currentFrame], cursorAnimation.offset, CursorMode.Auto);
        }
    }

    public void SetActiveCursorType(CursorType cursorType) {
        SetActiveCursorAnimation(GetCursorAnimation(cursorType));
    }
    private CursorAnimation GetCursorAnimation(CursorType cursorType)
    {
        foreach (CursorAnimation cursorAnimation in cursorAnimationList)
        {
            if (cursorAnimation.cursorType == cursorType)
            {
                return cursorAnimation;
            }
        }
        return null;
    }
    private void SetActiveCursorAnimation(CursorAnimation cursorAnimation)
    {
        this.cursorAnimation = cursorAnimation;
        currentFrame = 0;
        frameTimer = cursorAnimation.frameRate;
        frameCount = cursorAnimation.cursortextureArray.Length;
    }



}
