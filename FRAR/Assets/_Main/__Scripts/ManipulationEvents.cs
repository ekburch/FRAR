using DG.Tweening;
using FRAR.Utils;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControlTypes;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace FRAR
{
	public class ManipulationEvents : MonoBehaviour, IMixedRealityPointerHandler, IMixedRealityFocusHandler
	{
		private Quaternion initialRotationOnGrabStart;
		private Vector3 initialPositionOnGrabStart;
		private Vector3 initialGrabPoint;
		private Vector3 currentGrabPoint;
		private Vector3 grabPointInPointer;
		private Vector3 currentRotationAxis;

		[Header("Set in inspector")]
		AudioSource m_audioSource;
		[SerializeField] AudioClip m_audioClip1;
		[SerializeField] AudioClip m_audioClip2;
		[SerializeField] NeedleController needleController;
		[SerializeField] bool isUsingBoundsControl = false;
		[SerializeField] bool affectsEngineVolume = false;
		[SerializeField] AudioMixer m_engineMixer;
		//[SerializeField] Outline grabOutline;
		[SerializeField]
		[Tooltip("Check to enable frame-rate independent smoothing.")]
		private bool smoothingActive = false;

		/// <summary>
		/// Check to enable frame-rate independent smoothing.
		/// </summary>
		public bool SmoothingActive
		{
			get => smoothingActive;
			set => smoothingActive = value;
		}

		[SerializeField]
		[Range(0, 1)]
		[Tooltip("Enter amount representing amount of smoothing to apply to the rotation. Smoothing of 0 means no smoothing. Max value means no change to value.")]
		private float rotateLerpTime = 0.001f;

		/// <summary>
		/// Enter amount representing amount of smoothing to apply to the rotation. Smoothing of 0 means no smoothing. Max value means no change to value.
		/// </summary>
		public float RotateLerpTime
		{
			get => rotateLerpTime;
			set => rotateLerpTime = value;
		}

		[SerializeField]
		[Tooltip("Enable or disable constraint support of this component. When enabled, transform " +
			"changes will be post processed by the linked constraint manager.")]
		private bool enableConstraints = true;
		/// <summary>
		/// Enable or disable constraint support of this component. When enabled, transform
		/// changes will be post processed by the linked constraint manager.
		/// </summary>
		public bool EnableConstraints
		{
			get => enableConstraints;
			set => enableConstraints = value;
		}

		[SerializeField]
		[Tooltip("Constraint manager slot to enable constraints when manipulating the object.")]
		private ConstraintManager constraintsManager;
		/// <summary>
		/// Constraint manager slot to enable constraints when manipulating the object.
		/// </summary>
		public ConstraintManager ConstraintsManager
		{
			get => constraintsManager;
			set => constraintsManager = value;
		}

		[SerializeField] private UnityEvent onRotateStart = new UnityEvent();
		public UnityEvent OnRotateStart
		{
			get => onRotateStart;
			set => onRotateStart = value;
		}

		[SerializeField] private UnityEvent onRotateStop = new UnityEvent();
		public UnityEvent OnRotateStop
		{
			get => onRotateStop;
			set => onRotateStop = value;
		}

		[SerializeField] private UnityEvent m_rotationEvent = new UnityEvent();
		public UnityEvent OnRotationEvent { get => m_rotationEvent; set => m_rotationEvent = value; }

		[Header("Debug")]
		public bool isGrabbed = false;
		[Tooltip("Temporary - remove when the Statemachine is in place.")]
		public bool engineIsOn = false;
		public bool toToggle = false;
		public float prevYRotation;

		private IMixedRealityPointer currentPointer;

		private Outline outline;

		Tween highLightTween;

		// Start is called before the first frame update
		void Start()
		{
			m_audioSource = GetComponent<AudioSource>();
			if (isUsingBoundsControl) outline = GetComponent<Outline>();
			else outline = GetComponentInChildren<Outline>();
			if (outline != null) outline.enabled = false;
		}

		private void OnEnable()
		{

		}

		public void SetGrabbed(bool _isGrabbed)
		{
			isGrabbed = _isGrabbed;
		}

		public void ToggleBool(bool _toToggle)
		{
			engineIsOn = _toToggle;
		}

		void FixedUpdate()
		{
			if (isGrabbed && currentPointer != null)
				RotationEvents();
		}

		public void RotationEvents()
		{
			var initRotation = transform.rotation;
			if (currentPointer != null) currentGrabPoint = (currentPointer.Rotation * grabPointInPointer) + currentPointer.Position;
			bool isNear = currentPointer is IMixedRealityNearPointer;
			var rotationToUse = isUsingBoundsControl ? transform.position : currentRotationAxis;

			TransformFlags transformUpdated = 0;
			Vector3 initDir = Vector3.ProjectOnPlane(initialGrabPoint - transform.position, rotationToUse).normalized;
			Vector3 currentDir = Vector3.ProjectOnPlane(currentGrabPoint - transform.position, rotationToUse).normalized;
			Quaternion goal = Quaternion.FromToRotation(initDir, currentDir) * initialRotationOnGrabStart;
			MixedRealityTransform constraintRotation = MixedRealityTransform.NewRotate(goal);
			if (EnableConstraints && constraintsManager != null)
			{
				constraintsManager.ApplyRotationConstraints(ref constraintRotation, true, isNear);
			}

			if (!transformUpdated.HasFlag(TransformFlags.Rotate))
			{
				transform.rotation = smoothingActive ? Smoothing.SmoothTo(transform.rotation, goal, rotateLerpTime, Time.deltaTime) : goal;
			}

			bool isClockwise = GetRotationDirection(initialRotationOnGrabStart, goal);
			if (engineIsOn)
			{
				var amountToMoveNeedle = isClockwise ? 2 : 1;
				needleController?.HandleUserInput(amountToMoveNeedle);
				OnRotationEvent?.Invoke();
			}

			//AudioClip clip = isClockwise ? m_audioClip1 : m_audioClip2;
			//m_audioSource.PlayOneShot(m_audioClip1);

			if (affectsEngineVolume)
			{
				var volumeChange = isClockwise ? 10f : -10f;
				var sfxSources = SoundManager.Instance?.m_sfxSources;

				if (sfxSources != null)
				{
					m_engineMixer.GetFloat("EngineSFXVolume", out float mixerVol);
					mixerVol += volumeChange * Time.deltaTime;
					m_engineMixer.SetFloat("EngineSFXVolume", mixerVol);
				}
			}
			var compareRotValues = initRotation.eulerAngles.y - prevYRotation;
			float sign = Mathf.Sign(compareRotValues);

			prevYRotation = initRotation.eulerAngles.y;
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

		void DisableOutline()
		{
			if (outline != null)
			{
				outline.enabled = false;
				outline.OutlineWidth = 2f;
				outline.UpdateMaterialProperties();
				highLightTween.Pause();
				highLightTween.Rewind();
			}
		}

		private Vector3 GetRotationAxis()
		{
			var constraint = transform.GetComponent<RotationAxisConstraint>().ConstraintOnRotation;
			Vector3 eulerAngels = transform.eulerAngles;
			if (constraint.HasFlag(AxisFlags.XAxis))
			{
				eulerAngels = Vector3.right;
			}
			if (constraint.HasFlag(AxisFlags.YAxis))
			{
				eulerAngels = Vector3.up;
			}
			if (constraint.HasFlag(AxisFlags.ZAxis))
			{
				eulerAngels = Vector3.forward;
			}
			return eulerAngels;
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
			//DisableOutline();

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
				currentRotationAxis = GetRotationAxis();
				OnRotateStart?.Invoke();
				if (outline != null) outline.enabled = true;
				eventData.Use();
			}

			if (currentPointer != null)
			{
				eventData.Use();
			}
		}
		public void OnPointerDragged(MixedRealityPointerEventData eventData) 
		{
			//currentGrabPoint = (currentPointer.Rotation * grabPointInPointer) + currentPointer.Position;
			//DisableOutline();
		}

		public void OnPointerUp(MixedRealityPointerEventData eventData)
		{
			//DisableOutline();
			OnRotateStop?.Invoke();
			if (currentPointer != null && eventData.Pointer == currentPointer)
			{
				eventData.Use();
			}
			currentPointer = null;
		}

		public void OnPointerClicked(MixedRealityPointerEventData eventData) { }
	}
}