using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class StateCondition
{
    #region Inspector Assigned Variables
    [SerializeField] protected WorldStateValue m_worldStateValueReference;
    #endregion

    public abstract bool IsValid(StateMachine _stateMachine);
}