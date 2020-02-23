using UnityEngine;
using UnityEngine.SceneManagement;

namespace GILES
{
	public class pb_LoadLevel : MonoBehaviour
	{
		public void Load()
		{
			SceneManager.LoadScene(12);
		}
	}
}
