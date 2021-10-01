using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

namespace FRAR
{
    public class InputEvents : MonoBehaviour, IMixedRealityFocusHandler
    {
        public static event Action<InputEvents> OnInputEventTriggered;

        public GameObject displayPanel;

        [SerializeField]
        private string _inputName;
        [SerializeField]
        private string _inputDescription;
        [SerializeField]
        private int _inputValue;

        public string InputName => _inputName;
        public string InputDescription => _inputDescription;
        public int InputValue => _inputValue;

        public void OnFocusEnter(FocusEventData eventData)
        {
            OnInputEventTriggered?.Invoke(this);
            displayPanel.GetComponent<DescriptionsController>().UpdateText(InputName, InputDescription, transform);
        }

        public void OnFocusExit(FocusEventData eventData)
        {
            if (displayPanel != null) displayPanel.GetComponent<DescriptionsController>().ResetText();
        }
    }
}
