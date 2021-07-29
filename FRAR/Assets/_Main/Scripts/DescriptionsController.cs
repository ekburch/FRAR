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
            titleTextObj.text = titleText;
            descriptionTextObj.text = descriptionText;
            titleTextObj.GetComponentInParent<SolverHandler>().TransformOverride = this.transform;
        }

        public void ResetText()
        {
            titleTextObj.text = "";
            descriptionTextObj.text = "";
            titleTextObj.GetComponentInParent<SolverHandler>().TransformOverride = resetTransform;
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

