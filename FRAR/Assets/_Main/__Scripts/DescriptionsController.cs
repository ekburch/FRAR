using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Runtime.CompilerServices;

namespace FRAR
{
    public class DescriptionsController : MonoBehaviour
    {
        [SerializeField]
        private GameObject descriptionPanelPrefab;

        /// <summary>
        /// Large Dialog example prefab to display
        /// </summary>
        public GameObject DescriptionPanelPrefab
        {
            get => descriptionPanelPrefab;
            set => descriptionPanelPrefab = value;
        }

        [SerializeField]
        private Renderer contentBackPanelRenderer = default;

        private TextMeshPro m_titleText = default;
        private TextMeshPro m_descriptionText = default;
        private Transform resetTransform = default;

        private bool _showDiscriptions;

        public bool ShowDescriptions 
        { 
            get => _showDiscriptions; 
            set => _showDiscriptions = value; 
        }

        [SerializeField] GameObject chevron = default;
        [SerializeField] DirectionalIndicator m_chevronDirectionalIndicator = default;

        private void Start()
        {
            _showDiscriptions = true;
        }

        public void UpdateText(string newTitle, string newDescription, Transform transform, bool isActiveAndEnabled)
        {
            //descriptionPanelPrefab = ObjectPool.SharedInstance.GetPooledObject();
            if (descriptionPanelPrefab != null)
            {
                if (_showDiscriptions)
                {
                    m_titleText = descriptionPanelPrefab.GetComponent<DialogShell>().TitleText;
                    m_descriptionText = descriptionPanelPrefab.GetComponent<DialogShell>().DescriptionText;
                    m_titleText.text = newTitle;
                    m_descriptionText.text = newDescription;

                    contentBackPanelRenderer.enabled = isActiveAndEnabled;
                    m_chevronDirectionalIndicator.DirectionalTarget = transform;
                }
                else
                {
                    return;
                }
            }

                //if (GetComponent<SolverHandler>() == null)
                //    return;
                //else
                //{
                //    GetComponent<SolverHandler>().TransformOverride = transform.transform;
                //}
            
        }

        public void ResetText()
        {
            if (descriptionPanelPrefab != null)
            {
               //foreach(GameObject go in ObjectPool.SharedInstance.pooledObjects)
               //{
               //    if (go.activeInHierarchy)
               //    {
               //        descriptionPanelPrefab = go;
               //        m_titleText.text = "";
               //        m_descriptionText.text = "";
               //        m_titleText = null;
               //        m_descriptionText = null;
               //        go.SetActive(false);
               //        //dialogInstance.SetActive(false);
               //    }
               //}
            }

                //if (GetComponent<SolverHandler>() != null)
                //    GetComponent<SolverHandler>().TransformOverride = resetTransform;
                //else
                //    return;
        }

        private void OnClosedDialogEvent(DialogResult obj)
        {
            if (obj.Result == DialogButtonType.Yes)
            {
                Debug.Log(obj.Result);
            }
        }
    }
}

