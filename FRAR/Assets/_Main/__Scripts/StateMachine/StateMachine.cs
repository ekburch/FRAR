using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRAR
{
    public class StateMachine : MonoBehaviour
    {
        public BaseState currentState;

        public SoundManager m_audioManager = default;
        public UIManager m_uiManager = default;

        public virtual void Start()
        {
            currentState = GetInitialState();
            if (currentState != null)
                currentState.EnterState();
        }

        public virtual void Update()
        {
            if (currentState != null)
                currentState.UpdateState();
        }

        public void ChangeState(BaseState newState)
        {
            currentState.ExitState();

            currentState = newState;
            currentState.EnterState();
        }

        protected virtual BaseState GetInitialState()
        {
            return null;
        }

        /// <summary>
        /// For debugging, will remove in final
        /// </summary>
        private void OnGUI()
        {
            string tmp = currentState != null ? currentState.m_name : $"No current state!";
            GUILayout.Label($"<color='black'><size=40>(tmp)</size></color>");
        }
    }
}
