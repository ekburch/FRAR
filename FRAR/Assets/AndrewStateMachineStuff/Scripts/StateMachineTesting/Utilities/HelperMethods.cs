using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperMethods
{
    /// <summary>
    /// Shorthand way of checking a float as a bool
    /// </summary>
    /// <param name="_value">A floating point value assumed to be either 0 or 1</param>
    /// <returns>Whether this float is a 0 or 1</returns>
    public static bool AsBool(float _value)
    {
        return Mathf.Approximately(Mathf.Min(_value, 1), 1);
    }

    public static float BoolToFloat(bool _value)
    {
        return (_value) ? 1.0f : 0.0f;
    }
}