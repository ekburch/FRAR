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
        public Action<FocusEventData> OnFocusExited;

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

        [SerializeField]
        private Outline _outline;
        public Outline Outline => _outline;

        public void Start()
        {
            _outline = GetComponent<Outline>();
            _outline.enabled = false;
        }

        public void OnFocusEnter(FocusEventData eventData)
        {
            OnInputEventTriggered?.Invoke(this);
            descriptionsController?.UpdateText(InputName, InputDescription, transform, true);
            OnFocusEntered?.Invoke(eventData);
            Toggle();
            //hintsController.DescriptionToggle();
        }

        public void OnFocusExit(FocusEventData eventData)
        {
            //StartCoroutine(descriptionsController.ResetText(TimeUntilDialogResets));
            descriptionsController?.UpdateText("", "", null, false);
            OnFocusExited?.Invoke(eventData);
            Toggle();
            //hintsController.DescriptionToggle();
        }

        public void Toggle()
        {
            _outline.enabled = !_outline.enabled;
        }
    }
}
