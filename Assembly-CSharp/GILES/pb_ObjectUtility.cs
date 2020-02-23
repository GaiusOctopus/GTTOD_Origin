using System;
using UnityEngine;

namespace GILES
{
	public static class pb_ObjectUtility
	{
		public static GameObject AddChild(this GameObject go)
		{
			GameObject gameObject = new GameObject();
			gameObject.transform.SetParent(go.transform);
			return gameObject;
		}

		public static Transform AddChild(this Transform trs)
		{
			Transform component = new GameObject().GetComponent<Transform>();
			component.SetParent(trs);
			return component;
		}

		public static float CalcMinDistanceToBounds(Camera cam, Bounds bounds)
		{
			return Mathf.Max(Mathf.Max(bounds.size.x, bounds.size.y), bounds.size.z) * 0.5f / Mathf.Tan(cam.fieldOfView * 0.5f * ((float)Math.PI / 180f));
		}

		public static void Destroy<T>(T obj) where T : UnityEngine.Object
		{
			UnityEngine.Object.Destroy(obj);
		}
	}
}
