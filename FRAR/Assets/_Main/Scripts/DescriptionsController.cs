using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;

namespace FRAR
{
    public class DescriptionsController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro m_titleText = default;
        [SerializeField]
        private TextMeshPro m_descriptionText = default;
        [SerializeField]
        private Transform resetTransform = default;

        public void UpdateText(string newTitle, string newDescription, Transform transform)
        {
            if (m_titleText && m_descriptionText != null)
            {
                m_titleText.text = newTitle;
                m_descriptionText.text = newDescription;

                if (GetComponent<SolverHandler>() != null)
                    GetComponent<SolverHandler>().TransformOverride = transform.transform;
                else
                    return;
            }
        }

        public void ResetText()
        {
            if (m_titleText && m_descriptionText != null)
            {
                m_titleText.text = "";
                m_descriptionText.text = "";

                if (GetComponent<SolverHandler>() != null)
                    GetComponent<SolverHandler>().TransformOverride = resetTransform;
                else
                    return;
            }
        }
    }
}

