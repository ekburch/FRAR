using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoolStateCondition : StateCondition
{
    #region Inspector Assigned Variables
    [Header("Default Configuration")]
    [SerializeField] protected bool m_valueToCheck = true;
    #endregion

    #region Inherited Functionality
    public override bool IsValid(StateMachine _stateMachine)
    {
        return _stateMachine.HasWorldStateValue(m_worldStateValueReference, m_valueToCheck);
        //return (HelperMethods.AsBool(m_value) == m_valueToCheck);
    }
    #endregion
}