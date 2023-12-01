using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneController : SceneControllerBase
{
    public static GameSceneController instance;
    
    
    [field: SerializeField, Tooltip("Only set this in the editor!")] 
    public List<string> gameSceneName{get; private set;} = new List<string>();

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

    public void LoadScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }

    
}
