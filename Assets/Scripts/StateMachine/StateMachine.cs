using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{

    public State currentState {get; private set;}

    public void InitializeState(State _newState) => SwitchState(_newState);

    public void SwitchState(State _newState){

        currentState?.Exit();
        currentState = _newState;
        _newState?.Enter();

        if(currentState == null){
            throw new System.Exception("No Current State!");
        }

    }

    public virtual void Update() {
        
    }

    public virtual void FixedUpdate(){
        
    }

    
}
