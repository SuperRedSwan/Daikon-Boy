using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputReader : MonoBehaviour, PlayerControls.IPlayerActionsActions // I hate that >:(
{

    public static InputReader instance;

    private PlayerControls controls; 

    //Actual Control values
    public bool controlsLocked{get; private set;}  = false;

    public float moveDirection{get; private set;} = 0;
    public Vector2 moveDirVector{get; private set;} = Vector2.zero;

    public bool isHoldingJumpButton{get; private set;} = false;

    public event Action JumpEvent, DashEvent, OpenUIEvent;

    public void SetLockedControls(bool setLocked) => controlsLocked = setLocked;

    public void SetIsHoldingJumpButton() => isHoldingJumpButton = false;
    
    
    //public GameObject UIInputReader;

    private void Awake() {

        
        instance = this;
        
        //UIInputReader = GetComponentInChildren<GameObject>();
        

        controls = new PlayerControls();
        controls.PlayerActions.SetCallbacks(this);
        controls.Enable();    
    }

    private void OnDestroy() {
        controls.Disable();
    }

    private void Start() {
        DisableUI();   
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if(controlsLocked){return;}

        if(!context.performed){return;}
        DashEvent?.Invoke();
        
    }

    public void OnJump(InputAction.CallbackContext context)
    {   

        if(controlsLocked){return;}

        if(context.interaction is TapInteraction){
            
            if(context.performed){
                isHoldingJumpButton = false;
                JumpEvent?.Invoke();
                return;
            }
        }
        else{


            if(context.started){
                isHoldingJumpButton = true;
            }
            if(context.performed || context.canceled){
                isHoldingJumpButton = false;
            }
        }


        

    

       
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {   
        
        if(controlsLocked){return;}
        if(context.performed){

            moveDirVector = context.ReadValue<Vector2>();
            moveDirection = moveDirVector.x;
        }

        if(context.canceled){
            moveDirVector = Vector2.zero;
            moveDirection = 0;
        }
        
        
        
        
        
    }

    public void OnOpenMenu(InputAction.CallbackContext context)
    {
        if(!context.performed){return;}
        // switch control scheme and open UI
        Debug.Log("Switch action map and open UI");


        
            
            OpenUIEvent?.Invoke();
            EnableUI();
            
        
    }

    public void DisableUI(){

        controls.PlayerActions.Enable();
        controls.UI.Disable();
    }

    public void EnableUI(){
        controls.PlayerActions.Disable();
        controls.UI.Enable();  
    }
}
