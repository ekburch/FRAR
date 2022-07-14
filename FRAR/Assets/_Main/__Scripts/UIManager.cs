using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FRAR
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] GameObject m_TextDisplay = default;
        [SerializeField] TextMeshProUGUI m_text = default;

        public void ShowHideUIPrompt()
        {
            m_TextDisplay.SetActive(!m_TextDisplay.activeSelf);
        }

        public void UpdateText(string newText)
        {
            m_text.text = newText;
        }
    }
}
