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

        [SerializeField]
        private string _inputName;
        [SerializeField]
        private int _inputValue;

        public string InputName => _inputName;
        public int InputValue => _inputValue;

        public void OnFocusEnter(FocusEventData eventData)
        {
            OnInputEventTriggered?.Invoke(this);
        }

        public void OnFocusExit(FocusEventData eventData)
        {
            return;
        }
    }
}
