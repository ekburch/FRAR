using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRAR
{
    public class PanelOperationState : PanelBaseState
    {
        InputEvents m_inputEvents = default;
        bool m_tankToPumpOpened = default;
        bool m_singleLineOpened = default;
        int m_throttleLevel = default;
        int m_dischargePressure = default;

        public override void EnterState(PanelStateManager panelState)
        {
            Debug.Log("CURRENT STATE: OPERATION");
            m_throttleLevel = (int)(m_inputEvents?.InputValue);
        }
        public override void UpdateState(PanelStateManager panelState)
        {

        }
        public override void OnClick(PanelStateManager panelState)
        {

        }
    }
}
