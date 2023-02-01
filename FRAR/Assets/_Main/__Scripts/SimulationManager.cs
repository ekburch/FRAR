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
        public NeedleController[] needleControllers;
        [Tooltip("Temporary - will remove")]
        public NeedleController tachometer;
        public bool m_engineActive = false;

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
                //Debug.Log(entry.Key + ", " + entry.Value);
            }
        }
        private void Update()
        {
            int amount = m_engineActive ? 1 : 2;
			SimpleEngineComponentBehaviors(amount);
            if (!m_engineActive) tachometer.HandleUserInput(amount);
		}

        public void OnEngineButtonPress()
        {
            var soundManager = SoundManager.Instance;

            if (!m_engineActive)
            {
                m_engineActive = true;
				if (soundManager != null) soundManager.StartCoroutine("PlayEngineSounds");
            }
            else
            {
                m_engineActive = false;
				if (soundManager != null) soundManager.StopEngineSounds();
            }
               //m_engineActive = !m_engineActive;

            foreach (GameObject go in visualComponentsGO)
            {
                go.SetActive(!go.activeSelf);
            }
        }

        private void SimpleEngineComponentBehaviors(int value)
        {
            foreach(var needleController in needleControllers)
            {
                needleController.HandleUserInput(value);
            }
        }
    }
}
