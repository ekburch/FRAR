using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpStep_2 : BaseState
{
    private PumpActionSM _pSM;

    private float _Input;
    private float m_throttleLevel;
    private float m_singleLineDischargeLevel;
    private float m_masterDischargeLevel;

    public float ThrottleLevel { set => m_throttleLevel = value; }
    public float SingleLineDischargeLevel { set => m_singleLineDischargeLevel = value; }
    public float MasterDischargeLevel { set => m_masterDischargeLevel = value; }

    private bool m_isCavitating;
    private bool m_isTankToPumpOpen;
    private bool m_isDischargeOpen;

    public bool IsCavitating { set => m_isCavitating = value; }
    public bool IsTankToPumpOpen { set => m_isTankToPumpOpen = value; }
    public bool IsDichargeOpen { set => m_isDischargeOpen = value; }

    public PumpStep_2(PumpActionSM pumpActionSM) : base("PumpStep2", pumpActionSM) 
    {
        _pSM = (PumpActionSM)pumpActionSM;
    }

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
