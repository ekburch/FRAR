using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit;
using UnityEngine;
using System.Collections;

namespace FRAR
{
	public class MoveAxisConstraintExtension : TransformConstraint
    {
        #region Properties

        [SerializeField]
        [EnumFlags]
        [Tooltip("Constrain movement along an axis")]
        private AxisFlags constraintOnMovement = 0;

        /// <summary>
        /// Constrain movement along an axis
        /// </summary>
        public AxisFlags ConstraintOnMovement
        {
            get => constraintOnMovement;
            set => constraintOnMovement = value;
        }

        [SerializeField]
        [Tooltip("Relative to rotation at manipulation start or world")]
        private bool useLocalSpaceForConstraint = false;

        /// <summary>
        /// Relative to rotation at manipulation start or world
        /// </summary>
        public bool UseLocalSpaceForConstraint
        {
            get => useLocalSpaceForConstraint;
            set => useLocalSpaceForConstraint = value;
        }

        public override TransformFlags ConstraintType => TransformFlags.Move;

        [SerializeField] Transform m_frontBounds = default;
        public Transform FrontBounds
		{
            get => m_frontBounds;
			set => m_frontBounds = value;
		}

        [SerializeField] Collider m_bounds = default;
        public Collider Bounds
        {
            get => m_bounds;
            set => m_bounds = value;
        }

        [SerializeField] private const float MinDistanceFromBounds = 0.001f;
        [SerializeField] private const float MaxDistanceFromBounds = 0.01f;

		private Vector3 startPosition = default;
        private Collider m_collider = default;
        [SerializeField] Collider m_collisionTrigger = default;

        private IEnumerator m_coroutine;
        private bool isWithinBounds;
        private bool isStopped;

        #endregion Properties

        #region Private Methods

        private void Start()
        {
            m_coroutine = CheckToStopManipulation();
            startPosition = transform.localPosition;
            m_collider = GetComponent<Collider>();
            StartCoroutine(m_coroutine);
        }

		private void Update()
		{
            
            TryStopManipulation();
        }

        private IEnumerator CheckToStopManipulation()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.01f);

                if (!isWithinBounds)
                {
                    if (Vector3.Distance(transform.position, m_bounds.transform.position) > MaxDistanceFromBounds)
                    {
                        isStopped = true;
                        var objectManipulator = GetComponent<ObjectManipulator>();
                        objectManipulator.ForceEndManipulation();
                        transform.position = m_bounds.transform.position;
                    }
                }
                else if (isWithinBounds)
                {
                    if (!(Vector3.Distance(transform.position, m_bounds.transform.position) > MinDistanceFromBounds)) continue;
                    isStopped = false;
                }
                else
                {
                    break;
                }
            }
        }

		#endregion

		#region Public Methods

		/// <summary>
		/// Removes movement along a given axis if its flag is found
		/// in ConstraintOnMovement
		/// </summary>
		public override void ApplyConstraint(ref MixedRealityTransform transform)
        {
            Quaternion inverseRotation = Quaternion.Inverse(worldPoseOnManipulationStart.Rotation);
            Vector3 position = transform.Position;
            if (constraintOnMovement.HasFlag(AxisFlags.XAxis))
            {
                if (useLocalSpaceForConstraint)
                {
                    position = inverseRotation * position;
                    position.x = (inverseRotation * worldPoseOnManipulationStart.Position).x;
                    position = worldPoseOnManipulationStart.Rotation * position;
                }
                else
                {
                    position.x = worldPoseOnManipulationStart.Position.x;
                }
            }
            if (constraintOnMovement.HasFlag(AxisFlags.YAxis))
            {
                if (useLocalSpaceForConstraint)
                {
                    position = inverseRotation * position;
                    position.y = (inverseRotation * worldPoseOnManipulationStart.Position).y;
                    position = worldPoseOnManipulationStart.Rotation * position;
                }
                else
                {
                    position.y = worldPoseOnManipulationStart.Position.y;
                }
            }
            if (constraintOnMovement.HasFlag(AxisFlags.ZAxis))
            {
                if (useLocalSpaceForConstraint)
                {
                    position = inverseRotation * position;
                    position.z = (inverseRotation * worldPoseOnManipulationStart.Position).z;
                    position = worldPoseOnManipulationStart.Rotation * position;
                }
                else
                {
                    position.z = worldPoseOnManipulationStart.Position.z;
                }
            }
			transform.Position = position;
		}

        /// <summary>
        /// Stop object manipulation if it exceeds certain boundaries
        /// </summary>
        public void TryStopManipulation()
        {
            if (m_bounds != null)
            {
                if (m_bounds.bounds.Contains(transform.position))
                {
                    isWithinBounds = true;
                    m_collider.enabled = true;
                }
                else
                {
                    Debug.Log("Outside of boundaries!");
					GetComponent<ConstraintManager>();
					var objectManipulator = GetComponent<ObjectManipulator>();
					if (objectManipulator != null)
					{
                        Debug.Log("Force end manipulation");
						objectManipulator.ForceEndManipulation();
                        isWithinBounds = false;
                        m_collider.enabled = false;
                        //transform.position = m_bounds.bounds.center;
					}
				}
            }
            if (m_collisionTrigger != null)
			{
				if (m_collisionTrigger.bounds.Intersects(m_collider.bounds))
				{
					GetComponent<ConstraintManager>();
					var objectManipulator = GetComponent<ObjectManipulator>();
					if (objectManipulator != null)
					{
						objectManipulator.ForceEndManipulation();
					}
				}
			}
		}

		public void OnCollisionEnter(Collision collision)
		{
			if(collision.gameObject.name == "Bounds")
			{
                GetComponent<ConstraintManager>();
                var objectManipulator = GetComponent<ObjectManipulator>();
                if (objectManipulator != null)
                {
                    objectManipulator.ForceEndManipulation();
                }
            }
		}

        public void ClampWithinBounds()
        {
            Vector3 position = transform.position;
			float yMin = m_bounds.bounds.min.y;
			float yMax = m_bounds.bounds.max.y;
			float movementClamp = Mathf.Clamp(transform.position.y, yMin, yMax);
            transform.position = new Vector3(position.x, movementClamp, position.z);
        }

		#endregion Public Methods
	}
}
