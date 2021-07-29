using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

namespace FRAR.UI
{
    public class HintsController : MonoBehaviour
    {
        public delegate void HintsControllerDelegate();

        [SerializeField] private GameObject[] panelComponents = default;
        [SerializeField] private GameObject descriptionPanel = default;

        private bool isLabelsEnabled;
        public bool IsLabelsEnabled { set => isLabelsEnabled = value; }
        
        public void LabelToggle()
        {
            if (isLabelsEnabled)
            {
                OnToggleHints?.Invoke();
            }
            else
            {
                foreach (var components in panelComponents)
                {
                    components.SetActive(!components.activeSelf);
                }

            }
        }

        public void DescriptionToggle()
        {
            if (isLabelsEnabled)
                OnToggleHints?.Invoke();
            else
                descriptionPanel.SetActive(!descriptionPanel.activeSelf);
        }

        public event HintsControllerDelegate OnToggleHints;
    }
}
