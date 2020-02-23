using UnityEngine;

public class Rotate : MonoBehaviour
{
	public int speed = 5;

	public bool isLight;

	private void Update()
	{
		if (!isLight)
		{
			base.transform.Rotate((float)speed * Time.deltaTime, (float)(-speed / 2) * Time.deltaTime, (float)speed * Time.deltaTime);
		}
		else
		{
			base.transform.Rotate(0f, 0f, (float)speed * Time.deltaTime);
		}
	}
}
