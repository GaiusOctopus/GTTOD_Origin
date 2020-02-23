using UnityEngine;
using UnityEngine.SceneManagement;

namespace GILES.Example
{
	public class pb_SceneLoader : pb_MonoBehaviourSingleton<pb_SceneLoader>
	{
		public string sceneToLoadLevelInto = "EmptyScene";

		[HideInInspector]
		[SerializeField]
		private string json;

		public override bool dontDestroyOnLoad => true;

		public static void LoadScene(string path)
		{
			pb_MonoBehaviourSingleton<pb_SceneLoader>.instance.json = pb_FileUtility.ReadFile(path);
			pb_Scene.LoadLevel(pb_MonoBehaviourSingleton<pb_SceneLoader>.instance.json);
			if (SceneManager.GetActiveScene().buildIndex == 2)
			{
				GameManager.GM.StartCustomMap();
			}
		}
	}
}
