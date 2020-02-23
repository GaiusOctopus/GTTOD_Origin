using System;

namespace GILES
{
	public class pb_PositionToolButton : pb_ToolbarButton
	{
		public override string tooltip => "Position Tool";

		protected override void Start()
		{
			base.Start();
			if (pb_MonoBehaviourSingleton<pb_SelectionHandle>.instance.onHandleTypeChanged != null)
			{
				pb_SelectionHandle instance = pb_MonoBehaviourSingleton<pb_SelectionHandle>.instance;
				instance.onHandleTypeChanged = (Callback)Delegate.Combine(instance.onHandleTypeChanged, new Callback(OnHandleChange));
			}
			else
			{
				pb_MonoBehaviourSingleton<pb_SelectionHandle>.instance.onHandleTypeChanged = OnHandleChange;
			}
			OnHandleChange();
		}

		public void DoSetHandle()
		{
			pb_MonoBehaviourSingleton<pb_SelectionHandle>.instance.SetTool(Tool.Position);
		}

		private void OnHandleChange()
		{
			base.interactable = (!pb_MonoBehaviourSingleton<pb_SelectionHandle>.instance.GetIsHidden() && pb_MonoBehaviourSingleton<pb_SelectionHandle>.instance.GetTool() != Tool.Position);
		}
	}
}
