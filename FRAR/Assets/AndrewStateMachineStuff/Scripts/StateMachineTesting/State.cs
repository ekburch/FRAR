using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class State : MonoBehaviour
{
    #region Inspector Assigned Variables
    [Header("Important References")]
    [SerializeField] protected List<StateTransition> m_transitions;
    [Header("Events")]
    [SerializeField] protected UnityEvent m_onEnter;
    [SerializeField] protected UnityEvent m_onExit;
    [SerializeField] protected UnityEvent<float> m_onTick;
    #endregion
    #region Protected Variables
    protected StateMachine m_parent;
    #endregion
    #region Public Properties
    public abstract StateType Type {get;}

    public UnityEvent<float> OnTick
    {
        get => m_onTick;
        set => m_onTick = value;
    }
    #endregion

    #region Initialization
    public void Init(StateMachine _parent)
    {
        // Cache parent reference
        m_parent = _parent;
    }
    #endregion

    #region State-Related Functionality
    /// <summary>
    /// Called by parent state machine once a state has been entered
    /// </summary>
    public virtual void OnEnter()
    {
        // Invoke any necessary events
        m_onEnter?.Invoke();
        // Debug log state entrance
        //UnityEngine.Debug.Log($"{m_parent.gameObject.name} entered state: {Type.ToString()} || Time: {Time.time}");
    }

    /// <summary>
    /// Called by parent state machine every frame this state is active
    /// </summary>
    public virtual StateType OnUpdate()
    {
        // Tick the global timer
        m_parent.StateTimer += Time.deltaTime;
        // By default, simply check the transitions
        return CheckForStateTransition();
    }

    /// <summary>
    /// Called by parent state machine once a state has been entered
    /// </summary>
    public virtual void OnExit()
    {
        // Invoke any necessary events
        m_onExit?.Invoke();
        // Debug log state exit
        //UnityEngine.Debug.Log($"{m_parent.gameObject.name} exited state: {Type.ToString()} || Time: {Time.time}");
    }
    #endregion

    #region Transition-Related Functionality
    /// <summary>
    /// Loops through inspector assigned transitions and checks if a new state should be entered
    /// </summary>
    /// <returns></returns>
    protected virtual StateType CheckForStateTransition()
    {
        // Make sure there are transitions
        if (m_transitions != null && m_transitions.Count > 0)
        {
            // Check each transition and see if it is valid
            foreach(StateTransition transition in m_transitions)
            {
                // If the transition is valid, return the new state type
                if (transition.IsValid(m_parent)) return transition.State;
            }
        }
        // If here is reached, no valid transitions were found so the state machine continues in this state
        return Type;
    }
    #endregion
}
[System.Serializable]
public class StateTransition
{
    public StateType State;
    public ConditionHolder[] Conditions;

    public bool IsValid(StateMachine _stateMachine)
    {
        // Make sure there are valid conditions
        if (Conditions == null || Conditions.Length < 1) return false;

        // Loop through each condition and make sure it is valid
        foreach(ConditionHolder condition in Conditions)
        {
            // Check IsValid for each condition, if it is false this transition is impossible
            if (!condition.IsValid(_stateMachine)) return false;
        }
        // If here is reached, no invalid conditions were found, so this transition is possible
        return true;
    }
}

[System.Serializable]
public class ConditionHolder
{
    [SerializeReference, ClassReference] public StateCondition Condition;
    public bool IsValid(StateMachine _stateMachine)
    {
        return Condition.IsValid(_stateMachine);
    }
}
