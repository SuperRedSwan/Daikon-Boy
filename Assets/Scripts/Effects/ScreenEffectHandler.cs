using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class ScreenEffectHandler : MonoBehaviour
{
    public static ScreenEffectHandler instance;

    [Header("Fading")]

    [SerializeField] private Animator fadeAnimator;

    
    [Header("Camera Shake")]

    [SerializeField] private float globalShakeForce;

    private void Awake() {
        if(instance == null){
            instance = this;
        }
        else{
            DestroyImmediate(this);
            return;
        }

        DontDestroyOnLoad(this);
    }
    
    private void Start() {
        
    }

    public void FadeInFadeOut(float seconds){
        StartCoroutine(FIFO(seconds));
    }

        
    public void FadeIn() => fadeAnimator.CrossFadeInFixedTime("FadeIn", Time.deltaTime);

    public void FadeOut() => fadeAnimator.CrossFadeInFixedTime("FadeOut", Time.deltaTime);

    private IEnumerator FIFO(float seconds){

        //Debug.Log("Fade In");
        FadeIn();
        FadeOut();
        yield return new WaitForSeconds(seconds);
        FadeOut();
        //Debug.Log("Fade Out");
    }

    

    #region Camera

    public void CameraShake(CinemachineImpulseSource impulseSource, Vector3 direction){

        impulseSource.GenerateImpulseWithVelocity(direction * globalShakeForce);
        //impulseSource.GenerateImpulseWithForce(globalShakeForce);
        
    }

    #endregion
}
