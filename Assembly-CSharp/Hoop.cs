using UnityEngine;

public class Hoop : MonoBehaviour
{
	public GameObject AyyLmao;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Ball")
		{
			Object.Instantiate(AyyLmao, base.transform.position, base.transform.rotation);
			GameManager.GM.RandomStats[5].IncreaseStat();
		}
	}
}
