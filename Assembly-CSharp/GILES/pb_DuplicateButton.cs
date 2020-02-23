using System.Collections.Generic;
using UnityEngine;

namespace GILES
{
	public class pb_DuplicateButton : pb_ToolbarButton
	{
		public override string tooltip => "Duplicate Selection";

		protected override void Start()
		{
			base.Start();
			pb_Selection.AddOnSelectionChangeListener(OnSelectionChanged);
			OnSelectionChanged(null);
		}

		public void DoDuplicate()
		{
			List<GameObject> list = new List<GameObject>();
			List<IUndo> list2 = new List<IUndo>
			{
				new UndoSelection()
			};
			foreach (GameObject gameObject2 in pb_Selection.gameObjects)
			{
				GameObject gameObject = pb_Scene.Instantiate(gameObject2);
				list.Add(gameObject);
				list2.Add(new UndoInstantiate(gameObject));
			}
			Undo.RegisterStates(list2, "Duplicate Object");
			pb_Selection.SetSelection(list);
		}

		private void OnSelectionChanged(IEnumerable<GameObject> go)
		{
			base.interactable = (pb_Selection.activeGameObject != null);
		}
	}
}
