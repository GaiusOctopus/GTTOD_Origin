using UnityEngine;

public class GodPlayer : MonoBehaviour
{
	public float MovementSpeed;

	private Transform Player;

	private float MaxSpeed;

	private float Sensitivity = 2f;

	private float Speed = 1f;

	private float rotationX;

	private float rotationY;

	private float Invert;

	private bool InvertControls;

	private void Start()
	{
		Player = GameManager.GM.Player.transform;
		Sensitivity = PlayerPrefs.GetFloat("Sensitivity", 20f) / 10f;
		InvertControls = PlayerPrefsPlus.GetBool("Invert", defaultValue: false);
		if (InvertControls)
		{
			Invert = -1f;
		}
		else
		{
			Invert = 1f;
		}
	}

	private void Update()
	{
		Player.position = base.transform.position;
		if (KeyBindingManager.GetKey(KeyAction.Forward) || KeyBindingManager.GetKey(KeyAction.Backward) || KeyBindingManager.GetKey(KeyAction.Left) || KeyBindingManager.GetKey(KeyAction.Right) || KeyBindingManager.GetKey(KeyAction.Jump) || KeyBindingManager.GetKey(KeyAction.Crouch))
		{
			Speed = Mathf.Lerp(Speed, MaxSpeed, 0.1f);
		}
		else
		{
			Speed = Mathf.Lerp(Speed, 1f, 0.15f);
		}
		if (KeyBindingManager.GetKey(KeyAction.Dash))
		{
			MaxSpeed = MovementSpeed * 2f;
		}
		else
		{
			MaxSpeed = MovementSpeed;
		}
		if (KeyBindingManager.GetKey(KeyAction.Forward))
		{
			base.transform.Translate(Vector3.forward * Time.deltaTime * Speed);
		}
		if (KeyBindingManager.GetKey(KeyAction.Backward))
		{
			base.transform.Translate(Vector3.forward * -1f * Time.deltaTime * Speed);
		}
		if (KeyBindingManager.GetKey(KeyAction.Left))
		{
			base.transform.Translate(Vector3.right * -1f * Time.deltaTime * Speed);
		}
		if (KeyBindingManager.GetKey(KeyAction.Right))
		{
			base.transform.Translate(Vector3.right * Time.deltaTime * Speed);
		}
		if (KeyBindingManager.GetKey(KeyAction.Jump))
		{
			base.transform.Translate(Vector3.up * Time.deltaTime * Speed);
		}
		if (KeyBindingManager.GetKey(KeyAction.Crouch))
		{
			base.transform.Translate(Vector3.up * -1f * Time.deltaTime * Speed);
		}
		rotationX += Input.GetAxis("Mouse X") * Sensitivity;
		rotationY += (0f - Input.GetAxis("Mouse Y")) * Sensitivity * Invert;
		base.transform.rotation = Quaternion.Euler(rotationY, rotationX, base.transform.rotation.z);
	}
}
