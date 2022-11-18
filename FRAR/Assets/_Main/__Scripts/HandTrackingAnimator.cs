using FRAR.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRAR
{
	public class HandTrackingAnimator : HandRailsBase
	{
		[Header("Animation Settings")]
		[SerializeField]
		private Animator animator;

		[SerializeField]
		private string animationName;

		[SerializeField]
		private int animationLayer = 0;

		private static readonly int MotionTime = Animator.StringToHash("MotionTime");

		protected override void OnLocationUpdated()
		{
			var traveledLength = PointOnLine.DistanceTraveledAlongPath(WayPointLocations, CurrentIndex);
			var traveledFraction = traveledLength / TotalLength;

			if (!animator.GetCurrentAnimatorStateInfo(animationLayer).IsName(animationName))
			{
				animator.Play(animationName, animationLayer);
			}
			animator.SetFloat(MotionTime, traveledFraction);
		}
	}
}