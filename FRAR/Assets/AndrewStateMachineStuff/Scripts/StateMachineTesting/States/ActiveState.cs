using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveState : State
{
    #region Public Properties
    public override StateType Type => StateType.Active;
    #endregion
}