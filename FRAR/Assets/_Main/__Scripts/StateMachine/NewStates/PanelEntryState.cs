using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRAR
{
    public class PanelEntryState : PanelBaseState
    {
        bool m_isPumpEngaged;
        public bool IsPumpEngaged { set => m_isPumpEngaged = value; }

        public override void EnterState(PanelStateManager panelState)
        {
            Debug.Log("CURRENT STATE: ENTRY");
        }

        public override void UpdateState(PanelStateManager panelState)
        {
            if (m_isPumpEngaged)
                panelState.ChangeState(panelState.OperationState);
        }

        public override void OnClick(PanelStateManager panelState)
        {

        }
    }
}
