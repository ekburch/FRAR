using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRAR
{
    public class PumpStep_1 : BaseState
    {
        private PumpActionSM _pSM;

        [SerializeField] private bool m_isTankToPumpOpen;
        [SerializeField] private bool m_isCavitating;

        public bool IsTankToPumpOpen { set => m_isTankToPumpOpen = value; }
        public bool IsCavitating { set => m_isCavitating = value; }

        public PumpStep_1(PumpActionSM pumpActionSM) : base("PumpStep1", pumpActionSM) => _pSM = pumpActionSM;

        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("CURRENT STATE: STEP 1");
        }

        public override void UpdateState()
        {
            base.UpdateState();
            if (!m_isCavitating)
            {
                if (m_isTankToPumpOpen)
                    StateMachine.ChangeState(((PumpActionSM)StateMachine).PumpState2);
            }
            else
            {
                StateMachine.ChangeState(((PumpActionSM)StateMachine).CavitationState);
            }
        }

        public override void ExitState()
        {
            base.ExitState();
            // Cue AudioManager to play pump sounds
            // Turn on lights on panel
        }
    }
}
