using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public abstract class Transition : MonoBehaviour
{
    protected enum TransitionState
    {
        IN,
        OUT,
        FINISH
    }
    TransitionState currentState;

    [SerializeField] private bool m_OptionalTiming;


    protected float currentTime;

    [Range(0, 5)]
    public float inTime;

    [Range(0, 5)]
    public float outTime;
    protected bool isfinish;

    private bool b_ininit;
    private bool b_outinit;


    protected string SceneName;
    public void setScene(string SceneName)
    {
        this.SceneName = SceneName;
    }

    public string getScene()
    {
        return this.SceneName;
    }

    public bool isFinish()
    {
        return isfinish;
    }

    virtual protected void Awake()
    {
        currentTime = 0;
        isfinish = false;
        b_ininit = false;
        b_outinit = false;
        ChangeState(TransitionState.IN);
        DontDestroyOnLoad(this.gameObject);
    }

    protected void ChangeState(TransitionState state) {
        currentState = state;
    }

    private void Update()
    {
        switch (currentState)
        {
            case TransitionState.IN:
                if (b_ininit == false)
                {
                    InInit();
                    b_ininit = true;
                }
                else InProcess();

                if (m_OptionalTiming) return;

                if (currentTime >= inTime)
                {
                    currentTime = 0;
                    ChangeState(TransitionState.OUT);
                    Interval();

                    SceneManager.LoadScene(SceneName);
                }

                break;

            case TransitionState.OUT:
                if (b_outinit == false)
                {
                    OutInit();
                    b_outinit = true;
                }
                else OutProcess();

                if (m_OptionalTiming) return;

                if (currentTime >= outTime)
                {
                    currentTime = 0;
                    ChangeState(TransitionState.FINISH);
                }
                break;

            case TransitionState.FINISH:
                Finish();
                isfinish = true;
                break;
        }

        if(!m_OptionalTiming) currentTime += Time.deltaTime;
    }

    virtual protected void InInit() { }
    virtual protected void InProcess() { }

    virtual protected void Interval() { }

    virtual protected void OutInit() { }
    virtual protected void OutProcess() { }
    virtual protected void Finish() { }

}
