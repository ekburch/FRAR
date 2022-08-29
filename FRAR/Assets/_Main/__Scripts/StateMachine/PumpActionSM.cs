using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRAR
{
    public class PumpActionSM : StateMachine
    {
        public Default DefaultState;
        public PumpStep_1 PumpState1;
        public PumpStep_2 PumpState2;
        public PumpStep_3 PumpState3;
        public PumpStep_4 PumpState4;
        public PumpStep_5 PumpState5;
        public Cavitation CavitationState;
        public FailState FailState;
        public SuccessState SuccessState;

        [SerializeField] private bool m_isPumpEngaged = default;
        [SerializeField] private bool m_isTankToPumpOpen = default;
        [SerializeField] private bool m_isCavitating = default;
        [SerializeField] private bool m_stoppedCavitation = default;

        public bool IsPumpEngaged { set => m_isPumpEngaged = value; }
        public bool IsTankToPumpOpen { set => m_isTankToPumpOpen = value; }
        public bool IsCavitating { set => m_isCavitating = value; }
        public bool StoppedCavitation { set => m_stoppedCavitation = value; }

        [SerializeField] int m_throttleLevel = default;
        [SerializeField] int m_dischargePressure = default;

        public int ThrottleLevel { set => m_throttleLevel = value; }
        public int DischargePressure { set => m_dischargePressure = value; }

        private void Awake()
        {
            DefaultState = new Default(this);
            PumpState1 = new PumpStep_1(this);
            PumpState2 = new PumpStep_2(this);
            PumpState3 = new PumpStep_3(this);
            PumpState4 = new PumpStep_4(this);
            PumpState5 = new PumpStep_5(this);
            CavitationState = new Cavitation(this);
            FailState = new FailState(this);
            SuccessState = new SuccessState(this);
        }

        public override void Start()
        {
            currentState = GetInitialState();
        }

        protected override BaseState GetInitialState()
        {
            return DefaultState;
        }

        public override void Update()
        {
            base.Update();
            if (m_isPumpEngaged)
            {
                CheckToChangeState();
            }
        }

        private void CheckToChangeState()
        {

        }
    }
}
