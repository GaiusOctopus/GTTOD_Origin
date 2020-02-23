using UnityEngine;
using UnityEngine.UI;

public class RaycastAim : MonoBehaviour
{
	public GameManager GM;

	public LayerMask AvailableLayers;

	private RaycastHit Hit;

	public Transform AimPoint;

	public Transform BulletPoint;

	public Transform DefaultFollow;

	public Image RightSlider;

	public Image LeftSlider;

	public Text DisplayText;

	[HideInInspector]
	public GameObject MyLookedAtObject;

	[HideInInspector]
	public float FillAmount;

	private GTTODManager Manager;

	private float Distance;

	private float ResetTime;

	private float RandomX;

	private float RandomY;

	private void Start()
	{
		Manager = GM.GetComponent<GTTODManager>();
	}

	private void Update()
	{
		if (Distance > 5f)
		{
			if (ResetTime > 0f)
			{
				ResetTime -= Time.deltaTime;
				BulletPoint.localPosition = Vector3.Lerp(BulletPoint.localPosition, new Vector3(RandomX, RandomY, 0f), 0.15f);
			}
			else
			{
				BulletPoint.localPosition = Vector3.Lerp(BulletPoint.localPosition, new Vector3(0f, 0f, 0f), 0.2f);
			}
		}
		RightSlider.fillAmount = FillAmount;
		LeftSlider.fillAmount = FillAmount;
	}

	private void LateUpdate()
	{
		if (Physics.Raycast(base.transform.position, base.transform.forward, out Hit, 5E+10f, AvailableLayers))
		{
			if (Hit.collider.tag == "Interactable")
			{
				if (MyLookedAtObject == null)
				{
					MyLookedAtObject = Hit.collider.gameObject;
					MyLookedAtObject.SendMessage("Interacting", base.gameObject);
				}
				else if (Hit.collider.gameObject != MyLookedAtObject)
				{
					MyLookedAtObject = Hit.collider.gameObject;
					MyLookedAtObject.SendMessage("Interacting", base.gameObject);
				}
			}
			else if (MyLookedAtObject != null)
			{
				MyLookedAtObject.SendMessage("Stable");
				MyLookedAtObject = null;
			}
			Distance = Vector3.Distance(Hit.point, base.transform.position);
			if (Distance > 5f)
			{
				AimPoint.position = Hit.point;
			}
			else
			{
				AimPoint.position = DefaultFollow.position;
				BulletPoint.localPosition = new Vector3(0f, 0f, 0f);
			}
		}
		else
		{
			AimPoint.position = DefaultFollow.position;
		}
		if (MyLookedAtObject != null && !MyLookedAtObject.name.Contains("-"))
		{
			DisplayText.gameObject.SetActive(value: true);
			DisplayText.text = MyLookedAtObject.name;
		}
		else
		{
			DisplayText.gameObject.SetActive(value: false);
			DisplayText.text = "";
		}
	}

	public void Fire(float X, float Y)
	{
		ResetTime = 0.2f;
		RandomX = X;
		RandomY = Y;
	}
}
