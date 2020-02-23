using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GILES
{
	public class pb_ResourceManager : pb_ScriptableObjectSingleton<pb_ResourceManager>
	{
		private Dictionary<string, GameObject> lookup = new Dictionary<string, GameObject>();

		protected override void OnEnable()
		{
			base.OnEnable();
			string[] resource_Folder_Paths = pb_Config.Resource_Folder_Paths;
			for (int i = 0; i < resource_Folder_Paths.Length; i++)
			{
				GameObject[] array = (from x in Resources.LoadAll(resource_Folder_Paths[i])
					select (!(x is GameObject)) ? null : ((GameObject)x) into y
					where y != null
					select y).ToArray();
				for (int j = 0; j < array.Length; j++)
				{
					lookup.Add(array[j].GetComponent<pb_MetaDataComponent>().metadata.fileId, array[j]);
				}
				Resources.UnloadUnusedAssets();
			}
		}

		public static GameObject LoadPrefabWithId(string fileId)
		{
			if (string.IsNullOrEmpty(fileId))
			{
				return null;
			}
			GameObject value = null;
			if (pb_ScriptableObjectSingleton<pb_ResourceManager>.instance.lookup.TryGetValue(fileId, out value))
			{
				return value;
			}
			return null;
		}

		public static GameObject LoadPrefabWithMetadata(pb_MetaData metadata)
		{
			if (metadata.assetType == AssetType.Instance)
			{
				Debug.LogWarning("Attempting to load instance asset through resource manager.");
				return null;
			}
			switch (metadata.assetType)
			{
			case AssetType.Resource:
				if (pb_ScriptableObjectSingleton<pb_ResourceManager>.instance.lookup.ContainsKey(metadata.fileId))
				{
					return pb_ScriptableObjectSingleton<pb_ResourceManager>.instance.lookup[metadata.fileId];
				}
				Debug.LogWarning("Resource manager could not find \"" + metadata.fileId + "\" in loaded resources.");
				return null;
			case AssetType.Bundle:
				return pb_AssetBundles.LoadAsset<GameObject>(metadata.assetBundlePath);
			default:
				Debug.LogError("File not found from metadata: " + metadata);
				return null;
			}
		}

		public static IEnumerable<T> LoadAll<T>() where T : Object
		{
			List<T> list = new List<T>();
			string[] resource_Folder_Paths = pb_Config.Resource_Folder_Paths;
			foreach (string path in resource_Folder_Paths)
			{
				list.AddRange(Resources.LoadAll<T>(path));
			}
			resource_Folder_Paths = pb_Config.AssetBundle_Names;
			foreach (string name in resource_Folder_Paths)
			{
				try
				{
					AssetBundle assetBundle = pb_AssetBundles.LoadAssetBundleWithName(name);
					list.AddRange(assetBundle.LoadAllAssets<T>());
				}
				catch
				{
				}
			}
			return list;
		}
	}
}
