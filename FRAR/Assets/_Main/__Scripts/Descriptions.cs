using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;

namespace FRAR.UI
{
    public class Descriptions : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("GameObject that text is attached to follow")]
        private GameObject anchor;

        /// <summary>
        /// Getter/setter for anchor
        /// </summary>
        public GameObject Anchor
        {
            get { return anchor; }
            set { anchor = value; }
        }

        [Tooltip("Pivot point that text will rotate around.")]
        [SerializeField]
        private GameObject pivot;

        /// <summary>
        /// Pivot point that text will rotate around. 
        /// </summary>
        public GameObject Pivot => pivot;
    }
}

