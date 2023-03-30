using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinState : State
{
    #region Public Properties
    public override StateType Type => StateType.Win;
    #endregion
}