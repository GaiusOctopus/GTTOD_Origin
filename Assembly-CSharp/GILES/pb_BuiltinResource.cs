using System.Collections.Generic;
using UnityEngine;

namespace GILES
{
	public static class pb_BuiltinResource
	{
		private const string REQUIRED_PATH = "Required/";

		public const string mat_UnlitVertexColor = "Material/UnlitVertexColor";

		public const string mat_UnlitVertexColorWavy = "Material/UnlitVertexColorWavy";

		public const string mat_Wireframe = "Material/Wireframe";

		public const string mat_Highlight = "Material/Highlight";

		public const string mat_HandleOpaque = "Handles/Material/HandleOpaqueMaterial";

		public const string mat_RotateHandle = "Handles/Material/HandleRotateMaterial";

		public const string mat_HandleTransparent = "Handles/Material/HandleTransparentMaterial";

		public const string mat_LightGizmo = "Gizmos/Light";

		public const string mat_CameraGizmo = "Gizmos/Camera";

		public const string mat_WeaponPickupGizmo = "Gizmos/WeaponPickup";

		public const string mat_AmmoPickupGizmo = "Gizmos/AmmoPickup";

		public const string mat_HealthPickupGizmo = "Gizmos/HealthPickup";

		public const string mat_PlayerSpawnGizmo = "Gizmos/PlayerSpawn";

		public const string mat_EnemySpawnGizmo = "Gizmos/EnemySpawn";

		public const string mesh_Cone = "Handles/Mesh/ConeMesh.asset";

		public const string img_WhiteTexture = "Image/White";

		private static Dictionary<string, Object> pool;

		static pb_BuiltinResource()
		{
			pool = new Dictionary<string, Object>();
		}

		public static void EmptyPool()
		{
			foreach (KeyValuePair<string, Object> item in pool)
			{
				pb_ObjectUtility.Destroy(item.Value);
			}
			pool.Clear();
			pool = null;
		}

		public static Material GetMaterial(string materialPath)
		{
			return LoadResource<Material>(materialPath);
		}

		public static T LoadResource<T>(string path) where T : Object
		{
			Object value = null;
			if (pool.TryGetValue(path, out value) && value != null && value is T)
			{
				return (T)value;
			}
			T val = Resources.Load<T>("Required/" + path);
			if ((Object)val == (Object)null)
			{
				Debug.LogWarning("Built-in resource \"Required/" + path + "\" not found!");
				return null;
			}
			T val2 = Object.Instantiate(val);
			pool.Add(path, val2);
			return val2;
		}

		public static T GetResource<T>(string path) where T : Object
		{
			Object value = null;
			if (pool.TryGetValue(path, out value) && value != null && value is T)
			{
				return (T)value;
			}
			T val = Resources.Load<T>("Required/" + path);
			if ((Object)val == (Object)null)
			{
				Debug.LogWarning("Built-in resource \"Required/" + path + "\" not found!");
				return null;
			}
			pool.Add(path, val);
			return val;
		}
	}
}
