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
        public Action<FocusEventData> OnFocusEntered;

        public DescriptionsController descriptionsController;
        public HintsController hintsController;

        [SerializeField]
        private string _inputName;
        [SerializeField]
        private string _inputDescription;
        [SerializeField]
        private int _inputValue;
        [SerializeField]
        private float _timeUntilDialogResets = 2f;

        public string InputName => _inputName;
        public string InputDescription => _inputDescription;
        public int InputValue => _inputValue;
        public float TimeUntilDialogResets => _timeUntilDialogResets;

        public void OnFocusEnter(FocusEventData eventData)
        {
            OnInputEventTriggered?.Invoke(this);
            descriptionsController?.UpdateText(InputName, InputDescription, transform, true);
            OnFocusEntered?.Invoke(eventData);
            //hintsController.DescriptionToggle();
        }

        public void OnFocusExit(FocusEventData eventData)
        {
            //StartCoroutine(descriptionsController.ResetText(TimeUntilDialogResets));
            descriptionsController?.UpdateText("", "", null, false);
            //hintsController.DescriptionToggle();
            Debug.Log("OnFocusExit()");
        }
    }
}
