using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpActionSM : StateMachine
{
    [HideInInspector]
    public Default DefaultState;
    [HideInInspector]
    public PumpStep_1 PumpState1;
    [HideInInspector]
    public PumpStep_2 PumpState2;
    [HideInInspector]
    public PumpStep_3 PumpState3;
    [HideInInspector]
    public PumpStep_4 PumpState4;
    [HideInInspector]
    public PumpStep_5 PumpState5;
    [HideInInspector]
    public Cavitation CavitationState;

    private void Awake()
    {
        DefaultState = new Default(this);
        PumpState1 = new PumpStep_1(this);
        PumpState2 = new PumpStep_2(this);
        PumpState3 = new PumpStep_3(this);
        PumpState4 = new PumpStep_4(this);
        PumpState5 = new PumpStep_5(this);
        CavitationState = new Cavitation(this);
    }
    protected override BaseState GetInitialState()
    {
        return DefaultState;
    }
}
