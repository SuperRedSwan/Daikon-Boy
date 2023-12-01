using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(StateMachine _stateMachine, Player _player, int _animatorStringHash, string _name = "No Defined Name") : base(_stateMachine, _player, _animatorStringHash, _name)
    {
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlayAudio(AudioManager.instance.audioClips[2].audioClip); // UI Select

        player.dashBufferCounter = player.dashBufferLength;
        ScreenEffectHandler.instance?.CameraShake(player.imp, InputReader.instance.moveDirVector);
        //player.StartCoroutine(player.forceController.ForceSwitchStateAfterSeconds(player.dashWaitTime));

    }

    public override void Update(float _deltaTime)
    {
        base.Update(_deltaTime);

        
    // pause and then allow dash after seconds, not related to actual variable.

        if(!player.isDashing) {player.forceController.AllowMovement();}
        
        if(player.canDash){

            player.StartCoroutine(player.forceController.Dash(InputReader.instance.moveDirVector, player.dashSpeed, player.dashBufferLength, player.dashWaitTime, _deltaTime));
            player.dashBufferCounter -= _deltaTime;
        }
        else{
            if(!player.isDashing) stateMachine.SwitchState(player.moveState); // strange to do it this way but meh
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.allowedToDashAgain = false;
        

    }
}
