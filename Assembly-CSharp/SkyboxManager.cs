using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
	public Material Skybox1;

	public Material Skybox2;

	public Cubemap Reflection;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.L))
		{
			RenderSettings.skybox = Skybox2;
			RenderSettings.customReflection = Reflection;
		}
	}
}
