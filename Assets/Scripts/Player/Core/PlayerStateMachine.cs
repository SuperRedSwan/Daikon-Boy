using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public PlayerStateMachine()
    {
    }

    public int GetAnimatorHash(string _name) => Animator.StringToHash(_name);
}
