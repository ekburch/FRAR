using UnityEngine;

namespace FRAR
{
    public abstract class PanelBaseState
    {
        public abstract void EnterState(PanelStateManager panelState);
        public abstract void UpdateState(PanelStateManager panelState);
        public abstract void OnClick(PanelStateManager panelState);
    }
}
