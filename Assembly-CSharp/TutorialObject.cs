using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialObject : MonoBehaviour
{
	[Header("Tutorials")]
	public List<TutorialSegment> Tutorials;

	[Header("Set-Up")]
	public Transform TextFollower;

	public Text TutorialText;

	private SphereCollider Collider;

	private Transform Player;

	private Transform Target;

	private AudioSource Audio;

	private Animation Anim;

	private int Level;

	private void Start()
	{
		Collider = GetComponent<SphereCollider>();
		Player = GameObject.FindGameObjectWithTag("Player").transform;
		Audio = GetComponent<AudioSource>();
		Anim = TextFollower.GetComponent<Animation>();
		SetNextTutorial(Tutorials[Level].TargetPosition, Tutorials[Level].TutorialMessage);
	}

	private void Update()
	{
		TextFollower.LookAt(Player);
		Collider.radius = Mathf.Lerp(Collider.radius, 10f, 0.1f);
		if (Target != null)
		{
			base.transform.position = Vector3.Slerp(base.transform.position, Target.position, 0.02f);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			SetNextTutorial(Tutorials[Level].TargetPosition, Tutorials[Level].TutorialMessage);
		}
	}

	public void SetNextTutorial(Transform Location, string Message)
	{
		Level++;
		Target = Location;
		Collider.radius = 0.5f;
		TutorialText.text = Message.ToString();
		Anim.Play();
		Audio.Play();
	}
}
