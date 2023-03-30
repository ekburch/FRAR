using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateMachine : MonoBehaviour
{
    #region Inspector Assigned Variables
    [Header("Important References")]
    [SerializeField] protected List<State> m_states;
    [Header("Default Configuration")]
    [SerializeField] protected StateType m_defaultState = StateType.Idle;
    [Header("Events")]
    [SerializeField] protected UnityEvent<int> m_onStateChange;
    #endregion
    #region Protected Variables
    protected State m_currentState;

    protected float[] m_worldState;
    protected UnityEvent<WorldStateValue, float> m_onWorldStateValueChanged;
    #endregion
    #region Public Properties
    public State CurrentState
    {
        get => m_currentState;
        protected set
        {
            m_currentState = value;
            m_onStateChange?.Invoke((int) m_currentState.Type);
        }
    }

    public StateType CurrentStateType
    {
        get => CurrentState.Type;
    }

    public UnityEvent<int> OnStateChange
    {
        get => m_onStateChange;
        set => m_onStateChange = value;
    }

    public float StateTimer
    {
        get
        {
            return GetWorldStateValue(WorldStateValue.StateTimer);
        }
        set
        {
            SetWorldStateValue(WorldStateValue.StateTimer, value);
            if (m_currentState) m_currentState.OnTick?.Invoke(value);
        }
    }

    public float[] WorldState { get => m_worldState; }
    public UnityEvent<WorldStateValue, float> OnWorldStateValueChanged
    {
        get 
        {
            return m_onWorldStateValueChanged;
        }
        set
        {
            m_onWorldStateValueChanged = value;
        }
    }
    #endregion

    #region Initialization
    void Awake()
    {
        // Initialize the world state
        InitWorldStateValues();
        // Make sure there are states
        if (m_states == null || m_states.Count < 1) return;
        // Loop through each state and initialize its parent reference
        foreach(State state in m_states) state.Init(this);
        // Set default state
        SetState(m_defaultState);
    }

    void InitWorldStateValues()
    {
        m_onWorldStateValueChanged = new UnityEvent<WorldStateValue, float>();
        m_worldState = new float[Enum.GetValues(typeof(WorldStateValue)).Length];
        for(int i = 0; i < m_worldState.Length; i++)
        {
            SetWorldStateValue((WorldStateValue)i, 0.0f);
        }
    }
    #endregion

    #region Update
    void Update()
    {
        if (m_currentState == null) return;
        // Update the current state and check its transitions
        StateType _stateType = m_currentState.OnUpdate();
        // Check if a state transition has occurred
        if (_stateType != m_currentState.Type) SetState(_stateType);
    }
    #endregion

    #region State-Related Functionality
    public State GetState(StateType _type)
    {
        if (m_states == null || m_states.Count < 1) return null;

        for(int i = 0; i < m_states.Count; i++)
        {
            State state = m_states[i];
            if (state.Type == _type) return state;
        }
        return null;
    }

    public void SetState(StateType _type)
    {
        // Get the new state and make sure it is valid
        State newState = GetState(_type);
        if (newState == null) return;
        // If there is a current state, call exit logic
        if (m_currentState != null) m_currentState.OnExit();
        SetWorldStateValue(WorldStateValue.StateTimer, 0.0f);
        // Cache reference to new state and call entrance logic
        CurrentState = newState;
        CurrentState.OnEnter();
    }

    public void ResetToStart()
    {
        SetState(m_defaultState);

        for (int i = 0; i < m_worldState.Length; i++)
        {
            SetWorldStateValue((WorldStateValue)i, 0.0f);
        }
    }
    #endregion

    #region World State Value-Related Functionality
    public float GetWorldStateValue(WorldStateValue _stateRef)
    {
        if (m_worldState == null || m_worldState.Length < 1) return 0.0f;
        return m_worldState[(int)_stateRef];
    }
    public void SetWorldStateValue(WorldStateValue _stateRef, float _value)
    {
        if (m_worldState == null || m_worldState.Length < 1 || m_worldState[(int) _stateRef] == _value) return;
        m_worldState[(int) _stateRef] = _value;
        m_onWorldStateValueChanged?.Invoke(_stateRef, _value);
        //UnityEngine.Debug.Log($"{gameObject.name} SetWorldStateValue: {_stateRef} | Value: {_value} || Time: {Time.time}");
    }

    public void SetWorldStateValue(WorldStateValue _stateRef, bool _value)
    {
        if (m_worldState == null || m_worldState.Length < 1) return;
        SetWorldStateValue(_stateRef, HelperMethods.BoolToFloat(_value));
    }

    public bool HasWorldStateValue(WorldStateValue _stateRef, bool _value = true)
    {
        float stateValue = GetWorldStateValue(_stateRef);
        return (HelperMethods.AsBool(stateValue) == _value);
    }

    public bool HasWorldStateValueEqualTo(WorldStateValue _stateRef, float _value)
    {
        float stateValue = GetWorldStateValue(_stateRef);
        return (stateValue == _value);
    }

    public bool HasWorldStateValueGreaterThan(WorldStateValue _stateRef, float _valueToCheck)
    {
        float stateValue = GetWorldStateValue(_stateRef);
        return (stateValue > _valueToCheck);
    }

    public bool HasWorldStateValueGreaterThanOrEqualTo(WorldStateValue _stateRef, float _valueToCheck)
    {
        float stateValue = GetWorldStateValue(_stateRef);
        return (stateValue >= _valueToCheck);
    }

    public bool HasWorldStateValueLessThan(WorldStateValue _stateRef, float _valueToCheck)
    {
        float stateValue = GetWorldStateValue(_stateRef);
        return (stateValue < _valueToCheck);
    }

    public bool HasWorldStateValueLessThanOrEqualTo(WorldStateValue _stateRef, float _valueToCheck)
    {
        float stateValue = GetWorldStateValue(_stateRef);
        return (stateValue <= _valueToCheck);
    }

    #region Individual State Value Logic
    public void SetPumpEngaged(bool _value)
    {
        SetWorldStateValue(WorldStateValue.PumpEngaged, _value);
    }

    public void TogglePumpEngaged()
    {
        SetWorldStateValue(WorldStateValue.PumpEngaged, !(HasWorldStateValue(WorldStateValue.PumpEngaged)));

    }

    public void SetTankToPumpOpened(bool _value)
    {
        SetWorldStateValue(WorldStateValue.TankToPumpOpened, _value);
    }

    public void ToggleTankToPumpOpened()
    {
        SetWorldStateValue(WorldStateValue.TankToPumpOpened, !(HasWorldStateValue(WorldStateValue.TankToPumpOpened)));
    }

    public void SetSingleLineDischargeOpened(bool _value)
    {
        SetWorldStateValue(WorldStateValue.SingleLineDischargeOpened, _value);
    }

    public void ToggleSingleLineDischargeOpened()
    {
        SetWorldStateValue(WorldStateValue.SingleLineDischargeOpened, !(HasWorldStateValue(WorldStateValue.SingleLineDischargeOpened)));
    }

    public void SetThrottleDirection(int _value)
    {
        SetWorldStateValue(WorldStateValue.ThrottleDirection, _value);
    }

    public void SetThrottleDirection(float _value)
    {
        SetThrottleDirection((int)_value);
    }


    public void SetDischargePressure(float _value)
    {
        SetWorldStateValue(WorldStateValue.DischargePressure, _value);
    }
    #endregion
    #endregion
}

public enum StateType {Idle, Active, Startup, Cavitation, Fail, Win}

public enum WorldStateValue
{
    PumpEngaged, // Boolean: 0 = false, 1 = true
    TankToPumpOpened, // Boolean: 0 = false, 1 = true
    SingleLineDischargeOpened, // Boolean: 0 = false, 1 = true
    ThrottleDirection, // Integer: 0 = idle, 1 = increasing, -1 = decreasing
    DischargePressure, // Float
    StateTimer // Float, time elapsed in current state
}