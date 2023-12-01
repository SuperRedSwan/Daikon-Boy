using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{   
    
   

    public PlayerJumpState(StateMachine _stateMachine, Player _player, int _animatorStringHash, string _name = "No Defined Name") : base(_stateMachine, _player, _animatorStringHash, _name)
    {
    }

    public override void Enter()
    {
        base.Enter();
        

        if(!AudioManager.instance.GetSFXIsPlaying()) { AudioManager.instance.PlayAudio(AudioManager.instance.audioClips[3].audioClip); }


        player.allowedToWallJump = false;
        
        if(player.forceController.CheckIsGrounded()) {player.forceController.InitialJump(); }
        
        
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        player.HandleCornerCorrect();

        player.forceController.AllowMovement();
        
        if(InputReader.instance.isHoldingJumpButton){

            //Debug.Log("Holding Jump");
            
            player.forceController.VariableJump(Vector2.up, player.variedJumpForce, false);
            
        }
        else{

            //Debug.Log(" Not Holding Jump");
            
            
            stateMachine.SwitchState(player.idleState);
            return;
            

                
                
            
            
        }
       
        
    }

    public override void Exit()
    {
        base.Exit();
        player.allowedToWallJump = true;
    }
}
