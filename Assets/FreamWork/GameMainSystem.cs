// @ 
// ゲーム統括マネージャー　シングルトン

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using DG.Tweening;
using UnityEngine.Audio;
public class GameMainSystem : SingletonMonoBehaviourFast<GameMainSystem>
{
    public bool m_Debug;

    [SerializeField, ReadOnly] private int SCREEN_RESOLITION_X = 320;
    [SerializeField, ReadOnly] private int SCREEN_RESOLITION_Y = 240;
    [SerializeField] SoundVolume g_SoundVolume;
    [SerializeField] CursorManager cursorManager;

    FMOD.Studio.VCA m_VCAMusic;
    FMOD.Studio.VCA m_VCASE;

    protected StandardKeyBind m_KeyBind;

    [Serializable]
    public class SoundVolume
    {
        [SerializeField, Range(0.0f, 1.0f)]
        public float GlobalMusicVolume = 0.35f;

        [SerializeField, Range(0.0f, 1.0f)]
        public float GlobalSEVolume = 0.8f;
    }

    private void OnValidate()
    {
        m_VCAMusic.setVolume(g_SoundVolume.GlobalMusicVolume);
        m_VCASE.setVolume(g_SoundVolume.GlobalSEVolume);
    }

    public void SetBGMDynamicVolume(float volume) {
    }

    public void SetActiveCursorType(CursorManager.CursorType cursorType)
    {
        cursorManager.SetActiveCursorType(cursorType);
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);

        m_KeyBind = new StandardKeyBind();
        m_KeyBind.Enable();

        m_VCAMusic  = FMODUnity.RuntimeManager.GetVCA("vca:/Music");
        m_VCASE     = FMODUnity.RuntimeManager.GetVCA("vca:/SE");
    }

    private void Start()
    {
        Screen.SetResolution(SCREEN_RESOLITION_X, SCREEN_RESOLITION_Y, false, 60);
        DOTween.Init();
        InitVolume();
    }

    private void InitVolume() {
        g_SoundVolume = new SoundVolume();
        m_VCAMusic.setVolume(g_SoundVolume.GlobalMusicVolume);
        m_VCASE.setVolume(g_SoundVolume.GlobalSEVolume);
    }

    public SoundVolume GetSoundVolume() {
        return g_SoundVolume;
    }

    public void SetMusicVolume(float value) {
        g_SoundVolume.GlobalMusicVolume = value;
        m_VCAMusic.setVolume(g_SoundVolume.GlobalMusicVolume);
    }
    public void SetSEVolume(float value){
        g_SoundVolume.GlobalSEVolume = value;
        m_VCASE.setVolume(g_SoundVolume.GlobalSEVolume);
    }

    public Vector2 GetBaseScreenResolution() {
        return new Vector2(SCREEN_RESOLITION_X, SCREEN_RESOLITION_Y);
    }
    public Vector2 GetResolutionMul()
    {
        Vector2 ResolutionMul = GetBaseScreenResolution() / new Vector2(Screen.width, Screen.height);
        return ResolutionMul;
    }


    public StandardKeyBind GetKeyBind()
    {
        return m_KeyBind;
    }


    public string SecondsToHMS(double secs)
    {
        TimeSpan t = TimeSpan.FromSeconds(secs);

        string answer = string.Format("{0:D2}:{1:D2}:{2:D2}",
                        t.Hours,
                        t.Minutes,
                        t.Seconds,
                        t.Milliseconds);
        return answer;
    }

    public string SecondsToHM(double secs)
    {
        TimeSpan t = TimeSpan.FromSeconds(secs);

        string answer = string.Format("{0:D2}:{1:D2}",
                        t.Hours,
                        t.Minutes,
                        t.Seconds,
                        t.Milliseconds);
        return answer;
    }

}



