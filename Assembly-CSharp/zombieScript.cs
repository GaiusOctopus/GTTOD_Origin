using UnityEngine;

public class zombieScript : MonoBehaviour
{
	protected Animator myAnimator;

	private void Start()
	{
		myAnimator = GetComponent<Animator>();
	}

	private void Update()
	{
		myAnimator.SetFloat("speed", Input.GetAxis("Vertical"));
	}
}
