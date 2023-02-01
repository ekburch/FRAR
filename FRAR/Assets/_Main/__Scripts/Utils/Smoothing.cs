using UnityEngine;

namespace FRAR.Utils
{
    public class Smoothing
    {
		public static Vector3 SmoothTo(Vector3 source, Vector3 goal, float lerpTime, float deltaTime)
		{
			return Vector3.Lerp(source, goal, (lerpTime == 0f) ? 1f : 1f - Mathf.Pow(lerpTime, deltaTime));
		}

		public static Quaternion SmoothTo(Quaternion source, Quaternion goal, float slerpTime, float deltaTime)
		{
			return Quaternion.Slerp(source, goal, (slerpTime == 0f) ? 1f : 1f - Mathf.Pow(slerpTime, deltaTime));
		}
	}
}