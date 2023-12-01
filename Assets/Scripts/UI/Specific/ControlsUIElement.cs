using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlsUIElement : MonoBehaviour
{

    [SerializeField] public List<ControlSchemeElement> controlSchemes = new List<ControlSchemeElement>();
    [SerializeField] public TextMeshProUGUI controlSchemeDisplay;
    

    private int currentIndex = 0;


    private void Start() {
        controlSchemeDisplay.text = controlSchemes[currentIndex].text;
        controlSchemes[currentIndex].gameObject.SetActive(true);
    }

    public void NextScheme(){

        controlSchemes[currentIndex].gameObject.SetActive(false);

        currentIndex++;

        if(currentIndex > controlSchemes.Count - 1){
            currentIndex = 0;
            
        }

        controlSchemes[currentIndex].gameObject.SetActive(true);

        controlSchemeDisplay.text = controlSchemes[currentIndex].text;
    
    }

    public void PreviousScheme(){
        
        controlSchemes[currentIndex].gameObject.SetActive(false);

        currentIndex--;

        if(currentIndex < 0){
            currentIndex = controlSchemes.Count - 1;
            
        }
        
        controlSchemes[currentIndex].gameObject.SetActive(true);

        controlSchemeDisplay.text = controlSchemes[currentIndex].text;

        
    }

    [Serializable]
    public struct ControlSchemeElement{
        public GameObject gameObject;
        public string text;
    }
}
