using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioHandler : MonoBehaviour
{

    public void PlaySelectSound(){

        AudioManager.instance?.PlayAudio(AudioManager.instance?.audioClips[0].audioClip); // UI Select
    }
}
