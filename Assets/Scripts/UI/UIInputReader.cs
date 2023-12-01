using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputReader : MonoBehaviour
{
    public static UIInputReader instance;

    private InputAction navigationAction;
    public Vector2 navigationInput{get; private set;}

    public static PlayerInput playerInput{get; set;}

    private void Awake() {
        if(instance == null){
            instance = this;
        }

        playerInput = GetComponent<PlayerInput>();
        navigationAction = playerInput.actions["Navigate"]; 
    }

    private void Update() {
        navigationInput = navigationAction.ReadValue<Vector2>();
    }
}
