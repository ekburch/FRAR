using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SimpleAnimatorHook : MonoBehaviour
{
    #region Protected Variables
    protected Animator m_animator;
    #endregion
    #region Public Properties
    public Animator Animator
    {
        get
        {
            if (!m_animator) m_animator = GetComponent<Animator>();
            return m_animator;
        }
    }

    public int State 
    { 
        get { return Animator.GetInteger("State"); }
        set { Animator.SetInteger("State", value); }
    }
    #endregion
}
