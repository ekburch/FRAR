using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Microsoft.MixedReality.Toolkit.UI;

namespace FRAR
{
    public class HintsController : MonoBehaviour
    {
        public delegate void HintsControllerDelegate();

        UnityEvent toggleEvent;

        [SerializeField] private GameObject[] panelComponents = default;
        [SerializeField] private GameObject descriptionPanel = default;

        private bool isLabelsEnabled;
        public bool IsLabelsEnabled { set => isLabelsEnabled = value; }

        private void Start()
        {
            if (toggleEvent == null)
                toggleEvent = new UnityEvent();
            toggleEvent.AddListener(LabelToggle);
            //descriptionPanel = ObjectPool.SharedInstance.GetPooledObject();
            Toggle();
            DescriptionToggle();
        }

        public void LabelToggle()
        {
            if (isLabelsEnabled)
            {
                OnToggleHints?.Invoke();
            }
            else
            {
                Toggle();
            }
        }

        public void Toggle()
        {
            foreach (var components in panelComponents)
            {
                components.GetComponent<ToolTipSpawner>().enabled = !components.GetComponent<ToolTipSpawner>().enabled;
            }
        }

        public void DescriptionToggle()
        {
            if (isLabelsEnabled)
                OnToggleHints?.Invoke();
            else
            {
                //descriptionPanel.GetComponent<DescriptionsController>().ShowDescriptions = !descriptionPanel.GetComponent<DescriptionsController>().ShowDescriptions;
                descriptionPanel.SetActive(!descriptionPanel.activeSelf);
            }
        }

        public event HintsControllerDelegate OnToggleHints;
    }
}
