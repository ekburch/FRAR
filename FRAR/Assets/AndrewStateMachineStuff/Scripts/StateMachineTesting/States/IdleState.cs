using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    #region Inspector Assigned Variables
    #endregion
    #region Public Properties
    public override StateType Type => StateType.Idle;
    #endregion
    
    public override StateType OnUpdate()
    {
        // Other state check

        // By default, return this state
        return base.OnUpdate();
    }
}