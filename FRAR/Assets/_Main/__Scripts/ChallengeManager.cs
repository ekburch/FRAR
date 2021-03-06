//#define GAZE_CONTROLS

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine.UI;

namespace FRAR
{
    [RequireComponent(typeof(QuestionLoader))][RequireComponent(typeof(ScoreManager))]
    public class ChallengeManager : MonoBehaviour
    {
        public static ChallengeManager Instance;
        //public ChallengeQuestion[] challenges;
        //private static List<ChallengeQuestion> unansweredChallenges;
        [Header("Required to function")]
        [SerializeField] private QuestionLoader m_questionLoader = default;
        [SerializeField] private ScoreManager m_scoreManager = default;

        private ChallengeQuestionObject currentChallenge;
        private ChallengeQuestion_ScriptableObject currChallengeQuestion;

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
        [SerializeField]
        private PressableButtonHoloLens2[] m_answerButtons;
        [SerializeField]
        private bool isUsingUIControls = false;

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
        bool m_timerIsActive = false;

        IEnumerator coroutine = default;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError($"ChallengeManager.Awake(): {Instance} already exists, destroying duplicate gameObject.");
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            //if (unansweredChallenges == null || unansweredChallenges.Count == 0)
            //{
            //    unansweredChallenges = challenges.ToList<ChallengeQuestion>();
            //}

            //Will need to add logic to let user start quiz after a prompt with yes/no buttons

            //GetRandomChallengeQuestion();
            UpdateTextElements(m_bodyText, m_instructionsText);
            coroutine = ShowNextQuestion();
        }

        public void StartGame()
        {
            SetUpQuizTimer();
            m_scoreManager.ResetScoreValue();
            UpdateTextElements(m_titleText, "Question");
            m_mainButton.gameObject.SetActive(false);
            m_timerText.gameObject.SetActive(true);
            m_scoreText.gameObject.SetActive(true);
            ToggleAnswerButtons(true);
            m_isQuizMode = true;
            GetRandomChallengeQuestion();
        }

        private void GetRandomChallengeQuestion()
        {
            //int randomQuestionIndex = UnityEngine.Random.Range(0, unansweredChallenges.Count);
            //currentChallenge = unansweredChallenges[randomQuestionIndex];
            //
            //unansweredChallenges.RemoveAt(randomQuestionIndex);
#if GAZE_CONTROLS
            currentChallenge = m_questionLoader.GetUnaskedChallenge();
            UpdateTextElements(m_bodyText, currentChallenge.Question.ToString());
#else
            currChallengeQuestion = m_questionLoader.GetUnaskedChallenge();
            UpdateTextElements(m_bodyText, currChallengeQuestion.Question.ToString());
            SetUpAnswerUIForQuestions(currChallengeQuestion);
#endif
        }

        private void SetUpQuizTimer()
        {
            m_startTime = Time.time;
            m_currentTime = m_startTime;
            m_endTime = m_startTime + m_quizDuration;
            m_timeRemaining = m_quizDuration;
            m_timerIsActive = true;
        }

        private void Update()
        {
            if (m_isQuizMode)
            {
                if (m_timerIsActive)
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
            if (m_isQuizMode)
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
                StartCoroutine(coroutine);
            }
        }
        #region
#if !GAZE_CONTROLS
        public void SubmitAnswer(int answer)
        {
            bool isCorrect = answer == currChallengeQuestion.CorrectAnswer;
            CheckSubmittedAnswer(isCorrect);
        }

        public void CheckSubmittedAnswer(bool isCorrect)
        {
            if (m_isQuizMode)
            {
                ToggleAnswerButtons(false);
                string tmp;

                if (isCorrect)
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
                Invoke("GetRandomChallengeQuestion", m_timeToNextQuestion);
            }
        }
#endif
        #endregion
        public IEnumerator ShowNextQuestion()
        {
            yield return new WaitForSecondsRealtime(m_timeToNextQuestion);
            GetRandomChallengeQuestion();
            yield return null;
        }

        private void UpdateTextElements(TextMeshPro textObj, string newText)
        {
            textObj.text = newText;
        }

        #region UI Control Setup
#if !GAZE_CONTROLS
        private void SetUpAnswerUIForQuestions(ChallengeQuestion_ScriptableObject challengeQuestion)
        {
            for (int i = 0; i < challengeQuestion.Answers.Length; i++)
            {
                m_answerButtons[i].GetComponentInChildren<TextMeshPro>().text = challengeQuestion.Answers[i];
                m_answerButtons[i].gameObject.SetActive(true);
            }

            //for (int i = challengeQuestion.Answers.Length; i < m_answerButtons.Length; i++)
            //{
            //    m_answerButtons[i].gameObject.SetActive(false);
            //}
        }

        private void ToggleAnswerButtons(bool value)
        {
            for (int i = 0; i < m_answerButtons.Length; i++)
            {
                m_answerButtons[i].gameObject.SetActive(value);
            }
        }
#endif
        #endregion
        public void EndGame()
        {
            m_timerIsActive = false;
            UpdateTextElements(m_bodyText, m_summaryText + "Your final score is " + m_scoreManager.Score.ToString());
            UpdateTextElements(m_titleText, "Game over!");
            StopAllCoroutines();
            m_mainButton.gameObject.SetActive(true);
            ToggleAnswerButtons(false);
            m_timerText.gameObject.SetActive(false);
            m_scoreText.gameObject.SetActive(false);
            m_isQuizMode = false;
        }
    }
}

