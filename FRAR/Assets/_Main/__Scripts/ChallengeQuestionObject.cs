using UnityEngine;

namespace FRAR
{
    [System.Serializable]
    public class ChallengeQuestionObject : MonoBehaviour
    {
        public string Question;
        public string CorrectAnswer { get => gameObject.name; }
        public bool Asked { get; set; }
    }
}
