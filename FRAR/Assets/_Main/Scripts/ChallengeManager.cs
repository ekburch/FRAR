using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

namespace FRAR
{
    [RequireComponent(typeof(QuestionLoader))][RequireComponent(typeof(ScoreManager))]
    public class ChallengeManager : MonoBehaviour
    {
        //public ChallengeQuestion[] challenges;
        //private static List<ChallengeQuestion> unansweredChallenges;
        [Header("Required to function")]
        [SerializeField] private QuestionLoader m_questionLoader = default;
        [SerializeField] private ScoreManager m_scoreManager = default;

        private ChallengeQuestionObject currentChallenge;

        [Header("Gamified Elements")]
        [SerializeField]
        [Tooltip("How long the quiz lasts")]
        private float m_quizDuration = default;
        [SerializeField]
        [Tooltip("How long of a delay between questions")]
        private float m_timeToNextQuestion = default;

        [Header("Required UI")]
        [SerializeField]
        private TextMeshPro m_bodyText = default;
        [SerializeField]
        private TextMeshPro m_titleText = default;
        [SerializeField] 
        private TextMeshPro m_timerText = default;
        [SerializeField]
        private TextMeshPro m_scoreText = default;
        [SerializeField]
        private PressableButtonHoloLens2 m_mainButton = default;

        [Header("Set in editor")]
        [SerializeField]
        [Tooltip("Instructions for the challenge mode")]
        private String m_instructionsText = "";
        [SerializeField]
        [Tooltip("Summary at the end of challenge mode")]
        private String m_summaryText = "";

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

            //GetRandomChallengeQuestion();
            UpdateTextElements(m_bodyText, m_instructionsText);
        }

        public void StartGame()
        {
            GetRandomChallengeQuestion();
            SetUpQuizTimer();
            m_scoreManager.ResetScoreValue();
            UpdateTextElements(m_titleText, "Question");
            m_mainButton.gameObject.SetActive(false);
            m_timerText.gameObject.SetActive(true);
            m_scoreText.gameObject.SetActive(true);
            m_isQuizMode = true;
        }

        private void GetRandomChallengeQuestion()
        {
            //int randomQuestionIndex = UnityEngine.Random.Range(0, unansweredChallenges.Count);
            //currentChallenge = unansweredChallenges[randomQuestionIndex];
            //
            //unansweredChallenges.RemoveAt(randomQuestionIndex);
            currentChallenge = m_questionLoader.GetUnaskedChallenge();
            UpdateTextElements(m_bodyText, currentChallenge.Question.ToString());
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
                        EndGame();
                        return;
                    }

                    m_currentTime = Mathf.Min(m_currentTime + Time.deltaTime, m_endTime);
                    DisplayTime(m_timeRemaining -= Time.deltaTime);
                    UpdateTextElements(m_scoreText, m_scoreManager.Score.ToString());
                }
            }
        }

        void DisplayTime (float timeToDisplay)
        {
            timeToDisplay += 1;

            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);

            UpdateTextElements(m_timerText, string.Format("{0:00}:{1:00}", minutes, seconds));
        }

        public void CheckAnswer(string answer)
        {
            string tmp;
            if (answer == currentChallenge.CorrectAnswer)
            {
                //Increase our score
                m_scoreManager.UpdateScoreValue(1);
                //Increase our multiplier
                m_scoreManager.IncreaseCombo();
                //Display text to confirm
                tmp = "Correct!";
            }
            else
            {
                //Reset our multiplier
                m_scoreManager.ResetCombo();
                //Display text to confirm we got it wrong
                tmp = "Incorrect!";
            }
            //Tell the display panel to go to the next question
            UpdateTextElements(m_bodyText, tmp);
            StartCoroutine("ShowNextQuestion");
        }

        private IEnumerator ShowNextQuestion()
        {
            yield return new WaitForSeconds(m_timeToNextQuestion);
            GetRandomChallengeQuestion();
        }

        private void UpdateTextElements(TextMeshPro textObj, string newText)
        {
            textObj.text = newText;
        }

        public void EndGame()
        {
            m_isPlaying = false;
            UpdateTextElements(m_bodyText, m_summaryText + "Your final score is " + m_scoreManager.Score.ToString());
            UpdateTextElements(m_titleText, "Game over!");
            m_mainButton.gameObject.SetActive(true);
            m_timerText.gameObject.SetActive(false);
            m_scoreText.gameObject.SetActive(false);
        }
    }
}
