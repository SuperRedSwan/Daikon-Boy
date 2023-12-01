using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameUIInputHandler : MonoBehaviour
{   


    [SerializeField] private GameObject UICanvasObject;

    private void Start() {
        InputReader.instance.OpenUIEvent += OpenUI;

    }

    private void OnDestroy(){
        InputReader.instance.OpenUIEvent -= OpenUI;
        
        
    }

    public void OpenUI(){
        UICanvasObject.SetActive(true);
        
    }

    public void CloseUI(){  
        UICanvasObject.SetActive(false);
        InputReader.instance.DisableUI();
    }



}
