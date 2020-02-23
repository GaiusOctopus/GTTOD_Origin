using UnityEngine;

public class BlurController : MonoBehaviour
{
	public float BlurRadius;

	public Material BlurMaterial;

	private void Update()
	{
		BlurMaterial.SetFloat("_Size", BlurRadius);
	}
}
