using UnityEngine;
using UnityEngine.UI;

public class LimitScrollView : MonoBehaviour
{
	public ScrollRect Scroller;

	public float TopLimit;

	public float BottomLimit;

	private RectTransform Content;

	private void Awake()
	{
		Content = GetComponent<RectTransform>();
		Content.localPosition = new Vector3(0f, 0f, 0f);
	}

	private void Update()
	{
		UpdateScrollRec();
	}

	private void UpdateScrollRec()
	{
		if (Content.localPosition.y < TopLimit || Content.localPosition.y > BottomLimit)
		{
			if (Content.localPosition.y < TopLimit)
			{
				Content.localPosition = new Vector3(Content.localPosition.x, TopLimit, Content.localPosition.z);
				Scroller.inertia = false;
				Scroller.vertical = false;
			}
			if (Content.localPosition.y > BottomLimit)
			{
				Content.localPosition = new Vector3(Content.localPosition.x, BottomLimit, Content.localPosition.z);
				Scroller.inertia = false;
				Scroller.vertical = false;
			}
		}
		else
		{
			Scroller.inertia = true;
			Scroller.vertical = true;
		}
	}
}
