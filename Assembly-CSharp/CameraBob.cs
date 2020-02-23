using UnityEngine;

public class CameraBob : MonoBehaviour
{
	private bool isBumping;

	private float YPosition;

	private float BumpY;

	private float TimeToReset;

	public void CameraBump(float BumpAmount, float BumpTime)
	{
		BumpY = BumpAmount;
		TimeToReset = BumpTime;
	}

	private void Update()
	{
		if (TimeToReset > 0f)
		{
			TimeToReset -= Time.deltaTime;
			isBumping = true;
		}
		else
		{
			TimeToReset = 0f;
			isBumping = false;
		}
		if (isBumping)
		{
			float y = Mathf.Lerp(base.transform.localPosition.y, BumpY, 0.1f);
			base.transform.localPosition = new Vector3(0f, y, 0f);
		}
		else
		{
			float y2 = Mathf.Lerp(base.transform.localPosition.y, 0f, 0.05f);
			base.transform.localPosition = new Vector3(0f, y2, 0f);
		}
	}
}
