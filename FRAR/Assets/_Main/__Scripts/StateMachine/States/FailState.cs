using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailState : BaseState
{
    public FailState(PumpActionSM pumpActionSM) : base("Fail", pumpActionSM) { }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
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
