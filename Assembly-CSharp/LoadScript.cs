using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScript : MonoBehaviour
{
	public GameObject LoadingScreen;

	public Slider ProgressBar;

	public List<GameObject> DisableList;

	public void LoadLevel(int sceneIndex)
	{
		StartCoroutine(LoadAsync(sceneIndex));
		foreach (GameObject disable in DisableList)
		{
			disable.SetActive(value: false);
		}
	}

	private IEnumerator LoadAsync(int sceneIndex)
	{
		yield return new WaitForSeconds(0.1f);
		LoadingScreen.SetActive(value: true);
		foreach (GameObject disable in DisableList)
		{
			disable.SetActive(value: false);
		}
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
		while (!operation.isDone)
		{
			float value = Mathf.Clamp01(operation.progress / 0.9f);
			ProgressBar.value = value;
			yield return null;
		}
	}
}
