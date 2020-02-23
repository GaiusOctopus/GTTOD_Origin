using UnityEngine.SceneManagement;

namespace GILES
{
	public class pb_OpenSceneButton : pb_ToolbarButton
	{
		public string scene;

		public override string tooltip => "Open " + scene;

		public void OpenScene()
		{
			SceneManager.LoadScene(1);
		}
	}
}
