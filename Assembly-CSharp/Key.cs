using UnityEngine;

public class Key : MonoBehaviour
{
	public Lock MyLock;

	public GameObject KeyObject;

	private bool Obtained;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !Obtained)
		{
			MyLock.Unlock();
			GameManager.GM.Player.GetComponent<InventoryScript>().PrintMessage("DOOR UNLOCKED");
			Obtained = true;
			KeyObject.SetActive(value: false);
		}
	}

	public void ResetFuntionality()
	{
		KeyObject.SetActive(value: true);
		Obtained = false;
	}
}
