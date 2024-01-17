using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TitleEnum;

namespace TitleEnum {
    public enum Status
    {
        Title,
        Select,
        Option,
        Load
    };
}
public class TitleManager : MonoBehaviour
{
    public string m_StartGameScene;
    public string m_SelectScene;
    public string m_ArchiveScene;

    public GameObject[] panels;

    [SerializeField]
    private Status status;
    private StoryPhase m_StartPhase = StoryPhase.PhaseDefault;
    private StoryPhase m_DebugPhase = StoryPhase.Phase900;

    [SerializeField] private FMODUnity.EventReference m_SelectSound;
    [SerializeField] private FMODUnity.EventReference m_CancelSound;
    [SerializeField] private FMODUnity.EventReference m_StartSound;

    void Start()
    {
        status = Status.Title;
    }

    void Update()
    {
        bool rightPush = Input.GetMouseButtonDown(0);
        bool leftPush  = Input.GetMouseButtonDown(1);

        switch (status) {
            case Status.Title:
                if (rightPush) {
                    status = Status.Select;
                    tweenAllWindow();
                    FMODUnity.RuntimeManager.PlayOneShot(m_SelectSound, transform.position);
                }

            break;
            case Status.Select:

                break;
            case Status.Option:
                if (leftPush)
                {
                    status = Status.Select;
                    tweenAllWindow();
                    FMODUnity.RuntimeManager.PlayOneShot(m_CancelSound, transform.position);
                }
                break;
            case Status.Load:
                if (leftPush)
                {
                    status = Status.Select;
                    tweenAllWindow();
                    FMODUnity.RuntimeManager.PlayOneShot(m_CancelSound, transform.position);
                }
                break;
            default:
                break;

        }

    }

    public void tweenAllWindow() {
        foreach (var panel in panels)
        {
            if (panel == null) continue;
            panel.GetComponent<TitleSceneWindow>().Tween();
        }
    }
    public Status getStatus() {
        return status;
    }

    // SelectPanel
    public void SelectWindow_Next() {
        status = Status.Load;
        FMODUnity.RuntimeManager.PlayOneShot(m_SelectSound, transform.position);
        tweenAllWindow();
    }
    public void SelectWindow_Archive()
    {
        PlayArchive();
        FMODUnity.RuntimeManager.PlayOneShot(m_SelectSound, transform.position);
        tweenAllWindow();
    }

    public void SelectWindow_Select()
    {
        // PlaySelect();
        FMODUnity.RuntimeManager.PlayOneShot(m_StartSound, transform.position);
        tweenAllWindow();
    }

    public void SelectWindow_First()
    {
        GameStart();
        FMODUnity.RuntimeManager.PlayOneShot(m_StartSound, transform.position);
        tweenAllWindow();
    }

    public void SelectWindow_Option()
    {
        status = Status.Option;
        FMODUnity.RuntimeManager.PlayOneShot(m_SelectSound, transform.position);
        tweenAllWindow();
    }

    // Start
    public void GameStart() {
        GameManager.Instance.StartStory();
    }

    public void PlayArchive() {
        // PRJ080Manager.StartArchive();
    }


}

