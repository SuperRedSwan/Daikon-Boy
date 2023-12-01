using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControllerBase : MonoBehaviour
{
    public void LoadNextScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReloadCurrentScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadSceneZero(){
        SceneManager.LoadScene(0);
    }

    public void QuitGame(){
        Debug.Log("Quit Application");
        Application.Quit();
    }

    private void Start() {
        TimeTracker.instance?.CheckShouldTrack();
    }


}
