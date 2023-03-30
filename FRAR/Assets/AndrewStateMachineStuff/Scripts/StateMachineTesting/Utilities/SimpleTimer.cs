using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleTimer : MonoBehaviour
{
    #region Inspector Assigned Variables
    [Header("Default Configuration")]
    [SerializeField] protected bool m_startActive = true;
    [Header("Events")]
    [SerializeField] protected UnityEvent<float> m_onTick;
    #endregion
    #region Protected Variables
    protected bool m_isActive = false;
    public float m_value;
    #endregion
    #region Public Properties
    public bool IsActive 
    { 
        get => m_isActive; 
        set => m_isActive = value; 
    }

    public float Value { get => m_value; }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        IsActive = m_startActive;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isActive) return;
        m_value += Time.deltaTime;
        m_onTick?.Invoke(m_value);
    }

    public void SetActive(bool _value)
    {
        IsActive = _value;
        m_value = 0.0f;
    }

    private void OnEnable()
    {
        SetActive(m_startActive);
    }

    private void OnDisable()
    {
        SetActive(false);
    }
}
