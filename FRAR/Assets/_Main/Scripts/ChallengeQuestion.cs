namespace FRAR
{
    [System.Serializable]
    public class ChallengeQuestion
    {
        public string Question { get; set; }
        public string[] Answers { get; set; }
        public int CorrectAnswer { get; set; }
        public bool Asked { get; set; }
        public bool isCorrect;
    }
}
