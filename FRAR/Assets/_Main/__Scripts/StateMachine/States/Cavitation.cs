using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRAR
{
    public class Cavitation : BaseState
    {
        public Cavitation(PumpActionSM pumpActionSM) : base("Cavitation", pumpActionSM) { }

        private bool m_stoppedCavitation = default;
        public bool StoppedCavitation { set => m_stoppedCavitation = value; }

        float m_startTime = default;
        float m_currentTime = default;
        float m_endTime = default;
        float m_timeUntilFailure = 30f;
        bool m_isCountingDown = false;

        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("CURRENT STATE: CAVITATION");
            m_startTime = Time.time;
            m_currentTime = m_startTime;
            m_endTime = m_startTime + m_timeUntilFailure;
            m_isCountingDown = true;
        }

        public override void UpdateState()
        {
            base.UpdateState();
            if (m_isCountingDown)
            {
                if (!m_stoppedCavitation)
                {
                    if (m_currentTime >= m_endTime)
                    {
                        m_isCountingDown = false;
                        StateMachine.ChangeState(((PumpActionSM)StateMachine).FailState);
                    }
                    else
                    {
                        m_currentTime = Mathf.Min(m_currentTime + Time.deltaTime, m_endTime);

                    }
                }
                else
                {
                    m_isCountingDown = false;
                    StateMachine.ChangeState(((PumpActionSM)StateMachine).PumpState2);
                }
            }
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}
