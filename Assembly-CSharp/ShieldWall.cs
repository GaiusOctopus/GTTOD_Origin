using System.Collections;
using UnityEngine;

public class ShieldWall : MonoBehaviour
{
	public Transform ShieldTransform;

	public GameObject Shield;

	public bool ShouldFollow = true;

	private Transform Target;

	private Vector3 LookPosition;

	private Quaternion LookRotation;

	private Animation Anim;

	private void Start()
	{
		Target = GameManager.GM.Player.transform;
		Anim = Shield.GetComponent<Animation>();
		StartCoroutine(ShieldLife());
		base.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
	}

	private void Update()
	{
		if (ShouldFollow)
		{
			LookPosition = Target.position - base.transform.position;
			LookPosition.y = 0f;
			LookRotation = Quaternion.LookRotation(LookPosition);
			ShieldTransform.rotation = Quaternion.Slerp(ShieldTransform.rotation, LookRotation, 0.1f);
		}
	}

	private IEnumerator ShieldLife()
	{
		yield return new WaitForSeconds(0.65f);
		ShieldTransform.gameObject.SetActive(value: true);
		yield return new WaitForSeconds(15f);
		Anim.Play("ShieldEnd");
	}
}
