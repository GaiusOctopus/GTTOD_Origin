using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GILES.Interface
{
	public class pb_PrefabBrowser : MonoBehaviour
	{
		private List<GameObject> prefabs;

		private void Start()
		{
			prefabs = pb_ResourceManager.LoadAll<GameObject>().ToList();
			foreach (GameObject prefab in prefabs)
			{
				pb_PrefabBrowserItemButton pb_PrefabBrowserItemButton = base.transform.gameObject.AddChild().AddComponent<pb_PrefabBrowserItemButton>();
				pb_PrefabBrowserItemButton.asset = prefab;
				pb_PrefabBrowserItemButton.Initialize();
			}
		}
	}
}
