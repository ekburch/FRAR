using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Default : BaseState
{
    private float _Input;

    private bool m_isPumpEngaged;

    public bool IsPumpEngaged { set => m_isPumpEngaged = value; }

    public Default(PumpActionSM pumpActionSM) : base("Default", pumpActionSM) { }

    public override void EnterState()
    {
        base.EnterState();
        _Input = 0f;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (m_isPumpEngaged)
            StateMachine.ChangeState(((PumpActionSM) StateMachine).PumpState1);
    }

    public override void UpdateSimulation()
    {
        base.UpdateSimulation();
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
