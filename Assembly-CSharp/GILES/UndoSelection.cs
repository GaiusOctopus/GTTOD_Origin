using System.Collections;
using System.Linq;
using UnityEngine;

namespace GILES
{
	public class UndoSelection : IUndo
	{
		public Hashtable RecordState()
		{
			Hashtable hashtable = new Hashtable();
			int num = 0;
			foreach (GameObject gameObject in pb_Selection.gameObjects)
			{
				hashtable.Add(num++, gameObject);
			}
			return hashtable;
		}

		public void ApplyState(Hashtable hash)
		{
			pb_Selection.SetSelection(hash.Values.Cast<GameObject>().ToList());
		}

		public void OnExitScope()
		{
		}
	}
}
