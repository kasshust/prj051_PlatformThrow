using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fungus;


[CommandInfo("Audio",
              "Play Fmod Sound",
              "Play Fmod Sound")]
[AddComponentMenu("")]
public class PlayFmodSound : Command
{
    [SerializeField] private FMODUnity.EventReference m_FmodSound;

    public override void OnEnter()
    {
        FMODUnity.RuntimeManager.PlayOneShot(m_FmodSound, transform.position);
        Continue();
    }
    public override string GetSummary()
    {
        if (m_FmodSound.IsNull)
        {
            return "Error: No sound selected";
        }
        return m_FmodSound.ToString();
    }
}