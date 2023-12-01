using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialPrompt : MonoBehaviour
{   

    [SerializeField] List<GameObject> tutorialImages = new List<GameObject>(); // the image type for each image, xbox, keyboard, playstation.

    private void Start() {
        InputSystem.onDeviceChange += CheckInputType;
        CheckInputType();
    }
    private void OnEnable() {
        InputSystem.onDeviceChange += CheckInputType;
        CheckInputType();
    }

    private void OnDestroy() {
        InputSystem.onDeviceChange -= CheckInputType;
    }

    private void OnDisable() {
        InputSystem.onDeviceChange -= CheckInputType;
    }


    private void CheckInputType(InputDevice device, InputDeviceChange change) => CheckInputType();

    public void CheckInputType(){
        
        foreach (var device in InputSystem.devices)
        {
            if(device is Gamepad){

                string controllerName = device.displayName;

                if(controllerName.Contains("Xbox",System.StringComparison.OrdinalIgnoreCase)){
                    // Xbox Controller
                    DisplayTutorialImage(1);
                }

                if(controllerName.Contains("PlayStation",System.StringComparison.OrdinalIgnoreCase)){
                    // Playstation Controller
                    DisplayTutorialImage(2);
                    
                }
            }
            else{
                // Default to Keyboard
                DisplayTutorialImage(0);

            }
        }
    }

    // 0 is keyboard
    // 1 is xbox
    // 2 is playstation

    public void DisplayTutorialImage(int index = 0){ 
        
        foreach(GameObject image in tutorialImages){
            
            // Set all images inactive.
            image.SetActive(false);
            
        }

        // reset the right image to active.
        tutorialImages[index].SetActive(true);
    }
}
