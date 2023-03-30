using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class StateDisplay : SimpleDisplayHook
{
    #region Inspector Assigned Variables
    [Header("Important References")]
    [SerializeField] StateMachine m_parentStateMachine;
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
        m_parentStateMachine.OnStateChange.AddListener(UpdateText);
        UpdateText((int) m_parentStateMachine.CurrentStateType);
    }
    #endregion

    public void UpdateText(int _stateType)
    {
        DisplayText.text = $"State: {m_parentStateMachine.CurrentStateType.ToString()}";
    }

    private void OnDestroy()
    {
        if (!m_parentStateMachine) return;
        m_parentStateMachine.OnStateChange.RemoveListener(UpdateText);
    }
}
