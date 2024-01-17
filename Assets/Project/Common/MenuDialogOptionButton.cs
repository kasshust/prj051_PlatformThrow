using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MenuDialogOptionButton : MonoBehaviour
{


    public void SetCursorDefault()
    {
        GameMainSystem.Instance.SetActiveCursorType(CursorManager.CursorType.Check);
    }

    public void SetCursorCheck() {
        GameMainSystem.Instance.SetActiveCursorType(CursorManager.CursorType.Check);
    }

    public void playSE(AudioClip audioClip) {
        // SEManager.Instance.Play(audioClip);
    }

    public void PlayFmodSound(FMODUnity.EventReference sound) {
        FMODUnity.RuntimeManager.PlayOneShot(sound, transform.position);
    }
}
