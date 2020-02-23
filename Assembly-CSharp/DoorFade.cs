using UnityEngine;

public class DoorFade : MonoBehaviour
{
	public Material DoorMaterial;

	private float Color = 1f;

	private Vector4 GlowColor;

	private void Start()
	{
		Bump();
	}

	public void Bump()
	{
		Color = 1f;
	}

	private void Update()
	{
		if (Color > 0f)
		{
			Color -= Time.deltaTime / 2f;
		}
		GlowColor = new Vector4(Color * 3.5f, Color * 0.25f, 0f, 1f);
		DoorMaterial.SetColor("_Color", new Vector4(2f, 1f, 1f, 1f));
		DoorMaterial.SetColor("_EmissionColor", GlowColor);
	}
}
