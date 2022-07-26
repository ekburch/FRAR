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
        public static SimulationManager Instance = null;
        public Dictionary<string, int> panelComponents = new Dictionary<string, int>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError($"SimulationManager.Awake(): {Instance} already exists, destroying duplicate gameObject.");
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            DontDestroyOnLoad(gameObject);
        }

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
