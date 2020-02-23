using UnityEngine;

public class NextFrame : MonoBehaviour
{
	public GameObject Next;

	private float timeLeft = 0.075f;

	public bool isEnd;

	private void Update()
	{
		timeLeft -= Time.deltaTime;
		if (timeLeft < 0f && !isEnd)
		{
			Next.SetActive(value: true);
			base.gameObject.SetActive(value: false);
			timeLeft = 0.075f;
		}
		if (isEnd)
		{
			Object.Destroy(Next);
		}
	}
}
