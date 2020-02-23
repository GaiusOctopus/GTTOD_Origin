using UnityEngine;
using UnityEngine.UI;

public class PointsTransfer : MonoBehaviour
{
	public GTTODManager Manager;

	public Image PointsIndicator;

	public Text PointsUI;

	public Text PointsMultiplayerUI;

	private bool TakeTime;

	private Animation Anim;

	private void Start()
	{
		Anim = GetComponent<Animation>();
	}

	private void Update()
	{
		if (TakeTime)
		{
			if (PointsIndicator.fillAmount <= 0f)
			{
				Manager.ReduceMultiplyer();
			}
			else
			{
				PointsIndicator.fillAmount -= Time.deltaTime / 6f;
			}
		}
	}

	public void RestartUI()
	{
		if (!TakeTime)
		{
			TakeTime = true;
			Anim.Stop();
			Anim.Play("PointsPopIn");
		}
		else
		{
			Anim.Stop();
			Anim.Play("PointsFlash");
		}
		PointsIndicator.fillAmount = 1f;
		PointsMultiplayerUI.text = "X" + Manager.PointsMultiplyer.ToString();
	}

	public void DisableUI()
	{
		TakeTime = false;
		Anim.Stop();
		Anim.Play("PointsPopOut");
	}
}
