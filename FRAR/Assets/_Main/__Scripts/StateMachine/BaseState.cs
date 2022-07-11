using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    public string m_name;
    protected StateMachine StateMachine;

    public BaseState(string name, StateMachine stateMachine)
    {
        this.m_name = name;
        this.StateMachine = stateMachine;
    }

    public virtual void EnterState() { }
    public virtual void UpdateState() { }
    public virtual void ExitState() { }
}
