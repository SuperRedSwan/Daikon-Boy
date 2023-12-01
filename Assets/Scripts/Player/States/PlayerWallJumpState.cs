using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{

    int direction;
    
    

    public PlayerWallJumpState(StateMachine _stateMachine, Player _player, int _animatorStringHash, string _name = "No Defined Name") : base(_stateMachine, _player, _animatorStringHash, _name)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if(!AudioManager.instance.GetSFXIsPlaying()) { AudioManager.instance.PlayAudio(AudioManager.instance.audioClips[3].audioClip); }// UI Select

        player.wallJumping = true;

        direction = player.forceController.CheckOnRightWall(player.wallRaycastLength, player.transform.position, player.wallLayer) ? -1 : 1;
       
        player.forceController.Jump(new Vector2(player.wallJumpDirection.x*direction, player.wallJumpDirection.y).normalized, player.initialWallJumpForce, true);
        
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        
        

        if(InputReader.instance.isHoldingJumpButton){

            //Debug.Log("Holding Jump");
            
            if(Mathf.Sign(InputReader.instance.moveDirection) == -direction){

                player.StartCoroutine(SwitchAfterMoveInput(.1f));
                return;
            }

            
            player.forceController.VariableJump(new Vector2((player.wallJumpDirection.x*direction), player.wallJumpDirection.y).normalized, player.variedWallJumpForce, false);
            
        }
        else{

            //Debug.Log(" Not Holding Jump");
            
            stateMachine.SwitchState(player.idleState);
            return;
            
        }

    }

     
    private IEnumerator SwitchAfterMoveInput(float seconds){

        yield return new WaitForSeconds(seconds);
        InputReader.instance.SetIsHoldingJumpButton();

        stateMachine.SwitchState(player.idleState);
        
    }
    public override void Exit()
    {
        base.Exit();
        

        player.wallJumping = false;

    }
}
