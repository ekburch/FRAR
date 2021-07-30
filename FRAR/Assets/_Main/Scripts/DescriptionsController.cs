using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;

namespace FRAR.UI
{
    public class DescriptionsController : MonoBehaviour, IMixedRealityFocusHandler
    {
        [SerializeField]
        private TextMeshPro titleTextObj = default;
        [SerializeField]
        private TextMeshPro descriptionTextObj = default;
        [SerializeField]
        private Transform resetTransform = default;

        public string titleText = "Name this object";
        public string descriptionText = "New description for this object";

        public void UpdateText()
        {
            if (titleTextObj && descriptionTextObj != null)
            {
                titleTextObj.text = titleText;
                descriptionTextObj.text = descriptionText;

                if (titleTextObj.GetComponentInParent<SolverHandler>() != null)
                    titleTextObj.GetComponentInParent<SolverHandler>().TransformOverride = this.transform;
                else
                    return;
            }
        }

        public void ResetText()
        {
            if (titleTextObj && descriptionTextObj != null)
            {
                titleTextObj.text = "";
                descriptionTextObj.text = "";

                if (titleTextObj.GetComponentInParent<SolverHandler>() != null)
                    titleTextObj.GetComponentInParent<SolverHandler>().TransformOverride = resetTransform;
                else
                    return;
            }
        }

        public void OnFocusEnter(FocusEventData eventData)
        {
            UpdateText();
        }

        public void OnFocusExit(FocusEventData eventData)
        {
            ResetText();
        }
    }
}

