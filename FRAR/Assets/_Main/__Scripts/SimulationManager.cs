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
        public GameObject[] visualComponentsGO;

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
            //DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            panelComponents.Clear();
            InputEvents.OnInputEventTriggered += OnPanelEventsTriggered;
            foreach(var component in visualComponentsGO)
            {
                component.SetActive(false);
            }
        }

        private void OnPanelEventsTriggered(InputEvents inputEvent)
        {
            panelComponents[inputEvent.InputName] = inputEvent.InputValue;

            foreach(KeyValuePair<string, int> entry in panelComponents)
            {
                Debug.Log(entry.Key + ", " + entry.Value);
            }
        }

        public void SetStartingValues()
        {

        }

        public void OnEngineButtonPress()
        {
            SetStartingValues();
            var soundManager = SoundManager.Instance;
            if (soundManager != null)
                soundManager.IsPlayingScheduled = !soundManager.IsPlayingScheduled;

            foreach (GameObject go in visualComponentsGO)
            {
                go.SetActive(!go.activeSelf);
            }
        }
    }
}
