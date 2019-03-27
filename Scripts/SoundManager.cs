using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {

    public AudioMixer audioMixer;
    public MenuUI menuUI;


    public void OnMusicSlider(float value)
    {
        audioMixer.SetFloat("musicVol", Mathf.Log(value) * 20);
        if (menuUI.musicOff && MenuUI.muteIsPressed)
        {
            MenuUI.muteIsPressed = false;
            menuUI.musicOff = false;
            menuUI.muteMusicBtn.image.sprite = menuUI.muteBtnNormal;
        }
    }

    public void OnSoundSlider(float value)
    {
        audioMixer.SetFloat("soundVol", Mathf.Log(value) * 20);
    }
}
