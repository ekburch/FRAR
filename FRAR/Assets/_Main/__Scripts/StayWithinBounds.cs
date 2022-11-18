using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRAR
{
    public class StayWithinBounds : MonoBehaviour
    {
        [SerializeField] Transform m_frontBounds = default;
        public Transform FrontBounds
        {
            get => m_frontBounds;
            set => m_frontBounds = value;
        }

        [SerializeField] Transform m_backBounds = default;
        public Transform BackBounds
        {
            get => m_backBounds;
            set => m_backBounds = value;
        }

        private Vector3 startPosition;

        // Start is called before the first frame update
        void Start()
        {
            startPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (transform.position.y < m_frontBounds.position.y || transform.position.y > m_backBounds.position.y)
                transform.position = startPosition;
        }
    }
}