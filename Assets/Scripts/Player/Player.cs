using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ForceController))]
public class Player : MonoBehaviour // The Player Controller
{
    public PlayerStateMachine stateMachine{get; private set;} = new PlayerStateMachine();
    [SerializeField] public DebugObject debugObject;

    #region State Compontents

    public PlayerIdleState idleState{get; private set;}
    public PlayerMovementState moveState{get; private set;}
    public PlayerJumpState jumpState{get; private set;}
    public PlayerDashState dashState{get; private set;}
    public PlayerWallSlideState wallSlideState{get; private set;}
    public PlayerWallJumpState wallJumpState{get; private set;}
    
    #endregion

    #region Core Components

    public Rigidbody2D rb{get; private set;}
    public ForceController forceController{get; private set;}
    public Animator animator{get; private set;}
    [SerializeField] GameObject debugSprite;

    #endregion

    [field: Header("Movement Info")]

    [field: SerializeField] public float moveSpeed{get; private set;} = 15f;

    public bool facingRight{get; private set;} = true; // needs to start off as true

    [field: Header("Jumping Info")]

    [field: SerializeField] public float initialJumpForce{get; private set;} = 15f;
    [field: SerializeField] public float variedJumpForce{get; private set;} = 1.5f;

    [field: SerializeField] public float ascendingGravity{get; private set;} = 3.5f;
    [field: SerializeField] public float descendingGravity{get; private set;} = 5f;


    [field: SerializeField] public LayerMask whatIsGround{get; private set;}
    [field: SerializeField] public Vector3 groundRaycastOffset{get; private set;} = Vector3.zero;
    [field: SerializeField] public float groundRaycastLength{get; private set;} = .35f;
    
    [field: Header("Dashing Info")]


    [field: SerializeField] public float dashSpeed{get; private set;} = 1f;
    [field: SerializeField] public float dashLength{get; private set;} = .3f;
    [field: SerializeField] public float dashWaitTime{get; private set;} = .5f;
    [field: SerializeField] public float dashBufferLength{get; private set;} = .1f;

    public float dashBufferCounter{get; set;} = Mathf.Infinity;


    [field: SerializeField] public bool hasDashed{get; set;}
    [field: SerializeField] public bool isDashing{get; set;}
    public bool allowedToDashAgain{get; set;} = true;



    [SerializeField] public bool canDash => dashBufferCounter > 0 && !hasDashed;

    [field: Header("Wall Jump Variables")]

    [field: SerializeField] public LayerMask wallLayer{get; private set;}
    
    [field: SerializeField] public float wallRaycastLength{get; private set;} = .75f;

    [field: SerializeField] public float wallSlidingSpeed{get; private set;} = 2f;

    [field: SerializeField] public Vector2 wallJumpDirection{get; private set;} = Vector2.one;

    [field: SerializeField] public float initialWallJumpForce{get; private set;} = 15f;
    [field: SerializeField] public float variedWallJumpForce{get; private set;} = 1.5f;

    public bool allowedToWallJump{get; set;} = true;
    public bool wallSliding{get; set;} = false;
    
    #region Camera

    [field: SerializeField] public CinemachineImpulseSource imp{get; private set;}

    #endregion

    //[field: SerializeField] public float nullifyXVelocitySeconds{get; private set;} = .15f;

    public bool wallJumping{get; set;} = false;
    
    [field: Header("Corner Correction Info")]

    [field: SerializeField] public LayerMask cornerCorrectLayers{get; private set;}

    [field: SerializeField] public float yVelocity{get; private set;}  
    [field: SerializeField] public float topRaycastLength{get; private set;}

    [field: SerializeField] public Vector3 innerRaycastOffset {get; private set;} = Vector3.zero;
    
    [field: SerializeField] public Vector3 edgeRaycastOffset {get; private set;} = Vector3.zero;

    private void Awake() {

        // GettingComponents
        rb = GetComponent<Rigidbody2D>();
        forceController = GetComponent<ForceController>();
        animator = GetComponentInChildren<Animator>();
        imp = GetComponentInChildren<CinemachineImpulseSource>();
        // State initialization, has to happen after the state machine is initialized!
        idleState = new PlayerIdleState(stateMachine, this, stateMachine.GetAnimatorHash("Idle"), "Idle");
        moveState = new PlayerMovementState(stateMachine, this, stateMachine.GetAnimatorHash("Move"), "Move");
        jumpState = new PlayerJumpState(stateMachine, this, stateMachine.GetAnimatorHash("Jump"), "Jump");
        dashState = new PlayerDashState(stateMachine, this, stateMachine.GetAnimatorHash("Dash"), "Dash");
        wallJumpState = new PlayerWallJumpState(stateMachine, this, stateMachine.GetAnimatorHash("Jump"), "Wall Jump");
        wallSlideState = new PlayerWallSlideState(stateMachine, this, stateMachine.GetAnimatorHash("Slide"), "Wall Slide" );

        stateMachine.InitializeState(idleState);
    }

    private void Start() {
        
        InputReader.instance.JumpEvent += InputReader_JumpEvent;
        InputReader.instance.DashEvent += InputReader_DashEvent;
        debugSprite?.SetActive(false);
        StartCoroutine(StopAudio(1));
    }
    
    private IEnumerator StopAudio(float seconds){
        yield return new WaitForSeconds(seconds);
        AudioManager.instance.StopSubAudioSource();
    }

    private void Update() { // Handle input here & as part of events
        stateMachine.currentState?.Update(Time.deltaTime); // Never delete this it controls the states' updates

        
        
        // this will cause problems with wall jumping, putting in the wall jumping may also fix it!
        
        // go to move or idle state
        if(InputReader.instance.moveDirection != 0 && forceController.CheckIsGrounded() && !InputReader.instance.isHoldingJumpButton && !isDashing){
            
            stateMachine.SwitchState(moveState);
        }
        else if(InputReader.instance.moveDirection == 0 && forceController.CheckIsGrounded() && !InputReader.instance.isHoldingJumpButton && !isDashing){
            stateMachine.SwitchState(idleState);
        }
        

        // variable jump
        if(InputReader.instance.isHoldingJumpButton && forceController.CheckIsGrounded()){
            stateMachine.SwitchState(jumpState); // variable jump 
        }
        else if(InputReader.instance.isHoldingJumpButton && !forceController.CheckIsGrounded() && forceController.CheckOnAnyWall(wallRaycastLength,transform.position, wallLayer)){
            if(allowedToWallJump) stateMachine.SwitchState(wallJumpState); // variable jump 
            return;
        }

        if(!forceController.CheckIsGrounded() && !forceController.CheckOnAnyWall(wallRaycastLength, transform.position, wallLayer)){
            animator.SetBool("Falling", true);
        }
        else{
            animator.SetBool("Falling", false);
        }

        if(!forceController.CheckIsGrounded() && forceController.CheckOnAnyWall(wallRaycastLength, transform.position, wallLayer) && !wallJumping){

            if(!forceController.CheckIsGrounded() ){
                stateMachine.SwitchState(wallSlideState);
            }
            
        }
        
        
    }

    public void HandleDeath(){
        Debug.Log("ReloadScene");
        AudioManager.instance.PlayAudio(AudioManager.instance.audioClips[4].audioClip); // UI Select

        GameSceneController.instance.ReloadCurrentScene();
        ScreenEffectHandler.instance.FadeInFadeOut(.2f);
        InputReader.instance.SetLockedControls(true);
    }

    private void FixedUpdate() {
        stateMachine.currentState?.FixedUpdate(); // Never delete this it controls the states' fixed updates

        // quick fix to infinite dashing problem
        if(forceController.CheckIsGrounded() || forceController.CheckOnAnyWall(wallRaycastLength,transform.position, wallLayer)){

            allowedToDashAgain = true;
        } 

        if(!forceController.CheckIsGrounded() && !forceController.CheckOnAnyWall(wallRaycastLength,transform.position, wallLayer)){
            
             forceController.FallControl(descendingGravity, ascendingGravity);
        }


    }

    public void HandleCornerCorrect(){
        
        if(!forceController.CheckOnAnyWall(wallRaycastLength, transform.position, wallLayer) && forceController.CheckCanCornerCorrect(edgeRaycastOffset, innerRaycastOffset, topRaycastLength, transform.position, cornerCorrectLayers)){
            
            forceController.CornerCorrect(yVelocity, edgeRaycastOffset, innerRaycastOffset, topRaycastLength, transform.position, cornerCorrectLayers);
            StartCoroutine(ToggleAllowedToWallJump(.5f));
        }
    }

    public void SetFacingRight(bool _facingRight) => facingRight = _facingRight;

    public IEnumerator ToggleAllowedToWallJump(float seconds){

        allowedToWallJump = false;
        yield return new WaitForSeconds(seconds);
        allowedToWallJump = true;

    }

    private void InputReader_JumpEvent(){
        
        if(forceController.CheckOnAnyWall(wallRaycastLength, transform.position, wallLayer) && !forceController.CheckIsGrounded()){

            if(allowedToWallJump) stateMachine.SwitchState(wallJumpState);
            return;
        }
        else{
            stateMachine.SwitchState(jumpState);
            return;
        }
    }

    private void InputReader_DashEvent(){

        
        if(allowedToDashAgain) {stateMachine.SwitchState(dashState);}
    }
}
