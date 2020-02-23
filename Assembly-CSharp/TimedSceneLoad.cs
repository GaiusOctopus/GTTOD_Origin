using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimedSceneLoad : MonoBehaviour
{
	public Slider LoadSlider;

	public Text TipText;

	public List<string> Tips;

	private void Start()
	{
		StartCoroutine(LoadSyncOperation());
		int index = Random.Range(0, Tips.Count);
		TipText.text = Tips[index].ToString();
	}

	public IEnumerator LoadSyncOperation()
	{
		Application.backgroundLoadingPriority = ThreadPriority.High;
		yield return new WaitForSeconds(0.5f);
		AsyncOperation Operation = SceneManager.LoadSceneAsync(2);
		while (!Operation.isDone)
		{
			LoadSlider.value = Mathf.Clamp01(Operation.progress / 0.9f);
			yield return null;
		}
	}
}
