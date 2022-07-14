using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRAR
{
    public class Default : BaseState
    {
        [SerializeField] private bool m_isPumpEngaged;

        public bool IsPumpEngaged { set => m_isPumpEngaged = value; }

        public Default(PumpActionSM pumpActionSM) : base("Default", pumpActionSM) { }

        [SerializeField] UIManager m_uiManager = default;
        [SerializeField] string m_InstructionsText = "";

        public override void EnterState()
        {
            base.EnterState();
            m_uiManager?.UpdateText(m_InstructionsText);
        }

        public override void UpdateState()
        {
            base.UpdateState();
            if (m_isPumpEngaged)
                StateMachine.ChangeState(((PumpActionSM)StateMachine).PumpState1);
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}
