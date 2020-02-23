using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GILES
{
	public class UndoDelete : IUndo
	{
		public IEnumerable<GameObject> gameObjects;

		public UndoDelete(IEnumerable<GameObject> selection)
		{
			gameObjects = selection;
		}

		public Hashtable RecordState()
		{
			Hashtable hashtable = new Hashtable();
			int num = 0;
			foreach (GameObject gameObject in gameObjects)
			{
				gameObject.SetActive(value: false);
				hashtable.Add(num++, gameObject);
			}
			return hashtable;
		}

		public void ApplyState(Hashtable hash)
		{
			foreach (GameObject value in hash.Values)
			{
				value.SetActive(value: true);
			}
		}

		public void OnExitScope()
		{
			foreach (GameObject gameObject in gameObjects)
			{
				if (gameObject != null && !gameObject.activeSelf)
				{
					pb_ObjectUtility.Destroy(gameObject);
				}
			}
		}
	}
}
