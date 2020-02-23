using GILES.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace GILES
{
	[RequireComponent(typeof(pb_MetaDataComponent))]
	[pb_JsonIgnore]
	public class pb_Scene : pb_MonoBehaviourSingleton<pb_Scene>
	{
		public event Callback<GameObject> onObjectInstantiated;

		public event Callback onLevelLoaded;

		public event Callback onLevelCleared;

		protected override void Awake()
		{
			base.Awake();
			base.name = "Level Editor SceneGraph Root";
		}

		private void Start()
		{
			if (base.gameObject.GetComponent<pb_MetaDataComponent>() == null)
			{
				base.gameObject.AddComponent<pb_MetaDataComponent>();
			}
		}

		public static void AddOnObjectInstantiatedListener(Callback<GameObject> listener)
		{
			if (pb_MonoBehaviourSingleton<pb_Scene>.instance.onObjectInstantiated == null)
			{
				pb_MonoBehaviourSingleton<pb_Scene>.instance.onObjectInstantiated = listener;
			}
			else
			{
				pb_MonoBehaviourSingleton<pb_Scene>.instance.onObjectInstantiated += listener;
			}
		}

		public static void AddOnLevelLoadedListener(Callback listener)
		{
			if (pb_MonoBehaviourSingleton<pb_Scene>.instance.onLevelLoaded == null)
			{
				pb_MonoBehaviourSingleton<pb_Scene>.instance.onLevelLoaded = listener;
			}
			else
			{
				pb_MonoBehaviourSingleton<pb_Scene>.instance.onLevelLoaded += listener;
			}
		}

		public static void AddOnLevelClearedListener(Callback listener)
		{
			if (pb_MonoBehaviourSingleton<pb_Scene>.instance.onLevelCleared == null)
			{
				pb_MonoBehaviourSingleton<pb_Scene>.instance.onLevelCleared = listener;
			}
			else
			{
				pb_MonoBehaviourSingleton<pb_Scene>.instance.onLevelCleared += listener;
			}
		}

		public static GameObject Instantiate(GameObject original)
		{
			GameObject gameObject = Object.Instantiate(original);
			if (original.transform.parent != null)
			{
				gameObject.transform.SetParent(original.transform.parent);
			}
			else
			{
				gameObject.transform.parent = pb_MonoBehaviourSingleton<pb_Scene>.instance.transform;
			}
			pb_EditorComponentAttribute.StripEditorComponents(gameObject);
			if (pb_MonoBehaviourSingleton<pb_Scene>.instance.onObjectInstantiated != null)
			{
				pb_MonoBehaviourSingleton<pb_Scene>.instance.onObjectInstantiated(gameObject);
			}
			return gameObject;
		}

		public static GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation)
		{
			GameObject gameObject = Object.Instantiate(original, position, rotation);
			if (original.transform.parent != null)
			{
				gameObject.transform.SetParent(original.transform.parent);
			}
			else
			{
				gameObject.transform.parent = pb_MonoBehaviourSingleton<pb_Scene>.instance.transform;
			}
			pb_EditorComponentAttribute.StripEditorComponents(gameObject);
			if (pb_MonoBehaviourSingleton<pb_Scene>.instance.onObjectInstantiated != null)
			{
				pb_MonoBehaviourSingleton<pb_Scene>.instance.onObjectInstantiated(gameObject);
			}
			return gameObject;
		}

		public static string SaveLevel()
		{
			return JsonConvert.SerializeObject(new pb_SceneNode(pb_MonoBehaviourSingleton<pb_Scene>.instance.gameObject), Formatting.Indented, pb_Serialization.ConverterSettings);
		}

		public static void LoadLevel(string levelJson)
		{
			if (pb_MonoBehaviourSingleton<pb_Scene>.nullableInstance != null)
			{
				pb_MonoBehaviourSingleton<pb_Scene>.instance.Clear();
			}
			pb_Scene instance = pb_MonoBehaviourSingleton<pb_Scene>.instance;
			GameObject gameObject = JsonConvert.DeserializeObject<pb_SceneNode>(levelJson, pb_Serialization.ConverterSettings).ToGameObject();
			Transform[] array = new Transform[gameObject.transform.childCount];
			int num = 0;
			foreach (Transform item in gameObject.transform)
			{
				array[num++] = item;
			}
			Transform[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].SetParent(instance.transform);
			}
			pb_ObjectUtility.Destroy(gameObject);
			if (pb_MonoBehaviourSingleton<pb_Scene>.instance.onLevelLoaded != null)
			{
				pb_MonoBehaviourSingleton<pb_Scene>.instance.onLevelLoaded();
			}
		}

		public void Clear()
		{
			pb_Selection.Clear();
			foreach (Transform item in base.transform)
			{
				pb_ObjectUtility.Destroy(item.gameObject);
			}
			if (this.onLevelCleared != null)
			{
				this.onLevelCleared();
			}
		}

		public static List<GameObject> Children()
		{
			return pb_MonoBehaviourSingleton<pb_Scene>.instance.GetChildren(pb_MonoBehaviourSingleton<pb_Scene>.instance.transform);
		}

		private List<GameObject> GetChildren(Transform transform)
		{
			List<GameObject> list = new List<GameObject>();
			foreach (Transform item in transform)
			{
				if (item.gameObject.activeSelf)
				{
					list.Add(item.gameObject);
					list.AddRange(GetChildren(item));
				}
			}
			return list;
		}
	}
}
