using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTutorial : MonoBehaviour
{
	public GameObject FadeCanvas;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			StartCoroutine(Load());
			PlayerPrefsPlus.SetBool("HasBooted", value: true);
		}
	}

	public IEnumerator Load()
	{
		Object.Instantiate(FadeCanvas);
		yield return new WaitForSeconds(1f);
		GameManager.GM.Player.GetComponent<ac_CharacterController>().FreezePlayer();
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(1);
	}
}
