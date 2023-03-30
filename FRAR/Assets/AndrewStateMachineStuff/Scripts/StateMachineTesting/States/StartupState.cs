using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupState : State
{
    #region Inspector Assigned Variables
    [SerializeField] [Tooltip("State values used for comparison between current and previous world state")]
    List<WorldStateValue> m_stateValuesToCompare; // 
    #endregion
    #region Private Variables
    private float[] m_previousWorldState;
    private const int m_maxStateValueIndex = 5; // specifies the maximum index to check in world state, currently 5 to avoid checking any numerical values
    #endregion
    #region Public Properties
    public override StateType Type => StateType.Startup;
    #endregion

    #region State-Related Functionality
    public override void OnEnter()
    {
        // Cache the current world state
        m_previousWorldState = new float[m_parent.WorldState.Length];
        for(int i = 0; i < m_parent.WorldState.Length; i++)
        {
            // Cache the current value of the world state
            m_previousWorldState[i] = m_parent.WorldState[i];
        }
        // Call base functionality
        base.OnEnter();
    }

    protected override StateType CheckForStateTransition()
    {
        // Loop through each world state value to check
        if (m_stateValuesToCompare != null && m_stateValuesToCompare.Count > 0)
        {
            foreach(WorldStateValue value in m_stateValuesToCompare)
            {
                if (value != WorldStateValue.TankToPumpOpened)
                {
                    // Compare current world state to previous one
                    if (m_parent.WorldState[((int) value)] != m_previousWorldState[((int)value)])
                    {
                        // Enter Cavitation
                        return StateType.Cavitation;
                    }
                }
            }
        }
        // ALTERNATIVE: Go directly through the world state and check indices (assumes you have separated boolean and numerical values)
        /*
        // Check current world state vs previous one
        for (int i = 0; i < m_maxStateValueIndex; i++)
        {
            // Make sure this is not TankToPumpOpened
            if (i != ((int)WorldStateValue.TankToPumpOpened))
            {
                // Compare current world state to previous one
                if (m_parent.WorldState[i] != m_previousWorldState[i])
                {
                    // Enter Cavitation
                    return StateType.Cavitation;
                }
            }
        }
        */
        return base.CheckForStateTransition();
    }

    public override void OnExit()
    {
        // Clear the previous world state cached
        m_previousWorldState = null;
        // Call base functionality
        base.OnExit();
    }
    #endregion



}
