using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeTracker : MonoBehaviour
{

    public static TimeTracker instance;

    public bool shouldTrack = false;
    public float currentTime = 0;

    private void Awake() {
        ManageSingleton();
    }

    private void Start() {
        
    }

    private void Update() {
        
        if(shouldTrack) currentTime += Time.deltaTime;
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

    public void BeginTrackingTime() => shouldTrack = true;



    public void CheckShouldTrack(){
        
        if(SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings-1){
            shouldTrack = false;
        }
        else{
            if(SceneManager.GetActiveScene().buildIndex != 0){
                shouldTrack = true;
            }
            else{
                currentTime = 0;
                shouldTrack = false;
            }
        }
        
    }
}
