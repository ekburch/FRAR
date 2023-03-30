using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailState : State
{
    #region Public Properties
    public override StateType Type => StateType.Fail;
    #endregion
    
    public override StateType OnUpdate()
    {
        // Other state check

        // By default, return this state
        return base.OnUpdate();
    }
}