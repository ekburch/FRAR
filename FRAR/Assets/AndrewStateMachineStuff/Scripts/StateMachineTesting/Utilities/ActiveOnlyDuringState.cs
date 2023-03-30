using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveOnlyDuringState : MonoBehaviour
{
    #region Inspector Assigned Variables
    [Header("Important References")]
    [SerializeField] protected StateMachine m_parentStateMachine;
    [Header("Default Configuration")]
    [SerializeField] List<StateType> m_activeStates;
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
        m_parentStateMachine.OnStateChange.AddListener(DetermineActive);
        DetermineActive((int) m_parentStateMachine.CurrentStateType);
    }
    #endregion

    #region Core Functionality
    /// <summary>
    /// Uses bitwise operators to determine whether or not the gameObject should be active during the state
    /// </summary>
    protected void DetermineActive(int _stateIndex)
    {
        if (m_parentStateMachine == null || m_activeStates == null || m_activeStates.Count < 1) return;
        bool shouldBeActive = (m_activeStates.Any(i => ((StateType) _stateIndex) == i));
        UnityEngine.Debug.Log($"{gameObject.name} DetermineActive: {shouldBeActive} || Time: {Time.time}");
        gameObject.SetActive(shouldBeActive);
    }
    #endregion

    private void OnDestroy()
    {
        if (!m_parentStateMachine) return;
        m_parentStateMachine.OnStateChange.RemoveListener(DetermineActive);
    }
}
