using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRAR
{
    public class FailState : BaseState
    {
        public FailState(PumpActionSM pumpActionSM) : base("Fail", pumpActionSM) { }

        public override void EnterState()
        {
            base.EnterState();
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
}
