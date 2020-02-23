using System.Collections;
using UnityEngine;

namespace GILES
{
	public class UndoTransform : IUndo
	{
		public Transform target;

		public UndoTransform(Transform target)
		{
			this.target = target;
		}

		public Hashtable RecordState()
		{
			return new Hashtable
			{
				{
					"target",
					target
				},
				{
					"transform",
					new pb_Transform(target)
				}
			};
		}

		public void ApplyState(Hashtable values)
		{
			target = (Transform)values["target"];
			pb_Transform pbTransform = (pb_Transform)values["transform"];
			target.SetTRS(pbTransform);
		}

		public void OnExitScope()
		{
		}
	}
}
