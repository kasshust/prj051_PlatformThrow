using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[RequireComponent(typeof(WriterAudio))]
public class FungusVolumeController : MonoBehaviour
{
    WriterAudio wa;
    private void Awake()
    {
        wa = GetComponent<WriterAudio>();
    }
    void Update()
    {
        GameMainSystem.SoundVolume sv = GameMainSystem.Instance.GetSoundVolume();
        wa.volume = sv.GlobalSEVolume;
    }
}
