using System.Collections;
using UnityEngine;

namespace GILES
{
	public class UndoInstantiate : IUndo
	{
		public GameObject gameObject;

		[SerializeField]
		private bool initialized;

		public UndoInstantiate(GameObject go)
		{
			gameObject = go;
			initialized = false;
		}

		public Hashtable RecordState()
		{
			Hashtable result = new Hashtable
			{
				{
					gameObject,
					initialized && gameObject.activeSelf
				}
			};
			initialized = true;
			return result;
		}

		public void ApplyState(Hashtable hash)
		{
			foreach (DictionaryEntry item in hash)
			{
				((GameObject)item.Key).SetActive((bool)item.Value);
			}
		}

		public void OnExitScope()
		{
			if (gameObject != null && !gameObject.activeSelf)
			{
				pb_ObjectUtility.Destroy(gameObject);
			}
		}
	}
}
