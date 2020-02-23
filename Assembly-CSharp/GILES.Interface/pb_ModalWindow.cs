using UnityEngine;
using UnityEngine.UI;

namespace GILES.Interface
{
	public class pb_ModalWindow : pb_MonoBehaviourSingleton<pb_ModalWindow>
	{
		public GameObject contents;

		public Text windowTitle;

		public static void SetTitle(string title)
		{
			pb_MonoBehaviourSingleton<pb_ModalWindow>.instance.windowTitle.text = title;
		}

		public static void Show()
		{
			foreach (Transform item in pb_MonoBehaviourSingleton<pb_ModalWindow>.instance.transform)
			{
				item.gameObject.SetActive(value: true);
			}
			pb_MonoBehaviourSingleton<pb_ModalWindow>.instance.transform.SetAsLastSibling();
		}

		public static bool IsVisible()
		{
			foreach (Transform item in pb_MonoBehaviourSingleton<pb_ModalWindow>.instance.transform)
			{
				if (item.gameObject.activeSelf)
				{
					return true;
				}
			}
			return false;
		}

		public static void Hide()
		{
			foreach (Transform item in pb_MonoBehaviourSingleton<pb_ModalWindow>.instance.transform)
			{
				item.gameObject.SetActive(value: false);
			}
		}

		public static void SetContent(GameObject prefab)
		{
			foreach (Transform item in pb_MonoBehaviourSingleton<pb_ModalWindow>.instance.contents.transform)
			{
				pb_ObjectUtility.Destroy(item.gameObject);
			}
			prefab.transform.SetParent(pb_MonoBehaviourSingleton<pb_ModalWindow>.instance.contents.transform, worldPositionStays: false);
		}
	}
}
