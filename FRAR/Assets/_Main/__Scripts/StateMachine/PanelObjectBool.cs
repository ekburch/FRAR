using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRAR
{
    public class PanelObjectBool : PanelObject
    {
        private void Update()
        {
            PanelStateManager.Instance.worldState.Add(PanelItem, 1f);
        }
    }
}
