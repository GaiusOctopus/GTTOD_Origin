using UnityEngine;

public static class IKSolver
{
	public static void Solve(bool debug, float targetWeight, float rotationWeight, float hintWeight, float minReach, float maxReach, Transform startLimb, Transform middleLimb, Transform endLimb, Vector3 limbForward, Vector3 targetPos, Vector3 hintPos, Quaternion targetRot)
	{
		if (!startLimb || !middleLimb || !endLimb)
		{
			return;
		}
		Vector3 vector = targetPos - startLimb.position;
		Vector3 vector2 = middleLimb.position - startLimb.position;
		Quaternion rotation = startLimb.rotation;
		Quaternion rotation2 = middleLimb.rotation;
		if (minReach > 0f && vector.magnitude < minReach)
		{
			vector = vector.normalized * minReach;
		}
		if (maxReach > 0f && maxReach > minReach && vector.magnitude > maxReach)
		{
			vector = vector.normalized * maxReach;
		}
		targetPos = startLimb.position + vector;
		if (hintWeight != 0f)
		{
			Vector3 vector3 = Vector3.Cross(vector, startLimb.rotation * limbForward);
			Vector3 vector4 = Vector3.Cross(vector, hintPos - startLimb.position);
			float angle = Vector3.Angle(vector3, vector4);
			Vector3 axis = Vector3.Cross(vector3, vector4);
			startLimb.Rotate(axis, angle, Space.World);
			if (hintWeight != 1f)
			{
				startLimb.rotation = Quaternion.Lerp(rotation, startLimb.rotation, hintWeight);
			}
			rotation = startLimb.rotation;
		}
		if (targetWeight != 0f)
		{
			float angle = Vector3.Angle(vector2, vector);
			Vector3 axis = Vector3.Cross(vector2, vector);
			startLimb.Rotate(axis, angle, Space.World);
			vector = targetPos - middleLimb.position;
			vector2 = endLimb.position - middleLimb.position;
			angle = Vector3.Angle(vector2, vector);
			axis = Vector3.Cross(vector2, vector);
			middleLimb.Rotate(axis, angle, Space.World);
			float num = Vector3.Distance(startLimb.position, middleLimb.position);
			float num2 = Vector3.Distance(middleLimb.position, endLimb.position);
			float b = Mathf.Min(Vector3.Distance(startLimb.position, targetPos), num + num2 - 1E-06f);
			angle = LawOfCosToDegree(num, b, num2);
			axis = Vector3.Cross(targetPos - startLimb.position, startLimb.rotation * limbForward);
			startLimb.Rotate(axis, angle, Space.World);
			vector2 = endLimb.position - middleLimb.position;
			vector = targetPos - middleLimb.position;
			angle = Vector3.Angle(vector2, vector);
			axis = Vector3.Cross(vector2, vector);
			middleLimb.Rotate(axis, angle, Space.World);
			if (targetWeight != 1f)
			{
				startLimb.rotation = Quaternion.Lerp(rotation, startLimb.rotation, targetWeight);
				middleLimb.rotation = Quaternion.Lerp(rotation2, middleLimb.rotation, targetWeight);
			}
		}
		if (rotationWeight != 0f)
		{
			endLimb.rotation = Quaternion.Lerp(endLimb.rotation, targetRot, rotationWeight);
		}
		if (debug)
		{
			Debug.DrawLine(startLimb.position, endLimb.position, Color.blue);
			Debug.DrawLine(startLimb.position, middleLimb.position, Color.red);
			Debug.DrawLine(middleLimb.position, endLimb.position, Color.green);
		}
	}

	private static float LawOfCosToDegree(float a, float b, float c)
	{
		float num = Mathf.Acos((c * c - (a * a + b * b)) / (0f - 2f * a * b)) * 57.29578f;
		if (!float.IsNaN(num))
		{
			return num;
		}
		return 0f;
	}
}
