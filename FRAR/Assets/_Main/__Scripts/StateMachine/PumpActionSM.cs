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
    }
}
