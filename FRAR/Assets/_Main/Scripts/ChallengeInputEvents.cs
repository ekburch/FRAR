#define USING_GAZE_CONTROLS

using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRAR
{
    public class ChallengeInputEvents : MonoBehaviour
    {
        [SerializeField] InputEvents m_inputEvents = default;
        [SerializeField] ChallengeManager m_challengeManager = default;

        private void Awake()
        {
            m_inputEvents = GetComponent<InputEvents>();
        }

        void ToValidate(FocusEventData eventData)
        {
            m_challengeManager?.CheckAnswer(eventData.NewFocusedObject.name);
        }

        private void OnEnable()
        {
            m_inputEvents.OnFocusEntered += i => ToValidate(i);
        }

        private void OnDisable()
        {
            m_inputEvents.OnFocusEntered -= i => ToValidate(i);
        }
    }
}
