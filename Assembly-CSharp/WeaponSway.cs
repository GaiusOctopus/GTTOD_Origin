using UnityEngine;

public class WeaponSway : MonoBehaviour
{
	public float swayAmount;

	public float maxSway;

	public float smoothSway;

	private Vector3 initialPosition;

	private void Start()
	{
		initialPosition = base.transform.localPosition;
	}

	private void Update()
	{
		float value = (0f - Input.GetAxis("Mouse X")) * swayAmount;
		float value2 = (0f - Input.GetAxis("Mouse Y")) * swayAmount;
		value = Mathf.Clamp(value, 0f - maxSway, maxSway);
		value2 = Mathf.Clamp(value2, 0f - maxSway, maxSway);
		Vector3 a = new Vector3(value, value2, 0f);
		base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, a + initialPosition, Time.deltaTime * smoothSway);
	}
}
