using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : State
{
    protected StateMachine stateMachine;
    protected Player player;
    protected string name;
    protected int animatorStringHash;

    public PlayerState(StateMachine _stateMachine, Player _player, int _animatorStringHash, string _name = "No Defined Name")
    {
        stateMachine = _stateMachine;
        player = _player;
        animatorStringHash = _animatorStringHash;
        name = _name; // for debug purposes!
    }

    public override void Enter()
    {
        base.Enter();
        player.animator.SetBool(animatorStringHash, true);
    }

    public override void Update(float _deltaTime)
    {
        base.Update(_deltaTime);
        
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player?.debugObject.Log(("Current State: " + name ), player.transform.position, player?.name);

    }

    public override void Exit()
    {
        base.Exit();
        player.animator.SetBool(animatorStringHash, false);
        

    }
}
