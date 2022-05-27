using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class ChallengeQuestion_ScriptableObject : ScriptableObject
{
    [SerializeField]
    private string question;
    [SerializeField]
    private string[] answers;
    [SerializeField]
    private int correctAnswer;

    public string Question => question;
    public string[] Answers => answers;
    public int CorrectAnswer => correctAnswer;

    public bool Asked { get; internal set; }

    private void OnValidate()
    {
        if (correctAnswer > answers.Length) correctAnswer = 0;
        //RenameToMatchQuestionAndAnswer();
    }

    //private void RenameToMatchQuestionAndAnswer()
    //{
    //    string desiredName = string.Format("{0} [{1}]",
    //        question.Replace("?", ""),
    //        answers[correctAnswer]);
    //
    //    string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
    //    string shouldEndWith = "/" + desiredName + ".asset";
    //    if (assetPath.EndsWith(shouldEndWith) == false)
    //    {
    //        Debug.Log("Want to rename to " + desiredName);
    //        AssetDatabase.RenameAsset(assetPath, desiredName);
    //        AssetDatabase.SaveAssets();
    //    }
    //}
}
