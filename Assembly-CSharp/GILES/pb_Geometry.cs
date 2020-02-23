using UnityEngine;

namespace GILES
{
	public static class pb_Geometry
	{
		public static bool RayIntersectsTriangle(Ray InRay, Vector3 InTriangleA, Vector3 InTriangleB, Vector3 InTriangleC, Culling cull, out float OutDistance, out Vector3 OutPoint)
		{
			OutDistance = 0f;
			OutPoint = Vector3.zero;
			Vector3 vector = InTriangleB - InTriangleA;
			Vector3 vector2 = InTriangleC - InTriangleA;
			Vector3 rhs = Vector3.Cross(InRay.direction, vector2);
			float num = Vector3.Dot(vector, rhs);
			if ((cull == Culling.Front && num < Mathf.Epsilon) || (num > 0f - Mathf.Epsilon && num < Mathf.Epsilon))
			{
				return false;
			}
			float num2 = 1f / num;
			Vector3 lhs = InRay.origin - InTriangleA;
			float num3 = Vector3.Dot(lhs, rhs) * num2;
			if (num3 < 0f || num3 > 1f)
			{
				return false;
			}
			Vector3 rhs2 = Vector3.Cross(lhs, vector);
			float num4 = Vector3.Dot(InRay.direction, rhs2) * num2;
			if (num4 < 0f || num3 + num4 > 1f)
			{
				return false;
			}
			float num5 = Vector3.Dot(vector2, rhs2) * num2;
			if (num5 > Mathf.Epsilon)
			{
				OutDistance = num5;
				OutPoint.x = num3 * InTriangleB.x + num4 * InTriangleC.x + (1f - (num3 + num4)) * InTriangleA.x;
				OutPoint.y = num3 * InTriangleB.y + num4 * InTriangleC.y + (1f - (num3 + num4)) * InTriangleA.y;
				OutPoint.z = num3 * InTriangleB.z + num4 * InTriangleC.z + (1f - (num3 + num4)) * InTriangleA.z;
				return true;
			}
			return false;
		}
	}
}
