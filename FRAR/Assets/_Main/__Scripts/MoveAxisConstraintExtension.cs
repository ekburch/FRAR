using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit;
using UnityEngine;

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

        [SerializeField] Transform m_frontBounds = null;
        public Transform FrontBounds
		{
            get => m_frontBounds;
			set => m_frontBounds = value;
		}

		private Vector3 startPosition = default;
        private Collider m_collider = default;
        [SerializeField] Collider m_collisionTrigger = default;

        #endregion Properties

        #region Private Methods

        private void Start()
        {
            startPosition = transform.localPosition;
            m_collider = GetComponent<Collider>();
        }

		private void Update()
		{
            TryStopManipulation();
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

		#endregion Public Methods
	}
}
