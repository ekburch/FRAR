using FRAR.Utils;
using Microsoft.MixedReality.Toolkit.Extensions.HandPhysics;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

		[SerializeField]
		private Outline _grabOutline;
		public Outline GrabOutline => _grabOutline;

		Tween highLightTween;

		protected override void Awake()
		{
			//_grabOutline = GetComponentInChildren<Outline>();
			_grabOutline.enabled = false;
		}

		protected override void OnLocationUpdated()
		{
			var traveledLength = PointOnLine.DistanceTraveledAlongPath(WayPointLocations, CurrentIndex);
			var traveledFraction = traveledLength / TotalLength;

			if (!animator.GetCurrentAnimatorStateInfo(animationLayer).IsName(animationName))
			{
				animator.Play(animationName, animationLayer);
			}
			animator.SetFloat(MotionTime, traveledFraction);
			//DisableOutline();
		}

		private void OnTriggerEnter(Collider other)
		{
			JointKinematicBody joint = other.gameObject.GetComponent<JointKinematicBody>();
			if (joint == null) { return; }
			else
			{
				IsTrackingPose = true;
				//if (_grabOutline != null)
				//{
				//	_grabOutline.enabled = true;
				//	float outlineWidth = _grabOutline.OutlineWidth;
				//	highLightTween = DOTween.To(() => outlineWidth, x => outlineWidth = x, 6, 1).SetLoops(-1, LoopType.Yoyo).OnUpdate(() => _grabOutline.OutlineWidth = outlineWidth).Play();
					HandAnimatorManager.instance?.ActivateAnimatorByName(animator.name.ToString(), true);
				//}
			}
		}

		private void OnTriggerExit(Collider other)
		{
			JointKinematicBody joint = other.gameObject.GetComponent<JointKinematicBody>();
			if (joint == null) { return; }
			else
			{
				IsTrackingPose = false;
				//if (_grabOutline != null)
				//{
				//	DisableOutline();
				//}
				HandAnimatorManager.instance?.ActivateAnimatorByName(animator.name.ToString(), false);
			}
		}

		public override void OnFocusEnter(FocusEventData eventData)
		{
			//if (outline != null) outline.enabled = true;
		}

		public override void OnFocusExit(FocusEventData eventData)
		{
			//if (outline != null) outline.enabled = false;
			//if (currentPointer != null && eventData.Pointer == currentPointer)
			//{
			//	currentPointer = null;
			//}
		}

		void DisableOutline()
		{
			if (_grabOutline != null)
			{
				_grabOutline.enabled = false;
				_grabOutline.OutlineWidth = 2f;
				_grabOutline.UpdateMaterialProperties();
				highLightTween.Pause();
				highLightTween.Rewind();
			}
		}

		public override void OnPointerDown(MixedRealityPointerEventData eventData)
		{
			if (currentPointer == null && !eventData.used)
			{
				currentPointer = eventData.Pointer;
				IsTrackingPose = true;
				HandAnimatorManager.instance?.ActivateAnimatorByName(animator.name.ToString(), true);
				eventData.Use();
			}
			if (currentPointer != null)
			{
				eventData.Use();
			}
		}

		public override void OnPointerDragged(MixedRealityPointerEventData eventData)
		{

		}

		public override void OnPointerUp(MixedRealityPointerEventData eventData)
		{
			if (currentPointer != null && eventData.Pointer == currentPointer)
			{
				IsTrackingPose = false;
				HandAnimatorManager.instance?.ActivateAnimatorByName(animator.name.ToString(), false);
				eventData.Use();
			}
			currentPointer = null;
		}

		public override void OnPointerClicked(MixedRealityPointerEventData eventData)
		{

		}
	}
}