using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FRAR
{
    public class EnableOrDisableComponent : MonoBehaviour
    {
        public delegate void EnableOrDisableDelegate();
        public event EnableOrDisableDelegate OnToggleComponents;

        UnityEvent toggleEvent;

        private bool m_isToggled = default;
        public bool IsToggled { set => m_isToggled = value; }

        private void Start()
        {
            if (toggleEvent == null)
                toggleEvent = new UnityEvent();
            
        }

        public void CheckToToggle(Behaviour behaviour)
        {
            if (m_isToggled)
            {
                OnToggleComponents?.Invoke();
            }
            else
            {
                EnableOrDisable(behaviour);
            }
        }

        public void EnableOrDisable(Behaviour behaviour)
        {
            behaviour.enabled = !behaviour.enabled;
        }
    }
}

