using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NativeLogMenuExtra : Fungus.NarrativeLogMenu
{
    [SerializeField] private MapCommonPanel m_Panel;
    [SerializeField] private FungusExtra.DialogInputExtra m_DialogInputExtra;

    override protected  void Awake()
    {
        if (showLog)
        {
            // Only one instance of NarrativeLogMenu may exist
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;

            // GameObject.DontDestroyOnLoad(this);

            clickAudioSource = GetComponent<AudioSource>();
        }
        else
        {
            GameObject logView = GameObject.Find("NarrativeLogView");
            // logView.SetActive(false);
            this.enabled = false;
        }

        narLogViewtextAdapter.InitFromGameObject(narrativeLogView.gameObject, true);
    }

    override protected void Start()
    {
        UpdateNarrativeLogText();
    }

    override public void ToggleNarrativeLogView()
    {

        if (narrativeLogActive)
        {
            m_Panel.Open();
            m_DialogInputExtra.ToggleIgnore(true);

        }
        else
        {
            m_Panel.Close();
            m_DialogInputExtra.ToggleIgnore(false);
        }

        narrativeLogActive = !narrativeLogActive;
    }
}