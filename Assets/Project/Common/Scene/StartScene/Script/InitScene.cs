using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InitScene : MonoBehaviour
{
    public string SceneName;
    public string DebugSceneName;

    // Start is called before the first frame update
    void Start()
    {
        if(GameMainSystem.Instance.m_Debug) SceneManager.LoadScene(DebugSceneName);
        else SceneManager.LoadScene(SceneName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
