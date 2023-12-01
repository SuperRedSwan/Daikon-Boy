using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource mainAudioSource, subAudioSource; // main audio source for main music, sub audio for sfx
    [field: SerializeField] public List<AudioManagerClip> audioClips{get; private set;} = new List<AudioManagerClip>();

    public float sfxVolume{get; private set;} = .5f;
    public float mainVolume{get; private set;} = .5f;


    private void Awake() {
        ManageSingleton();    
    }
    
    private void Start() {
        sfxVolume = subAudioSource.volume;
        mainVolume = mainAudioSource.volume;
    }

    private void ManageSingleton()
    {
       
        if(instance != null){
            gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
        else{
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void PlayAudio(AudioManagerClip audioClip){
        subAudioSource.volume = sfxVolume;
        mainAudioSource.volume = mainVolume;

        subAudioSource.PlayOneShot(audioClip.audioClip);
    }

    public bool GetSFXIsPlaying() => subAudioSource.isPlaying;

    public void StopSubAudioSource() => subAudioSource.Stop();
        
    public void PlayAudio(AudioClip audioClip){
        subAudioSource.volume = sfxVolume;
        mainAudioSource.volume = mainVolume;

        subAudioSource.PlayOneShot(audioClip);
    }

    public void SetSubAudioVolume(float volume){
        subAudioSource.volume = volume;
        sfxVolume = volume;
    }

    public void SetMainAudioVolume(float volume){
        mainAudioSource.volume = volume;
        mainVolume = volume;
    }

    [System.Serializable]
    public class AudioManagerClip{
        
        public string name;
        public AudioClip audioClip;

        public AudioManagerClip(string name, int index, AudioClip audioClip){
            this.name = name;
            this.audioClip = audioClip;
        }
    }
}
