using UnityEngine;

public class DissolveObject : MonoBehaviour
{
	public bool DissolveOut = true;

	public Shader Shader;

	public Texture MainTexture;

	public Texture DissolveTexture;

	public Texture EdgeRamp;

	public bool HasLight;

	[ConditionalField("HasLight", null)]
	public Light Light;

	[ConditionalField("HasLight", null)]
	public float Intensity = 1f;

	private MeshRenderer ObjectRenderer;

	private float DissolveLevel = 1f;

	private void Start()
	{
		ObjectRenderer = GetComponent<MeshRenderer>();
		ObjectRenderer.material = new Material(Shader);
		ObjectRenderer.material.SetTexture("_MainTex", MainTexture);
		ObjectRenderer.material.SetTexture("_DissolveTex", DissolveTexture);
		ObjectRenderer.material.SetTexture("_EdgeAroundRamp", EdgeRamp);
		ObjectRenderer.material.SetFloat("_EdgeAround", 0.1f);
		ObjectRenderer.material.SetFloat("_EdgeAroundPower", 3f);
		ObjectRenderer.material.SetFloat("_EdgeAroundHDR", 3f);
		ObjectRenderer.material.SetFloat("_EdgeDistortion", 0f);
		if (DissolveOut)
		{
			DissolveLevel = 1f;
			Object.Destroy(base.gameObject, 5f);
		}
		else
		{
			DissolveLevel = 0f;
		}
	}

	private void Update()
	{
		if (DissolveOut)
		{
			DissolveLevel -= Time.deltaTime / 4f;
		}
		else
		{
			DissolveLevel += Time.deltaTime;
		}
		ObjectRenderer.material.SetFloat("_Progress", DissolveLevel);
		if (HasLight)
		{
			if (DissolveLevel > 0.5f && DissolveLevel <= 0.75f)
			{
				Light.intensity = Mathf.Lerp(Light.intensity, Intensity, 0.005f);
			}
			else
			{
				Light.intensity -= Time.deltaTime * 2f;
			}
		}
	}
}
