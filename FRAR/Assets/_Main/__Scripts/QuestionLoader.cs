#define USING_UI_SELECTION

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FRAR
{
    public class QuestionLoader : MonoBehaviour
    {
        [SerializeField]
        private ChallengeQuestionObject[] challengeQuestionGameObjects;

        [SerializeField] private ChallengeQuestion_ScriptableObject[] challengeQuestionScriptableObjects;

        //private static string[] questionsText;

        //private Dictionary<GameObject, string> componentsDictionary = new Dictionary<GameObject, string>();

        private void Start()
        {
            LoadAllQuestions();
        }

        private void LoadAllQuestions()
        {
            foreach (ChallengeQuestionObject challengeQuestion in challengeQuestionGameObjects)
            {
                challengeQuestion.Asked = false;
            }

            challengeQuestionScriptableObjects = Resources.LoadAll<ChallengeQuestion_ScriptableObject>("Questions");
            
            #region Old code 
            //string path = Application.streamingAssetsPath + "/challenge_questions.txt";
            //using (StreamReader reader = new StreamReader(path))
            //{
            //    string _questionsText = reader.ReadToEnd();
            //    questionsText = _questionsText.Split('\n');
            //}
            //
            //char[] c = new char[questionsText.Length];
            //for(int i = 0; i < questionsText.Length; i++)
            //{
            //    c = questionsText[i].ToArray();
            //    c.CopyTo(challengeQuestions, i);
            //}
            #endregion
        }

#if !USING_UI_SELECTION
        #region Using Gaze Controls
        public ChallengeQuestionObject GetUnaskedChallenge()
        {
            CheckToResetQuestions();

            var question = challengeQuestionGameObjects.Where(t => t.Asked == false).OrderBy(t => Random.Range(0, int.MaxValue)).FirstOrDefault();
            question.Asked = true;
            return question;
        }

        private void CheckToResetQuestions()
        {
            if (challengeQuestionGameObjects.Any(t => t.Asked == false) == false)
            {
                ResetQuestions();
            }
        }

        private void ResetQuestions()
        {
            foreach (var question in challengeQuestionGameObjects)
            {
                question.Asked = false;
            }
        }
        #endregion

#else
        #region Using UI Controls
        public ChallengeQuestion_ScriptableObject GetUnaskedChallenge()
        {
            CheckToResetQuestions();
            var question = challengeQuestionScriptableObjects.Where(t => t.Asked == false).OrderBy(t => Random.Range(0, int.MaxValue)).FirstOrDefault();
            question.Asked = true;
            return question;
        }

        private void CheckToResetQuestions()
        {
            if (challengeQuestionScriptableObjects.Any(t => t.Asked == false) == false)
            {
                ResetQuestions();
            }
        }

        private void ResetQuestions()
        {
            foreach (var question in challengeQuestionScriptableObjects)
            {
                question.Asked = false;
            }
        }
        #endregion
#endif
    }

    //[System.Serializable]
    //public class ChallengeQuestion
    //{
    //    public string Question { get; set; }
    //    public string[] Answers { get; set; }
    //    public int CorrectAnswer { get; set; }
    //    public bool Asked { get; set; }
    //}
}

