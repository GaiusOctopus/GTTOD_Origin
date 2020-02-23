using System;

namespace GILES
{
	public class pb_RedoButton : pb_ToolbarButton
	{
		public override string tooltip
		{
			get
			{
				string currentRedo = Undo.GetCurrentRedo();
				if (string.IsNullOrEmpty(currentRedo))
				{
					return "Nothing to Redo";
				}
				return "Redo: " + currentRedo;
			}
		}

		protected override void Start()
		{
			base.Start();
			if (pb_ScriptableObjectSingleton<Undo>.instance.undoStackModified != null)
			{
				Undo instance = pb_ScriptableObjectSingleton<Undo>.instance;
				instance.undoStackModified = (Callback)Delegate.Combine(instance.undoStackModified, new Callback(RedoStackModified));
			}
			else
			{
				pb_ScriptableObjectSingleton<Undo>.instance.undoStackModified = RedoStackModified;
			}
			if (pb_ScriptableObjectSingleton<Undo>.instance.redoStackModified != null)
			{
				Undo instance2 = pb_ScriptableObjectSingleton<Undo>.instance;
				instance2.redoStackModified = (Callback)Delegate.Combine(instance2.redoStackModified, new Callback(RedoStackModified));
			}
			else
			{
				pb_ScriptableObjectSingleton<Undo>.instance.redoStackModified = RedoStackModified;
			}
			pb_Scene.AddOnLevelLoadedListener(delegate
			{
				base.interactable = false;
			});
			pb_Scene.AddOnLevelClearedListener(delegate
			{
				base.interactable = false;
			});
			RedoStackModified();
		}

		public void DoRedo()
		{
			Undo.PerformRedo();
		}

		private void RedoStackModified()
		{
			base.interactable = !string.IsNullOrEmpty(Undo.GetCurrentRedo());
		}
	}
}
