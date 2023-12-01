using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExiter : MonoBehaviour
{   
    [SerializeField] private int nextLevelIndex = 0;
    [SerializeField] private float duration = .1f;
    [SerializeField] bool isLastLevel = false;

    private void OnTriggerEnter2D(Collider2D other) {
        
        if(!other.GetComponent<Player>()){return;}

        Transition();
        
        
    }

    private void Transition(){
        ScreenEffectHandler.instance.FadeInFadeOut(duration);
        StartCoroutine(LockControlsFor(duration*10f));
        AudioManager.instance.PlayAudio(AudioManager.instance.audioClips[6].audioClip); // UI Select
        
        if(isLastLevel){
            GameSceneController.instance.LoadSceneZero();
        }
        else{
            GameSceneController.instance.LoadScene(GameSceneController.instance.gameSceneName[nextLevelIndex]);
        }

    }

    private IEnumerator LockControlsFor(float seconds){

        InputReader.instance.SetLockedControls(true);
        yield return new WaitForSeconds(seconds);
        InputReader.instance.SetLockedControls(false);


    }
}
