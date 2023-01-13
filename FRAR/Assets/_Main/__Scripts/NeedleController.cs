using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRAR
{
    public class NeedleController : MonoBehaviour
    {
        private const float MAX_METER_ANGLE = -20;
        private const float MIN_METER_ANGLE = 210;

        public GameObject m_needle = default;

		[Header("Set in inspector for each gauge")]
		[SerializeField] private float currentValue = 0f;
        [SerializeField] private float maxValue = 200f;

        [Header("Set in inspector")]
        [SerializeField]
        private float needleSpeed = 100f;
		[SerializeField]
		private float minGaugeValue = 0f;
		[SerializeField]
        private float maxGaugeValue = 210f;

        // Update is called once per frame
        void Update()
        {
            if (currentValue > maxValue)
                currentValue = maxValue;
        }

        private float GetNeedleRotation()
        {
            float totalAngleSize = MIN_METER_ANGLE - MAX_METER_ANGLE;
            float needleSpeedNormalized = currentValue / maxValue;

            return MIN_METER_ANGLE - needleSpeedNormalized * totalAngleSize;
        }

        public void SetTargetValue(float value)
        {
            maxValue = value;
        }

        public void HandleUserInput(int amount)
        {
            float gaugeValue = currentValue;
            switch(amount)
            {
                case 1:
                    needleSpeed = 50f;
                    currentValue += needleSpeed * Time.deltaTime;
                    break;
                case 2:
                    needleSpeed = 20f;
                    currentValue -= needleSpeed * Time.deltaTime;
                    break;
                default:
                    needleSpeed = 0f;
                    currentValue = gaugeValue;
                    break;
            }

            SetNeedleRotation();
        }

        public void UpdateValues()
        {
            if (maxValue > currentValue)
            {
                currentValue += needleSpeed * Time.deltaTime;
                currentValue = Mathf.Clamp(currentValue, minGaugeValue, maxValue);
            }
            else if (maxValue < currentValue)
            {
                currentValue -= needleSpeed * Time.deltaTime;
                currentValue = Mathf.Clamp(currentValue, maxValue, maxGaugeValue);
            }

            SetNeedleRotation();
        }

        public void SetNeedleRotation()
        {
            m_needle.transform.localEulerAngles = new Vector3(0, 0, GetNeedleRotation());
        }
    }
}