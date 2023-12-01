using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementState : PlayerState
{
    public PlayerMovementState(StateMachine _stateMachine, Player _player, int _animatorStringHash, string _name = "No Defined Name") : base(_stateMachine, _player, _animatorStringHash, _name)
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
        player.forceController.AllowMovement();

        
        
        

        

    }

    public override void Exit()
    {
        base.Exit();
    }
}
