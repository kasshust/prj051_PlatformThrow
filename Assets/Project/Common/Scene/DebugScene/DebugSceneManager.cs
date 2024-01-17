using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSceneManager : MonoBehaviour
{
    public StoryPhase       m_DebugStoryPhase;
    public StoryStage       m_DebugStoryStage;
    public StorySubStage    m_DebugStorySubStage;

    public void LoadScene(string SceneName)
    {
        TransitionManager.Instance.changeScene(SceneName, 0);
    }
}
