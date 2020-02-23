using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GILES
{
	public class pb_AssetBundles : pb_MonoBehaviourSingleton<pb_AssetBundles>
	{
		private List<string> availableAssetBundles = new List<string>();

		private Dictionary<string, AssetBundle> loadedAssetBundles = new Dictionary<string, AssetBundle>();

		public override bool dontDestroyOnLoad => true;

		public static void RegisterAssetBundle(string path)
		{
			pb_MonoBehaviourSingleton<pb_AssetBundles>.instance._RegisterAssetBundle(pb_FileUtility.GetFullPath(path));
		}

		public static AssetBundle LoadAssetBundle(string path)
		{
			string fullPath = pb_FileUtility.GetFullPath(path);
			return pb_MonoBehaviourSingleton<pb_AssetBundles>.instance._LoadAssetBundle(fullPath);
		}

		public static AssetBundle LoadAssetBundleWithName(string name)
		{
			return pb_MonoBehaviourSingleton<pb_AssetBundles>.instance._LoadAssetBundleWithName(name);
		}

		public static bool GetAssetPath<T>(T asset, out pb_AssetBundlePath path) where T : UnityEngine.Object
		{
			return pb_MonoBehaviourSingleton<pb_AssetBundles>.instance._GetAssetPath(asset, out path);
		}

		public static T LoadAsset<T>(pb_AssetBundlePath path) where T : UnityEngine.Object
		{
			return pb_MonoBehaviourSingleton<pb_AssetBundles>.instance._LoadAsset<T>(path);
		}

		private void _RegisterAssetBundle(string full_path)
		{
			if (!availableAssetBundles.Contains(full_path))
			{
				availableAssetBundles.Add(full_path);
			}
		}

		private AssetBundle _LoadAssetBundle(string full_path)
		{
			AssetBundle value = null;
			if (!loadedAssetBundles.TryGetValue(full_path, out value))
			{
				_RegisterAssetBundle(full_path);
				value = AssetBundle.LoadFromFile(full_path);
				loadedAssetBundles.Add(full_path, value);
			}
			return value;
		}

		private void _UnloadAssetBundle(string full_path, bool unloadAllLoadedObjects)
		{
			if (loadedAssetBundles.ContainsKey(full_path))
			{
				loadedAssetBundles[full_path].Unload(unloadAllLoadedObjects);
				loadedAssetBundles.Remove(full_path);
			}
		}

		private void _UnloadAssetBundle(AssetBundle bundle)
		{
			foreach (KeyValuePair<string, AssetBundle> loadedAssetBundle in loadedAssetBundles)
			{
				if (loadedAssetBundle.Value == bundle)
				{
					loadedAssetBundles.Remove(loadedAssetBundle.Key);
					break;
				}
			}
		}

		private bool _GetAssetPath<T>(T asset, out pb_AssetBundlePath path) where T : UnityEngine.Object
		{
			foreach (string availableAssetBundle in availableAssetBundles)
			{
				try
				{
					AssetBundle bundle = _LoadAssetBundle(availableAssetBundle);
					foreach (KeyValuePair<string, UnityEngine.Object> item in LoadBundleAssetsWithPaths(bundle))
					{
						if (item.Value.GetType() == typeof(T))
						{
							path = new pb_AssetBundlePath(availableAssetBundle, item.Key);
							return true;
						}
					}
				}
				catch (Exception ex)
				{
					Debug.LogWarning("Failed loading AssetBundle: " + availableAssetBundle + "\n" + ex.ToString());
				}
			}
			path = null;
			return false;
		}

		private Dictionary<string, UnityEngine.Object> LoadBundleAssetsWithPaths(AssetBundle bundle)
		{
			Dictionary<string, UnityEngine.Object> dictionary = new Dictionary<string, UnityEngine.Object>();
			string[] allAssetNames = bundle.GetAllAssetNames();
			foreach (string text in allAssetNames)
			{
				dictionary.Add(text, bundle.LoadAsset(text));
			}
			return dictionary;
		}

		private AssetBundle _LoadAssetBundleWithName(string name)
		{
			foreach (string availableAssetBundle in availableAssetBundles)
			{
				if (string.Compare(Path.GetFileName(availableAssetBundle), name, ignoreCase: true) == 0)
				{
					return _LoadAssetBundle(availableAssetBundle);
				}
			}
			string[] assetBundle_SearchDirectories = pb_Config.AssetBundle_SearchDirectories;
			for (int i = 0; i < assetBundle_SearchDirectories.Length; i++)
			{
				string[] files = Directory.GetFiles(assetBundle_SearchDirectories[i], name, SearchOption.AllDirectories);
				if (files.Length != 0)
				{
					string[] array = files;
					foreach (string full_path in array)
					{
						try
						{
							AssetBundle assetBundle = _LoadAssetBundle(full_path);
							if (assetBundle != null)
							{
								return assetBundle;
							}
						}
						catch
						{
						}
					}
				}
			}
			return null;
		}

		private T _LoadAsset<T>(pb_AssetBundlePath path) where T : UnityEngine.Object
		{
			AssetBundle assetBundle = _LoadAssetBundleWithName(path.assetBundleName);
			if (assetBundle == null)
			{
				return null;
			}
			if (assetBundle.Contains(path.filePath))
			{
				return assetBundle.LoadAsset<T>(path.filePath);
			}
			return null;
		}
	}
}
