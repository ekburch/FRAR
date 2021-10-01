using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace FRAR
{
    public class ChallengeManager : MonoBehaviour
    {
        //public ChallengeQuestion[] challenges;
        //private static List<ChallengeQuestion> unansweredChallenges;
        [Header("Required to function")]
        [SerializeField] private QuestionLoader m_questionLoader = default;
        [SerializeField] private DescriptionsController m_descriptionsController = default;

        private ChallengeQuestion currentChallenge;

        [Header("Gamified Elements")]
        [SerializeField]
        [Tooltip("How long the quiz lasts")]
        private float m_quizDuration = default;
        [SerializeField]
        [Tooltip("How long of a delay between questions")]
        private float m_timeToNextQuestion = default;
        [SerializeField]
        private TextMeshPro m_timerText = default;
        float m_currentTime = default;
        float m_endTime = default;
        float m_startTime = default;
        float m_timeRemaining = default;
        bool m_isQuizMode = false;
        bool m_isPlaying = false;

        private void Start()
        {
            //if (unansweredChallenges == null || unansweredChallenges.Count == 0)
            //{
            //    unansweredChallenges = challenges.ToList<ChallengeQuestion>();
            //}

            //Will need to add logic to let user start quiz after a prompt with yes/no buttons

            GetRandomChallengeQuestion();
        }

        private void GetRandomChallengeQuestion()
        {
            //int randomQuestionIndex = UnityEngine.Random.Range(0, unansweredChallenges.Count);
            //currentChallenge = unansweredChallenges[randomQuestionIndex];
            //
            //unansweredChallenges.RemoveAt(randomQuestionIndex);
            currentChallenge = m_questionLoader.GetUnaskedChallenge();
        }

        private void SetUpQuizTimer()
        {
            m_startTime = Time.time;
            m_currentTime = m_startTime;
            m_endTime = m_startTime + m_quizDuration;
            m_timeRemaining = m_quizDuration;
            m_isPlaying = true;
        }

        private void Update()
        {
            if (m_isQuizMode)
            {
                if (m_isPlaying)
                {
                    if(m_currentTime >= m_endTime)
                    {
                        m_isPlaying = false;
                        return;
                    }

                    m_currentTime = Mathf.Min(m_currentTime + Time.deltaTime, m_endTime);
                    DisplayTime(m_timeRemaining -= Time.deltaTime);
                }
            }
        }

        void DisplayTime (float timeToDisplay)
        {
            timeToDisplay += 1;

            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);

            m_timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
