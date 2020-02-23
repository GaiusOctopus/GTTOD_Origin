using System.Collections.Generic;
using UnityEngine;

namespace GILES
{
	public class pb_HandleUtility
	{
		public static bool ClosestPointsOnTwoLines(Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2, out Vector3 closestPointLine1, out Vector3 closestPointLine2)
		{
			closestPointLine1 = Vector3.zero;
			closestPointLine2 = Vector3.zero;
			float num = Vector3.Dot(lineVec1, lineVec1);
			float num2 = Vector3.Dot(lineVec1, lineVec2);
			float num3 = Vector3.Dot(lineVec2, lineVec2);
			float num4 = num * num3 - num2 * num2;
			if (num4 != 0f)
			{
				Vector3 rhs = linePoint1 - linePoint2;
				float num5 = Vector3.Dot(lineVec1, rhs);
				float num6 = Vector3.Dot(lineVec2, rhs);
				float d = (num2 * num6 - num5 * num3) / num4;
				float d2 = (num * num6 - num5 * num2) / num4;
				closestPointLine1 = linePoint1 + lineVec1 * d;
				closestPointLine2 = linePoint2 + lineVec2 * d2;
				return true;
			}
			return false;
		}

		public static bool PointOnLine(Ray InLineA, Ray InLineB, out Vector3 OutPointA, out Vector3 OutPointB)
		{
			return ClosestPointsOnTwoLines(InLineA.origin, InLineA.direction, InLineB.origin, InLineB.direction, out OutPointA, out OutPointB);
		}

		public static bool PointOnPlane(Ray ray, Vector3 planePosition, Vector3 planeNormal, out Vector3 hit)
		{
			return PointOnPlane(ray, new Plane(planeNormal, planePosition), out hit);
		}

		public static bool PointOnPlane(Ray ray, Plane plane, out Vector3 hit)
		{
			if (plane.Raycast(ray, out float enter))
			{
				hit = ray.GetPoint(enter);
				return true;
			}
			hit = Vector3.zero;
			return false;
		}

		private static Vector3 Mask(Vector3 vec)
		{
			return new Vector3((vec.x > 0f) ? 1f : (-1f), (vec.y > 0f) ? 1f : (-1f), (vec.z > 0f) ? 1f : (-1f));
		}

		private static float Mask(float val)
		{
			if (!(val > 0f))
			{
				return -1f;
			}
			return 1f;
		}

		public static Vector3 DirectionMask(Transform target, Vector3 rayDirection)
		{
			return -Mask(new Vector3(Vector3.Dot(rayDirection, target.right), Vector3.Dot(rayDirection, target.up), Vector3.Dot(rayDirection, target.forward)));
		}

		public static float CalcMouseDeltaSignWithAxes(Camera cam, Vector3 origin, Vector3 upDir, Vector3 rightDir, Vector2 mouseDelta)
		{
			if (Mathf.Abs(mouseDelta.magnitude) < 0.0001f)
			{
				return 1f;
			}
			Vector2 b = cam.WorldToScreenPoint(origin);
			Vector2 a = cam.WorldToScreenPoint(origin + upDir);
			Vector2 a2 = cam.WorldToScreenPoint(origin + rightDir);
			float f = Vector2.Dot(mouseDelta, a - b);
			float f2 = Vector2.Dot(mouseDelta, a2 - b);
			if (Mathf.Abs(f) > Mathf.Abs(f2))
			{
				return Mathf.Sign(f);
			}
			return Mathf.Sign(f2);
		}

		public static float CalcSignedMouseDelta(Vector2 lhs, Vector2 rhs)
		{
			float num = Vector2.Distance(lhs, rhs);
			float num2 = 1f / (float)Mathf.Min(Screen.width, Screen.height);
			if (Mathf.Abs(lhs.x - rhs.x) > Mathf.Abs(lhs.y - rhs.y))
			{
				return num * num2 * ((lhs.x - rhs.x > 0f) ? 1f : (-1f));
			}
			return num * num2 * ((lhs.y - rhs.y > 0f) ? 1f : (-1f));
		}

		public static float GetHandleSize(Vector3 position)
		{
			Camera main = Camera.main;
			if (!main)
			{
				return 1f;
			}
			Transform transform = main.transform;
			float d = Vector3.Dot(position - transform.position, main.transform.forward);
			Vector3 a = main.WorldToScreenPoint(transform.position + transform.forward * d);
			Vector3 b = main.WorldToScreenPoint(transform.position + (transform.right + transform.forward * d));
			return 1f / (a - b).magnitude;
		}

		public static Ray TransformRay(Ray ray, Transform transform)
		{
			Matrix4x4 worldToLocalMatrix = transform.worldToLocalMatrix;
			return new Ray(worldToLocalMatrix.MultiplyPoint(ray.origin), worldToLocalMatrix.MultiplyVector(ray.direction));
		}

		public static GameObject ObjectRaycast(Ray ray, IEnumerable<GameObject> objects)
		{
			float distance = float.PositiveInfinity;
			float num = float.PositiveInfinity;
			GameObject result = null;
			Bounds bounds = new Bounds(Vector3.zero, Vector3.one);
			foreach (GameObject @object in objects)
			{
				Ray ray2 = TransformRay(ray, @object.transform);
				Renderer component = @object.GetComponent<Renderer>();
				if (component != null)
				{
					if (component.bounds.IntersectRay(ray, out distance))
					{
						MeshFilter component2 = @object.GetComponent<MeshFilter>();
						if (component2 != null && component2.sharedMesh != null && MeshRaycast(component2.sharedMesh, ray2, out pb_RaycastHit hit) && hit.distance < num)
						{
							num = hit.distance;
							result = @object;
						}
					}
				}
				else
				{
					bounds.center = @object.transform.position;
					if (bounds.IntersectRay(ray, out distance) && distance < num)
					{
						num = distance;
						result = @object;
					}
				}
			}
			return result;
		}

		public static bool MeshRaycast(Mesh mesh, Ray ray, out pb_RaycastHit hit)
		{
			Vector3[] vertices = mesh.vertices;
			int[] triangles = mesh.triangles;
			float OutDistance = float.PositiveInfinity;
			Vector3 OutPoint = Vector3.zero;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				Vector3 vector = vertices[triangles[i]];
				Vector3 vector2 = vertices[triangles[i + 1]];
				Vector3 vector3 = vertices[triangles[i + 2]];
				if (pb_Geometry.RayIntersectsTriangle(ray, vector, vector2, vector3, Culling.Front, out OutDistance, out OutPoint))
				{
					hit = new pb_RaycastHit();
					hit.point = OutPoint;
					hit.distance = Vector3.Distance(hit.point, ray.origin);
					hit.normal = Vector3.Cross(vector2 - vector, vector3 - vector);
					hit.triangle = new int[3]
					{
						triangles[i],
						triangles[i + 1],
						triangles[i + 2]
					};
					return true;
				}
			}
			hit = null;
			return false;
		}

		public static float DistancePoint2DToLine(Camera cam, Vector2 mousePosition, Vector3 worldPosition1, Vector3 worldPosition2)
		{
			Vector2 v = cam.WorldToScreenPoint(worldPosition1);
			Vector2 w = cam.WorldToScreenPoint(worldPosition2);
			return DistancePointLineSegment(mousePosition, v, w);
		}

		public static float DistancePointLineSegment(Vector2 p, Vector2 v, Vector2 w)
		{
			float num = (v.x - w.x) * (v.x - w.x) + (v.y - w.y) * (v.y - w.y);
			if (num == 0f)
			{
				return Vector2.Distance(p, v);
			}
			float num2 = Vector2.Dot(p - v, w - v) / num;
			if ((double)num2 < 0.0)
			{
				return Vector2.Distance(p, v);
			}
			if ((double)num2 > 1.0)
			{
				return Vector2.Distance(p, w);
			}
			Vector2 b = v + num2 * (w - v);
			return Vector2.Distance(p, b);
		}

		public static bool PointInPolygon(Vector2[] polygon, Vector2 point)
		{
			float num = float.PositiveInfinity;
			float num2 = float.NegativeInfinity;
			float num3 = float.PositiveInfinity;
			float num4 = float.NegativeInfinity;
			for (int i = 0; i < polygon.Length; i++)
			{
				if (polygon[i].x < num)
				{
					num = polygon[i].x;
				}
				else if (polygon[i].x > num2)
				{
					num2 = polygon[i].x;
				}
				if (polygon[i].y < num3)
				{
					num3 = polygon[i].y;
				}
				else if (polygon[i].y > num4)
				{
					num4 = polygon[i].y;
				}
			}
			if (point.x < num || point.x > num2 || point.y < num3 || point.y > num4)
			{
				return false;
			}
			Vector2 p = new Vector2(num - 1f, num4 + 1f);
			int num5 = 0;
			for (int j = 0; j < polygon.Length; j += 2)
			{
				if (GetLineSegmentIntersect(p, point, polygon[j], polygon[j + 1]))
				{
					num5++;
				}
			}
			return num5 % 2 != 0;
		}

		public static bool GetLineSegmentIntersect(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
		{
			Vector2 vector = default(Vector2);
			vector.x = p1.x - p0.x;
			vector.y = p1.y - p0.y;
			Vector2 vector2 = default(Vector2);
			vector2.x = p3.x - p2.x;
			vector2.y = p3.y - p2.y;
			float num = ((0f - vector.y) * (p0.x - p2.x) + vector.x * (p0.y - p2.y)) / ((0f - vector2.x) * vector.y + vector.x * vector2.y);
			float num2 = (vector2.x * (p0.y - p2.y) - vector2.y * (p0.x - p2.x)) / ((0f - vector2.x) * vector.y + vector.x * vector2.y);
			if (num >= 0f && num <= 1f && num2 >= 0f)
			{
				return num2 <= 1f;
			}
			return false;
		}
	}
}
