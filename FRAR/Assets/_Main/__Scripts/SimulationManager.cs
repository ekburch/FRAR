using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SceneSystem;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace FRAR
{
    public class SimulationManager : MonoBehaviour
    {
        public Dictionary<string, int> panelComponents = new Dictionary<string, int>();
        
        private void Start()
        {
            panelComponents.Clear();
            InputEvents.OnInputEventTriggered += OnPanelEventsTriggered;
        }

        private void OnPanelEventsTriggered(InputEvents inputEvent)
        {
            panelComponents[inputEvent.InputName] = inputEvent.InputValue;

            foreach(KeyValuePair<string, int> entry in panelComponents)
            {
                Debug.Log(entry.Key + ", " + entry.Value);
            }
        }
    }
}
