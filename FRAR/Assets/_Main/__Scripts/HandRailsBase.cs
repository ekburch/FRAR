using System.Collections.Generic;
using System.Linq;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using Unity.XR.CoreUtils;
using Microsoft.MixedReality.Toolkit.Extensions.HandPhysics;
using System;

namespace FRAR.Utils
{
	public abstract class HandRailsBase : MonoBehaviour, IMixedRealityPointerHandler
    {
		[Header("General Settings")]
		[SerializeField]
		private List<GameObject> wayPoints = new List<GameObject>();

		[SerializeField]
		private TrackedHandJoint joint = TrackedHandJoint.IndexMiddleJoint;

		[SerializeField]
		private bool trackRightHand = true;

		[SerializeField]
		private float maxDistanceFromLine = 0.1f;

		[SerializeField]
		private float maxDistanceFromWayPoint = 0.02f;

		[SerializeField]
		private bool trackPinch = true;

		[SerializeField]
		private bool trackGrab = true;

		[SerializeField]
		private bool isTrackingPose = false;

		public bool IsTrackingPose { set => isTrackingPose = value; }

		[SerializeField]
		private Collider m_collider = default;

		[Header("Debug Settings")]
		[SerializeField]
		private bool drawGizmoLines = false;

		[SerializeField]
		private Color gizmoLineColor = Color.yellow;

		[SerializeField]
		private bool drawRuntimeDebugObjects = false;

		[Header("Runtime debug Settings")]
		[SerializeField]
		private GameObject runtimeDebugObject;

		[SerializeField]
		private Material wayPointLineMaterial;

		[SerializeField]
		private float wayPointLineWidth = 0.01f;

		protected int CurrentIndex { get; private set; } = 0;

		protected Vector3 PointOnLine { get; private set; } =
			Vector3.negativeInfinity;

		protected List<Vector3> WayPointLocations { get; private set; }

		protected float TotalLength { get; private set; }

		private IMixedRealityHandJointService handJointService;
		private IMixedRealityHandJointService HandJointService =>
			handJointService ??
				(handJointService =
				  CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>());
		
		private IMixedRealityPointer Pointer { get; set; }

		private void OnEnable()
		{
			isTrackingPose = true;
			CurrentIndex = 0;
			WayPointLocations = wayPoints.Select(p => p.transform.position).ToList();
			TotalLength = WayPointLocations.TotalLength();
			DrawRuntimeDebugObjects();
		}

		void Update()
		{
			if (isTrackingPose)
			{
				var trackedHand = trackRightHand ? Handedness.Right : Handedness.Left;
				if (HandJointService.IsHandTracked(trackedHand) &&
					((GestureUtils.IsPinching(trackedHand) && trackPinch) ||
						(GestureUtils.IsGrabbing(trackedHand) && trackGrab)))
				{
					var jointPosition = HandJointService.RequestJointTransform(joint, trackedHand).position;

					// Find closest point on the current segment
					PointOnLine = jointPosition.GetClosestPointOnLineSegment(WayPointLocations, CurrentIndex);
					if (Vector3.Distance(PointOnLine, jointPosition) > maxDistanceFromLine)
					{
						return;
					}

					// Check if we should change to another line segment
					var closestIndex = jointPosition.GetClosestLineSegmentIndex(WayPointLocations);
					if (closestIndex != CurrentIndex)
					{
						// Can we jump a segment forward?
						if (closestIndex == CurrentIndex + 1)
						{
							if (Vector3.Distance(WayPointLocations[CurrentIndex + 1], PointOnLine) <
								maxDistanceFromWayPoint)
							{
								CurrentIndex++;
								PointOnLine = jointPosition.GetClosestPointOnLineSegment(WayPointLocations, CurrentIndex);
							}
						}
						// If not, can we jump a segment backwards?
						else if (closestIndex == CurrentIndex - 1)
						{
							if (Vector3.Distance(WayPointLocations[CurrentIndex], PointOnLine) <
									maxDistanceFromWayPoint)
							{
								CurrentIndex--;
								PointOnLine = jointPosition.GetClosestPointOnLineSegment(WayPointLocations, CurrentIndex);
							}
						}
					}
					OnLocationUpdated();
				}
			}
		}

		protected abstract void OnLocationUpdated();

		#region Debugging
		private void OnDrawGizmos()
		{
			if (!drawGizmoLines)
			{
				return;
			}
			Gizmos.color = gizmoLineColor;
			var i = 0;
			while (i < wayPoints.Count - 1)
			{
				Gizmos.DrawLine(wayPoints[i].transform.position, wayPoints[i + 1].transform.position);
				i++;
			}
		}

		private void DrawRuntimeDebugObjects()
		{
			if (drawRuntimeDebugObjects && runtimeDebugObject != null)
			{
				var lineTemplate = new GameObject("DebugLine");
				lineTemplate.transform.parent = transform;
				var lineRenderer = lineTemplate.AddComponent<LineRenderer>();
				lineRenderer.useWorldSpace = true;
				lineRenderer.startWidth = wayPointLineWidth;
				lineRenderer.endWidth = lineRenderer.startWidth;
				lineRenderer.material = wayPointLineMaterial;

				for (var i = 0; i < WayPointLocations.Count; i++)
				{
					Instantiate(runtimeDebugObject, WayPointLocations[i], Quaternion.identity, transform);
					if (i > 0)
					{
						var lineObject = Instantiate(lineTemplate);
						lineObject.GetComponent<LineRenderer>().SetPositions(new[] { WayPointLocations[i - 1], WayPointLocations[i] });
					}
				}
				Destroy(lineTemplate);
			}
		}
		#endregion

		public void OnPointerDown(MixedRealityPointerEventData eventData)
		{
			throw new NotImplementedException();
		}

		public void OnPointerDragged(MixedRealityPointerEventData eventData)
		{
			throw new NotImplementedException();
		}

		public void OnPointerUp(MixedRealityPointerEventData eventData)
		{
			throw new NotImplementedException();
		}

		public void OnPointerClicked(MixedRealityPointerEventData eventData)
		{
			throw new NotImplementedException();
		}
	}
}