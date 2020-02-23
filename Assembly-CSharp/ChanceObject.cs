using UnityEngine;

public class ChanceObject : MonoBehaviour
{
	public float PercentageChance = 50f;

	private void Awake()
	{
		if (Random.Range(0f, 100f) >= PercentageChance)
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
