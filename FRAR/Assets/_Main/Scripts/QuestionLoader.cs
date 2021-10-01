using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FRAR
{
    public class QuestionLoader : MonoBehaviour
    {
        private ChallengeQuestion[] challengeQuestions;
        private static List<ChallengeQuestion> unansweredChallenges;
        private static string[] questionsText;

        private void Awake()
        {
            LoadAllQuestions();
        }

        private void LoadAllQuestions()
        {
            string path = Application.streamingAssetsPath + "/challenge_questions.txt";
            using (StreamReader reader = new StreamReader(path))
            {
                string _questionsText = reader.ReadToEnd();
                questionsText = _questionsText.Split('\n');
            }

            char[] c = new char[questionsText.Length];
            for(int i = 0; i < questionsText.Length; i++)
            {
                c = questionsText[i].ToArray();
                c.CopyTo(challengeQuestions, i);
            }
        }

        public ChallengeQuestion GetUnaskedChallenge()
        {
            CheckToResetQuestions();

            var question = challengeQuestions.Where(t => t.Asked == false).OrderBy(t => Random.Range(0, int.MaxValue)).FirstOrDefault();
            question.Asked = true;
            return question;
        }

        private void CheckToResetQuestions()
        {
            if (challengeQuestions.Any(t => t.Asked == false) == false)
                ResetQuestions();
        }

        private void ResetQuestions()
        {
            foreach(var question in challengeQuestions)
            {
                question.Asked = false;
            }
        }
    }
}
