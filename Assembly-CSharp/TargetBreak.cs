using System.Collections;
using UnityEngine;

public class TargetBreak : MonoBehaviour
{
	public GameObject Break;

	public void Damage(float Damage)
	{
		base.gameObject.GetComponent<BoxCollider>().enabled = false;
		base.gameObject.GetComponent<MeshRenderer>().enabled = false;
		Object.Instantiate(Break, base.transform.position, base.transform.rotation);
		StartCoroutine(Respawn());
	}

	public IEnumerator Respawn()
	{
		yield return new WaitForSeconds(3f);
		base.gameObject.GetComponent<BoxCollider>().enabled = true;
		base.gameObject.GetComponent<MeshRenderer>().enabled = true;
	}
}
