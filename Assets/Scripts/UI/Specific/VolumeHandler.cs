using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VolumeHandler : MonoBehaviour
{   
    [SerializeField] private TextMeshProUGUI volumeDisplay;
    [SerializeField] private bool isMainVolume = false;

    private void Start() {
        UpdateDisplay();
        
        
    }

    public void VolumeUpSFX(){


        if(AudioManager.instance.sfxVolume >= 1f){return;}

        AudioManager.instance.SetSubAudioVolume(AudioManager.instance.sfxVolume + .05f);

        UpdateDisplay();

    }

    public void VolumeDownSFX(){

        if(AudioManager.instance.sfxVolume <= 0f){return;}

        AudioManager.instance.SetSubAudioVolume(AudioManager.instance.sfxVolume - .05f);
        
        UpdateDisplay();
        
    }

    public void VolumeUpMain(){


        if(AudioManager.instance.mainVolume >= 1f){return;}

        AudioManager.instance.SetMainAudioVolume(AudioManager.instance.mainVolume + .05f);

        UpdateDisplay();
        
    }

    public void VolumeDownMain(){

        if(AudioManager.instance.mainVolume <= 0f){return;}

        AudioManager.instance.SetMainAudioVolume(AudioManager.instance.mainVolume - .05f);
        UpdateDisplay();
        
        
    }

    private void UpdateDisplay(){

        if(isMainVolume){
            volumeDisplay.text = Mathf.Round((AudioManager.instance.mainVolume * 100f))  + "%";
        
        }
        else{
           
            volumeDisplay.text = Mathf.Round((AudioManager.instance.sfxVolume * 100f))  + "%";
        }
    }
}
