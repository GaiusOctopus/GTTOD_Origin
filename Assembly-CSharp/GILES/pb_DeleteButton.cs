using System.Collections.Generic;
using UnityEngine;

namespace GILES
{
	public class pb_DeleteButton : pb_ToolbarButton
	{
		public override string tooltip => "Delete Selection";

		protected override void Start()
		{
			base.Start();
			pb_Selection.AddOnSelectionChangeListener(OnSelectionChanged);
			OnSelectionChanged(null);
		}

		public void DoDelete()
		{
			Undo.RegisterStates(new IUndo[2]
			{
				new UndoDelete(pb_Selection.gameObjects),
				new UndoSelection()
			}, "Delete Selection");
			pb_Selection.Clear();
		}

		private void OnSelectionChanged(IEnumerable<GameObject> go)
		{
			base.interactable = (pb_Selection.activeGameObject != null);
		}
	}
}
