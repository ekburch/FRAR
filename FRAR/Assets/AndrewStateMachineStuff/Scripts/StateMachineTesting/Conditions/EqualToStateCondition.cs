using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EqualToStateCondition : StateCondition
{
    #region Inspector Assigned Variables
    [Header("Default Configuration")]
    [SerializeField] protected float m_valueToCheck;
    #endregion

    #region Inherited Functionality
    public override bool IsValid(StateMachine _stateMachine)
    {
        return _stateMachine.HasWorldStateValueGreaterThanOrEqualTo(m_worldStateValueReference, m_valueToCheck);
        //return (m_checkOrEqualTo) ? (m_value >= m_valueToCheck) : (m_value > m_valueToCheck);
    }
    #endregion
}
