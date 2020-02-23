using System.Collections;
using UnityEngine;

public class LevelStarter : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(StartLevel());
	}

	private IEnumerator StartLevel()
	{
		yield return new WaitForSeconds(1f);
		GameManager.GM.GetComponent<LevelManager>().LevelHasLoaded();
		GameManager.GM.GetComponent<GTTODManager>().SetLevelObject(base.gameObject);
	}
}
