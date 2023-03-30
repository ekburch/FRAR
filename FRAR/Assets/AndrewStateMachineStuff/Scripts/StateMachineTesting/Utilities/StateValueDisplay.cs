using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateValueDisplay : SimpleDisplayHook
{
    #region Inspector Assigned Variables
    [SerializeField] StateMachine m_parentStateMachine;
    [SerializeField] WorldStateValue m_valueToListenTo;
    [SerializeField] WorldStateValueType m_valueType = WorldStateValueType.Float;
    #endregion

    #region Initialization
    // Start is called before the first frame update
    private void Start()
    {
        // Initialize with the inspector assigned state machine reference
        Init(m_parentStateMachine);
    }

    /// <summary>
    /// Cache state machine reference and hook up enable/disable functionality to OnStateChange event
    /// </summary>
    /// <param name="_parentStateMachine"></param>
    public void Init(StateMachine _parentStateMachine)
    {
        // Cache parent state machine
        m_parentStateMachine = _parentStateMachine;
        // Hook up event
        m_parentStateMachine.OnWorldStateValueChanged.AddListener(UpdateStateValue);
        m_leadingText = $"{m_valueToListenTo.ToString()}: ";
        UpdateStateValue(m_valueToListenTo, 0.0f);
    }
    #endregion

    public void UpdateStateValue(WorldStateValue _worldStateRef, float _value)
    {
        if (_worldStateRef == m_valueToListenTo)
        {
            //UnityEngine.Debug.Log($"{gameObject.name} - State: {_worldStateRef} | Value: {_value} || Time: {Time.time}");
            string text = _value.ToString("0.00");
            switch (m_valueType)
            {
                case WorldStateValueType.Boolean:
                    text = (HelperMethods.AsBool(_value) ? "True" : "False");
                    break;
                case WorldStateValueType.Integer:
                    text = ((int)_value).ToString();
                    break;
                default:
                    break;
            }
            SetText(text);
        }
    }

    private void OnDestroy()
    {
        if (!m_parentStateMachine) return;
        m_parentStateMachine.OnWorldStateValueChanged.RemoveListener(UpdateStateValue);
    }
}

public enum WorldStateValueType {Float, Integer, Boolean}
