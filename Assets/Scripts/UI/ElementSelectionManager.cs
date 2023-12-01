using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ElementSelectionManager : MonoBehaviour
{

    public static ElementSelectionManager instance;

    [field: SerializeField] public GameObject[] UIElements{get; private set;}

    [field: SerializeField] public  GameObject lastSelectedObject{get; set;}
    public int lastSelectedIndex{get;  set;}

    private void Awake() {
        if(instance == null){
            instance = this;
        }
    }

    private void Start() {
        StartCoroutine(InitializeWaitOneFrame());    
    }

    private void OnEnable() {

        StartCoroutine(InitializeWaitOneFrame());
    }

    private void Update() {
        
        // If we move down
        // Next selection in array

        if(UIInputReader.instance.navigationInput.y < 0){
            HandleNextElementSelection(1);
        }

        // If we move up
        // Previous Selection in the array
    
        if(UIInputReader.instance.navigationInput.y > 0){
            HandleNextElementSelection(-1);
        }

        // If we move left
        // Previous Selection in the array

        if(UIInputReader.instance.navigationInput.x > 0){
            HandleNextElementSelection(-1);
        }

        // If we move Right
        // Previous Selection in the array

        if(UIInputReader.instance.navigationInput.x > 0){
            HandleNextElementSelection(1);
        }

        
    }

    private void HandleNextElementSelection(int addition){

        if(EventSystem.current.currentSelectedGameObject == null && lastSelectedObject != null){

            int newIndex = lastSelectedIndex + addition;
            newIndex = Mathf.Clamp(newIndex, 0, UIElements.Length - 1);


            EventSystem.current.SetSelectedGameObject(UIElements[newIndex]);
        }
    }

    private IEnumerator InitializeWaitOneFrame(int index = 0){

        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(UIElements[index]);
    }
}
