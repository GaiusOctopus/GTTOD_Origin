using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VoidLoad : MonoBehaviour
{
	public Transform Player;

	public int SceneID;

	public Slider ProgressBar;

	private float ProgressValue;

	public Text LevelNumber;

	public List<GameObject> EnableObjects;

	public List<GameObject> LoadObjects;

	private AsyncOperation operation;

	private bool beginLoad;

	private bool HasLoaded;

	public void EnterVoid()
	{
		StartCoroutine(LoadAsync());
		GetComponent<Animation>().Play("VoidLoad");
		foreach (GameObject enableObject in EnableObjects)
		{
			enableObject.SetActive(value: true);
		}
	}

	private void Update()
	{
		if (beginLoad)
		{
			ProgressValue = Mathf.Lerp(ProgressValue, operation.progress, 0.01f);
			ProgressBar.value = ProgressValue;
			if (ProgressValue >= 0.85f && !HasLoaded)
			{
				HasLoaded = true;
				base.gameObject.GetComponent<Animation>().Play("VoidLoaded");
				StartCoroutine(BeginLevel());
			}
		}
	}

	private IEnumerator LoadAsync()
	{
		yield return new WaitForSeconds(6f);
		beginLoad = true;
		Application.backgroundLoadingPriority = ThreadPriority.Low;
		operation = SceneManager.LoadSceneAsync(SceneID);
		operation.allowSceneActivation = false;
		while (!operation.isDone)
		{
			yield return null;
		}
	}

	private IEnumerator BeginLevel()
	{
		yield return new WaitForSeconds(2f);
		operation.allowSceneActivation = true;
		yield return new WaitForSeconds(0.5f);
		foreach (GameObject enableObject in EnableObjects)
		{
			enableObject.SetActive(value: false);
		}
		beginLoad = false;
		HasLoaded = false;
		ProgressBar.value = 0f;
		ProgressValue = 0f;
		foreach (GameObject loadObject in LoadObjects)
		{
			loadObject.SetActive(value: true);
		}
		Player.position = new Vector3(0f, 0f, 0f);
	}

	public void CheckLevelLocation(int LevelID)
	{
		SceneID = LevelID;
		LevelNumber.text = (LevelID - 2).ToString();
	}
}
