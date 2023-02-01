using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRAR
{
    public class NeedleController : MonoBehaviour
    {
		// Done because the model is weird and has weird angles
		// And the project is too dependent
		[Header("Set in inspector for each gauge")]
		[SerializeField] private float MAX_METER_ANGLE = -133;
		[SerializeField] private float MIN_METER_ANGLE = 133;

        public GameObject m_needle = default;

        [Header("Update as needed")]
        [SerializeField] private float needleSpeed = 100f;
		[SerializeField] private float currentValue = 0f;

        float initialNeedleSpeedValue;

        private void Awake()
        {
            currentValue = MIN_METER_ANGLE;
            initialNeedleSpeedValue = needleSpeed;
            SetNeedleRotation();
        }

        public void HandleUserInput(int amount)
        {
            float gaugeValue = currentValue;
            switch(amount)
            {
                case 1:
                    needleSpeed = initialNeedleSpeedValue;
                    currentValue -= needleSpeed * Time.deltaTime;
                    break;
                case 2:
                    needleSpeed = initialNeedleSpeedValue;
                    currentValue += needleSpeed * Time.deltaTime;
                    break;
                default:
                    needleSpeed = 0f;
                    currentValue = gaugeValue;
                    break;
            }

            SetNeedleRotation();
        }

        public void SetNeedleRotation()
        {
            m_needle.transform.localEulerAngles = new Vector3(0, GetNeedleRotation(), 0);
        }

		private float GetNeedleRotation()
		{
            currentValue = Mathf.Clamp(currentValue, MAX_METER_ANGLE, MIN_METER_ANGLE);
            return currentValue;
		}
	}
}