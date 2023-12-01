using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(StateMachine _stateMachine, Player _player, int _animatorStringHash, string _name = "No Defined Name") : base(_stateMachine, _player, _animatorStringHash, _name)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update(float _deltaTime)
    {
        base.Update(_deltaTime);
        
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.forceController.AllowMovement(false);
    
        if(!player.forceController.CheckIsGrounded() && player.forceController.CheckOnAnyWall(player.wallRaycastLength,player.transform.position, player.wallLayer)){
            player.stateMachine.SwitchState(player.wallSlideState);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
