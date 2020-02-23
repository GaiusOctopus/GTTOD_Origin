using System;

namespace GILES
{
	public class pb_UndoButton : pb_ToolbarButton
	{
		public override string tooltip
		{
			get
			{
				string currentUndo = Undo.GetCurrentUndo();
				if (string.IsNullOrEmpty(currentUndo))
				{
					return "Nothing to Undo";
				}
				return "Undo: " + currentUndo;
			}
		}

		protected override void Start()
		{
			base.Start();
			if (pb_ScriptableObjectSingleton<Undo>.instance.undoStackModified != null)
			{
				Undo instance = pb_ScriptableObjectSingleton<Undo>.instance;
				instance.undoStackModified = (Callback)Delegate.Combine(instance.undoStackModified, new Callback(UndoStackModified));
			}
			else
			{
				pb_ScriptableObjectSingleton<Undo>.instance.undoStackModified = UndoStackModified;
			}
			pb_Scene.AddOnLevelLoadedListener(delegate
			{
				base.interactable = false;
			});
			pb_Scene.AddOnLevelClearedListener(delegate
			{
				base.interactable = false;
			});
			UndoStackModified();
		}

		public void DoUndo()
		{
			Undo.PerformUndo();
		}

		private void UndoStackModified()
		{
			base.interactable = !string.IsNullOrEmpty(Undo.GetCurrentUndo());
		}
	}
}
