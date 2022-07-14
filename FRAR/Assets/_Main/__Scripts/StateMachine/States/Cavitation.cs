using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cavitation : BaseState
{
    private float _Input;

    public Cavitation(PumpActionSM pumpActionSM) : base("Cavitation", pumpActionSM) { }

    [SerializeField] UIManager m_uiManager = default;
    [SerializeField] string m_InstructionsText = "";

    public override void EnterState()
    {
        base.EnterState();
        _Input = 0f;
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
