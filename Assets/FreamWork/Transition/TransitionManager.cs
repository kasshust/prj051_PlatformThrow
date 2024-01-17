using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : SingletonMonoBehaviourFast<TransitionManager>
{
    [SerializeField]
    private TransitionPattern tranitionPattern;

    private GameObject currentTransition;

    void Update() {
        if (currentTransition != null) {
            if (currentTransition.GetComponent<Transition>().isFinish())
            {
                destroyCurrentTransition();
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
        if (tranitionPattern.Count() == 0) Debug.LogError("トランジションが登録されてない");
    }

    public bool isPlayTransition()
    {
        return currentTransition != null;
    }

    // indexで指定　非推奨
    public void changeScene(string SceneName, int index = 0)
    {
        if (!isPlayTransition()){
            currentTransition = Instantiate(tranitionPattern.Get(index));
            Transition tr = currentTransition.GetComponent<Transition>();
            tr.setScene(SceneName);
        } else {
            Debug.Log("遷移中です : " + SceneName);
        }
    }

    // 名前で指定
    public void changeScene(string SceneName, string name)
    {
        if (!isPlayTransition())
        {
            currentTransition = Instantiate(tranitionPattern.Get(name));
            Transition tr = currentTransition.GetComponent<Transition>();
            tr.setScene(SceneName);
        }
        else
        {
            Debug.Log("既に遷移中です : " + SceneName);
        }
    }

    public void changeScene(string SceneName, GameObject transitionPrefab)
    {
        if (!isPlayTransition())
        {
            currentTransition = Instantiate(transitionPrefab);
            Transition tr = currentTransition.GetComponent<Transition>();
            tr.setScene(SceneName);

        }
        else
        {
            Debug.Log("既に遷移中です : " + SceneName);
        }
    }

    private void destroyCurrentTransition() {
        Destroy(currentTransition);
    }
}
