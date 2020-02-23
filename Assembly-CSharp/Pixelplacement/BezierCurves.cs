using UnityEngine;

namespace Pixelplacement
{
	public static class BezierCurves
	{
		public static Vector3 GetPoint(Vector3 startPosition, Vector3 controlPoint, Vector3 endPosition, float percentage)
		{
			percentage = Mathf.Clamp01(percentage);
			float num = 1f - percentage;
			return num * num * startPosition + 2f * num * percentage * controlPoint + percentage * percentage * endPosition;
		}

		public static Vector3 GetFirstDerivative(Vector3 startPoint, Vector3 controlPoint, Vector3 endPosition, float percentage)
		{
			percentage = Mathf.Clamp01(percentage);
			return 2f * (1f - percentage) * (controlPoint - startPoint) + 2f * percentage * (endPosition - controlPoint);
		}

		public static Vector3 GetPoint(Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent, float percentage, bool evenDistribution, int distributionSteps)
		{
			if (evenDistribution)
			{
				int num = distributionSteps + 1;
				float[] array = new float[num];
				Vector3 a = Locate(startPosition, endPosition, startTangent, endTangent, 0f);
				float num2 = 0f;
				for (int i = 1; i < num; i++)
				{
					Vector3 vector = Locate(startPosition, endPosition, startTangent, endTangent, (float)i / (float)num);
					num2 = (array[i] = num2 + Vector3.Distance(a, vector));
					a = vector;
				}
				float num3 = percentage * array[distributionSteps];
				int num4 = 0;
				int num5 = distributionSteps;
				int num6 = 0;
				while (num4 < num5)
				{
					num6 = num4 + (((num5 - num4) / 2) | 0);
					if (array[num6] < num3)
					{
						num4 = num6 + 1;
					}
					else
					{
						num5 = num6;
					}
				}
				if (array[num6] > num3)
				{
					num6--;
				}
				float num7 = array[num6];
				if (num7 == num3)
				{
					return Locate(startPosition, endPosition, startTangent, endTangent, num6 / distributionSteps);
				}
				return Locate(startPosition, endPosition, startTangent, endTangent, ((float)num6 + (num3 - num7) / (array[num6 + 1] - num7)) / (float)distributionSteps);
			}
			return Locate(startPosition, endPosition, startTangent, endTangent, percentage);
		}

		public static Vector3 GetFirstDerivative(Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent, float percentage)
		{
			percentage = Mathf.Clamp01(percentage);
			float num = 1f - percentage;
			return 3f * num * num * (startTangent - startPosition) + 6f * num * percentage * (endTangent - startTangent) + 3f * percentage * percentage * (endPosition - endTangent);
		}

		private static Vector3 Locate(Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent, float percentage)
		{
			percentage = Mathf.Clamp01(percentage);
			float num = 1f - percentage;
			return num * num * num * startPosition + 3f * num * num * percentage * startTangent + 3f * num * percentage * percentage * endTangent + percentage * percentage * percentage * endPosition;
		}
	}
}
