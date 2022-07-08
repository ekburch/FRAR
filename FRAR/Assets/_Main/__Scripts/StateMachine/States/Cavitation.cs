using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cavitation : BaseState
{
    private float _Input;

    public Cavitation(PumpActionSM pumpActionSM) : base("Cavitation", pumpActionSM) { }

    public override void EnterState()
    {
        base.EnterState();
        _Input = 0f;
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
