using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;
using UniSearchableEnumAttribute;

public class EventSceneManager : MonoBehaviour
{

    [SerializeField, ReadOnly]
    public Flowchart Flowchart;
    string message = "PhaseDefault";
    string endText = "_End";

    [SerializeField, SearchableEnum, ReadOnly] StoryPhase m_cPhase;

    [SerializeField, ReadOnly]
    private Button m_button;

    [SerializeField, ReadOnly]
    private bool m_isWating;

    [SerializeField]
    private bool m_PhaseDebug = false;

    [SerializeField,ReadOnly]
    private MapCommonPanel m_Slide1;

    [SerializeField, ReadOnly]
    private MapCommonPanel m_Slide2;

    [SerializeField, ReadOnly]
    private MapCommonPanel m_TimePanel;


    private void Start() {
        Flowchart.SetBooleanVariable("Recorder", StoryManager.Instance.GetState() == PhasePlayState.Recoeder);
        InitEventScene();
    }

    private void InitEventScene() {
        m_isWating = true;
        // BGMManager.Instance.FadeOut();
    }

    private void Update()
    {
        if (m_isWating && !TransitionManager.Instance.isPlayTransition()) {
            LoadPhaseEvent();
            m_isWating = false;
        };
    }

    // public string m_RoomSound = BGMPath.ROOM_SOUND;

    public void MusicStart(string path)
    {
        // m_RoomSound = path;
        // BGMManager.Instance.Play(m_RoomSound, 0.2f, 0, 1, true, false);
        /*
        BGMManager.Instance.FadeIn(m_RoomSound, 2, () => {
            Debug.Log("BGMフェードイン終了");
        });
        */
    }

    public void JingleStart(AudioClip source) {
        // BGMManager.Instance.Play(source, 0.2f, 0, 1, false, false);
    }

    private void MusicEnd()
    {
        /*
        BGMManager.Instance.FadeOut(m_RoomSound, 2, () => {
            Debug.Log("BGMフェードアウト終了");
        });
        */
    }

    private void LoadPhaseEvent() {
        if (m_PhaseDebug)
        {
            Debug.Log("Debug状態のためフェーズを呼び出しません");
            return;
        }
        else m_cPhase = StoryManager.Instance.GetPhase();

        if (m_cPhase == StoryPhase.PhaseDefault) {
            Debug.Log("フェーズがデフォルトなため、フェーズを呼び出しません");
            return;
        }

        FlagPhase(m_cPhase);
        message = m_cPhase.ToString();
        FireEvent(m_cPhase.ToString());
    }

    public void FlagPhase(StoryPhase phase) {
        GameDataBase.Instance.m_CurrentSaveData.m_PhaseFlag[(int)phase] = true;
    }

    public void FireEvent(string phaseName) {
        Flowchart.ExecuteBlock(phaseName);
    }

    public void FireEvent() {
        Flowchart.SendFungusMessage(message);
    }

    public void SkipEvent() {
        m_button.enabled = false;
        Flowchart.StopAllBlocks();
        Flowchart.ExecuteBlock(m_cPhase.ToString() + endText);
        MusicEnd();
    }


    public void OpenSlide() {
        m_Slide1.Open();
        m_Slide2.Open();
        m_TimePanel.Open();
    }

    public void CloseSlide()
    {
        m_Slide1.Close();
        m_Slide2.Close();
        m_TimePanel.Close();
    }

}
