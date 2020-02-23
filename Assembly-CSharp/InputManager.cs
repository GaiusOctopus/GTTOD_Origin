using UnityEngine;

public class InputManager : MonoBehaviour
{
	public float Sensitivity = 2f;

	public bool KeyboardMouse = true;

	public GameObject CamParent;

	private float rotationX;

	private float rotationY;

	private float MaxYRotation = 90f;

	private float XAccel;

	private float YAccel;

	private float XLength;

	private float YLength;

	private bool isFrozen;

	private int Invert = 1;

	private float XAccelerationTime;

	private float YAccelerationTime;

	public float ControllerAcceleration = 0.1f;

	public GameObject Player;

	public bool InvertControls;

	public bool isCrouching;

	private bool inAir;

	private float CameraTilt;

	private void Update()
	{
		UpdateMouseLook();
	}

	private void UpdateMouseLook()
	{
		if (!isFrozen)
		{
			if (KeyboardMouse)
			{
				rotationX += Input.GetAxis("Mouse X") * Sensitivity;
				rotationY += (0f - Input.GetAxis("Mouse Y")) * Sensitivity * (float)Invert;
				XAccel = 0f;
				YAccel = 0f;
			}
			else if (!KeyboardMouse)
			{
				rotationX += Input.GetAxis("Controller X") * XAccel;
				rotationY += Input.GetAxis("Controller Y") * YAccel * (float)Invert;
				if (Input.GetAxis("Controller X") >= 0.35f || Input.GetAxis("Controller X") <= -0.35f)
				{
					XAccel = Mathf.Lerp(XAccel, Sensitivity, XAccelerationTime);
					XAccelerationTime += Time.unscaledDeltaTime * ControllerAcceleration;
				}
				else
				{
					XAccel = Sensitivity * 0.35f;
					XAccelerationTime = 0f;
				}
				if (Input.GetAxis("Controller Y") >= 0.35f || Input.GetAxis("Controller Y") <= -0.35f)
				{
					YAccel = Mathf.Lerp(YAccel, Sensitivity, YAccelerationTime);
					YAccelerationTime += Time.unscaledDeltaTime * ControllerAcceleration;
				}
				else
				{
					YAccel = Sensitivity * 0.35f;
					YAccelerationTime = 0f;
				}
			}
			if (!inAir)
			{
				if (rotationY >= MaxYRotation)
				{
					rotationY = MaxYRotation;
				}
				if (0f - MaxYRotation >= rotationY)
				{
					rotationY = 0f - MaxYRotation;
				}
			}
			else if (!isCrouching)
			{
				if (rotationY > 270f)
				{
					rotationY *= -0.275f;
				}
				if (rotationY < -270f)
				{
					rotationY *= -0.275f;
				}
			}
			Player.transform.rotation = Quaternion.Euler(Player.transform.rotation.x, rotationX, Player.transform.rotation.z);
			CamParent.transform.localRotation = Quaternion.Euler(rotationY, CamParent.transform.localRotation.y, CameraTilt);
		}
		if (InvertControls)
		{
			Invert = -1;
		}
		else
		{
			Invert = 1;
		}
	}

	public void SetXRotation(float NewRotation)
	{
		rotationX = NewRotation;
	}

	public void SetYRotation(float NewRotation)
	{
		rotationY = NewRotation;
	}
}
