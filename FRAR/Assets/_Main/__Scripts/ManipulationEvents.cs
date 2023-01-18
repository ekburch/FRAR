using DG.Tweening;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

namespace FRAR
{
	public class ManipulationEvents : MonoBehaviour, IMixedRealityPointerHandler, IMixedRealityFocusHandler
	{
		private Quaternion initialRotationOnGrabStart;
		private Vector3 initialPositionOnGrabStart;
		private Vector3 initialGrabPoint;
		private Vector3 currentGrabPoint;
		private Vector3 grabPointInPointer;

		[Header("Set in inspector")]
		AudioSource m_audioSource;
		[SerializeField] AudioClip m_audioClip1;
		[SerializeField] AudioClip m_audioClip2;
		[SerializeField] bool isUsingBoundsControl = false;
		[SerializeField] NeedleController needleController;

		public bool isGrabbed = false;

		private IMixedRealityPointer currentPointer;

		private Outline outline;

		Tween highLightTween;

		// Start is called before the first frame update
		void Start()
		{
			m_audioSource = GetComponent<AudioSource>();
			if (isUsingBoundsControl) outline = GetComponent<Outline>();
			//else outline = GetComponentInChildren<Outline>();
			if (outline != null) outline.enabled = false;
		}

		private void OnEnable()
		{

		}

		public void SetGrabbed(bool _isGrabbed)
		{
			isGrabbed = _isGrabbed;
		}

		// Update is called once per frame
		void Update()
		{
			if (isGrabbed) RotationEvents();
		}

		public void RotationEvents()
		{
			var initRotation = transform.rotation;

			Vector3 initDir = Vector3.ProjectOnPlane(initialGrabPoint - transform.position, transform.position).normalized;
			Vector3 currentDir = Vector3.ProjectOnPlane(currentGrabPoint - transform.position, transform.position).normalized;
			Quaternion goal = Quaternion.FromToRotation(initDir, currentDir) * initialRotationOnGrabStart;

			bool isClockwise = GetRotationDirection(initRotation, goal);
			var amountToMoveNeedle = isClockwise ? 2 : 1;
			needleController?.HandleUserInput(amountToMoveNeedle);

			AudioClip clip = isClockwise ? m_audioClip1 : m_audioClip2;
			m_audioSource.PlayOneShot(clip);
		}

		public bool GetRotationDirection(Quaternion initialRot, Quaternion endRot)
		{
			float initY = initialRot.y;
			float endY = endRot.y;
			float clockwise = 0f;
			float counterClockwise = 0f;

			if (endY <= initY)
			{
				clockwise = initY - endY;
				counterClockwise = initY + (360 - endY);
			}
			else
			{
				clockwise = 360 - endY + initY;
				counterClockwise = initY - endY;
			}
			return clockwise <= counterClockwise;
		}

		public void OnFocusEnter(FocusEventData eventData)
		{
			if (outline != null)
			{
				outline.enabled = true;
				float outlineWidth = outline.OutlineWidth;
				highLightTween = DOTween.To(() => outlineWidth, x => outlineWidth = x, 6, 1).SetLoops(-1, LoopType.Yoyo).OnUpdate(() => outline.OutlineWidth = outlineWidth).Play();
			}
		}

		public void OnFocusExit(FocusEventData eventData)
		{
			if (outline != null)
			{
				outline.enabled = false;
				outline.OutlineWidth = 2f;
				outline.UpdateMaterialProperties();
				highLightTween.Pause();
			}

			if (currentPointer != null && eventData.Pointer == currentPointer)
			{
				currentPointer = null;
			}
		}

		public void OnPointerDown(MixedRealityPointerEventData eventData)
		{
			if (currentPointer == null && !eventData.used)
			{
				currentPointer = eventData.Pointer;
				initialGrabPoint = currentPointer.Result.Details.Point;
				currentGrabPoint = initialGrabPoint;
				initialRotationOnGrabStart = transform.rotation;
				initialPositionOnGrabStart = transform.position;
				grabPointInPointer = Quaternion.Inverse(eventData.Pointer.Rotation) * (initialGrabPoint - currentPointer.Position);
				if (outline != null) outline.enabled = true;
				eventData.Use();
			}

			if (currentPointer != null)
			{
				eventData.Use();
			}
		}
		public void OnPointerDragged(MixedRealityPointerEventData eventData) { }

		public void OnPointerUp(MixedRealityPointerEventData eventData)
		{
			if (outline != null) outline.enabled = false;
			if (currentPointer != null && eventData.Pointer == currentPointer)
			{
				eventData.Use();
			}
		}

		public void OnPointerClicked(MixedRealityPointerEventData eventData) { }
	}
}