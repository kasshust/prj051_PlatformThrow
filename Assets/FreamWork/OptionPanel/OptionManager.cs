using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionManager : MonoBehaviour
{
    

    public Slider MusicVolumeSlider;
    public Slider SEVolumeSlider;
    public Dropdown ScreenSizeSelect;

    // Start is called before the first frame update
    void Start()
    {
        GameMainSystem.SoundVolume soundVolume = GameMainSystem.Instance.GetSoundVolume();

        MusicVolumeSlider.value = soundVolume.GlobalMusicVolume * 100.0f;
        SEVolumeSlider.value    = soundVolume.GlobalSEVolume * 100.0f;
    }

    public void setMusicVolume() {
        GameMainSystem.Instance.SetMusicVolume(MusicVolumeSlider.value/100.0f);

    }
    public void setSEVolume() {
        GameMainSystem.Instance.SetSEVolume(SEVolumeSlider.value / 100.0f);
    }

    public void setWindowSize() {
        switch (ScreenSizeSelect.value) {
            case 0:
                Screen.SetResolution(320, 240, false, 60);
                break;
            case 1:
                Screen.SetResolution(640, 480, false, 60);
                break;
            case 2:
                Screen.SetResolution(1280, 960, false, 60);
                break;
            default:

                break;
        }
        
    }
}
