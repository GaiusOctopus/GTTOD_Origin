using UnityEngine;

public class NecessaryObjects : MonoBehaviour
{
	public GTTODPause Pause;

	public void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}
}
