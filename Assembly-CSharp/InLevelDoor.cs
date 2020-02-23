using UnityEngine;

public class InLevelDoor : MonoBehaviour
{
	public MidDoor entrance;

	public MidDoor exit;

	public GameObject nextSection;

	public GameObject previousSection;

	public void EnterDoor(Transform target)
	{
		target.transform.position = entrance.transform.position;
		target.GetComponent<Rigidbody>().AddForce(base.transform.forward * 500f);
		target.GetComponent<InventoryScript>().OpenDoor();
		base.gameObject.GetComponent<Animation>().Play("EnterDoor");
		nextSection.SetActive(value: true);
		Invoke("DisableLastSection", 1f);
	}

	private void DisableLastSection()
	{
		previousSection.SetActive(value: false);
	}
}
