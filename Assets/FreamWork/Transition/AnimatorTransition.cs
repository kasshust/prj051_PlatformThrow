using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public abstract class AnimatorTransition : Transition
{
    [SerializeField]    private Animator m_Animator;
    [SerializeField]    private string m_InAnimationName;
    [SerializeField]    private string m_OutAnimationName;

    private AnimatorStateInfo m_StateInfo;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void InInit()
    {
        m_Animator.Play(m_InAnimationName);
    }

    protected override void InProcess()
    {
        m_StateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (!m_StateInfo.IsName(m_InAnimationName))
        {
            ChangeState(TransitionState.OUT);
            SceneManager.LoadScene(SceneName);
        }
    }

    protected override void OutInit()
    {
        m_Animator.Play(m_OutAnimationName);
    }

    protected override void OutProcess()
    {
        m_StateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (!m_StateInfo.IsName(m_OutAnimationName))
        {
            ChangeState(TransitionState.FINISH);
            
        }
    }

    protected override void Finish()
    {
    }
}
