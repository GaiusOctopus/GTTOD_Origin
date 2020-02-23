namespace GILES
{
	public class pb_NewSceneButton : pb_ToolbarButton
	{
		public override string tooltip => "Discard changes and open new scene";

		public void OpenNewScene()
		{
			pb_MonoBehaviourSingleton<pb_Scene>.instance.Clear();
		}
	}
}
