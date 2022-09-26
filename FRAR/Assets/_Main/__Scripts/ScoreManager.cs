using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace FRAR
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }
        public int Score { get; private set; }
        public int AnswerStreak { get; private set; }

		public int multiplier = 1;

        private void Awake()
        {
            Instance = this;
            ResetCombo();
        }

        public void UpdateScoreValue(int amount)
        {
            Score += amount * multiplier;
            if (Score <= 0) Score = 0;
        }

        public void IncreaseCombo()
        {
            AnswerStreak++;
            UpdateMultiplier(AnswerStreak);
        }

        public int UpdateMultiplier(int amount)
        {
            multiplier = (int)Mathf.Clamp(Mathf.FloorToInt(amount / 3), 1f, 4f);
            return multiplier;
        }

        public void ResetCombo()
        {
            AnswerStreak = 0;
            UpdateMultiplier(0);
        }

        public void ResetScoreValue()
        {
            Score = 0;
        }
    }
}
