using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRAR
{
    public class HandAnimatorManager : MonoBehaviour
    {
        public static HandAnimatorManager instance;
        public GameObject[] m_objectsToTrack = default;
        List<Animator> animators = new List<Animator>();

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            if (m_objectsToTrack.Length >= 1)
            {
                for (int i = 0; i < m_objectsToTrack.Length; i++)
                {
                    animators.Add(m_objectsToTrack[i].GetComponent<Animator>());
                    animators[i].enabled = false;
                }
            }
            else
                return;
        }

        public void ActivateAnimatorByName(string name, bool isActive)
        {
            if (m_objectsToTrack.Length >= 1)
            {
                for (int i = 0; i < m_objectsToTrack.Length; i++)
                {
                    animators[i].enabled = m_objectsToTrack[i].name == name && isActive;
                }
            }
        }
    }
}