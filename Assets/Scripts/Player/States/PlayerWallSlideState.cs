using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(StateMachine _stateMachine, Player _player, int _animatorStringHash, string _name = "No Defined Name") : base(_stateMachine, _player, _animatorStringHash, _name)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.wallSliding = true;

    }

    public override void Update(float _deltaTime)
    {
        base.Update(_deltaTime);

        player.forceController.AllowMovement();

        if(player.forceController.CheckIsGrounded()){

                        

            player.stateMachine.SwitchState(player.moveState);
            return;
        }

        if(!player.forceController.CheckOnAnyWall(player.wallRaycastLength, player.transform.position, player.wallLayer)){

            
            
        
            player.stateMachine.SwitchState(player.moveState);
            
            return;
        }

        if(!player.wallJumping) player.forceController.WallSlide(player.wallSlidingSpeed);

        
    }

    public override void Exit()
    {
        base.Exit();
        player.wallSliding = false;

    }
}
