using Boxophobic;
using UnityEngine;

[HelpURL("https://docs.google.com/document/d/1pIzIHIZ-cSh2ykODSZCbAPtScJ4Jpuu7lS3rNEHCLbc/edit#heading=h.pzat2b29j9a0")]
[DisallowMultipleComponent]
[ExecuteInEditMode]
public class HeightFogPerObject : MonoBehaviour
{
	[BMessage("Info", "The Object does not have a Mesh Renderer!", 5f, 5f)]
	public bool messageNoRenderer;

	[BMessage("Info", "Objects using multiple materials are not supported!", 5f, 5f)]
	public bool messageMultiMaterials;

	[BMessage("Info", "The Object does not have a Material assigned!", 5f, 5f)]
	public bool messageNoMaterial;

	[BMessage("Info", "Please note that the Height Fog Per Object option is EXPERIMENTAL and will not work for all transparent objects. Available in Play mode only. Please read the documentation for more!", 5f, 5f)]
	public bool messageTransparencySupport = true;

	[BCategory("Settings")]
	public bool categoryMaterial;

	public Material customFogMaterial;

	[BMessage("Info", "The is not a valid Height Fog material! Please assign the correct shader first!", 5f, 0f)]
	public bool messageInvalidFogMaterial;

	private int transparencyRenderQueue = 3002;

	private Material originalMaterial;

	private Material instanceMaterial;

	private Material transparencyMaterial;

	private GameObject transparencyGO;

	private void Awake()
	{
		if (GameObjectIsInvalid())
		{
			return;
		}
		transparencyGO = new GameObject(base.gameObject.name + " (Height Fog Object)");
		transparencyGO.transform.parent = base.gameObject.transform;
		transparencyGO.transform.localPosition = Vector3.zero;
		transparencyGO.transform.localRotation = Quaternion.identity;
		transparencyGO.transform.localScale = Vector3.one;
		transparencyGO.AddComponent<MeshFilter>();
		transparencyGO.AddComponent<MeshRenderer>();
		transparencyGO.GetComponent<MeshFilter>().sharedMesh = base.gameObject.GetComponent<MeshFilter>().sharedMesh;
		Material sharedMaterial = base.gameObject.GetComponent<MeshRenderer>().sharedMaterial;
		instanceMaterial = new Material(sharedMaterial);
		instanceMaterial.name = sharedMaterial.name + " (Instance)";
		if (customFogMaterial == null)
		{
			transparencyMaterial = new Material(instanceMaterial);
			transparencyMaterial.shader = Shader.Find("BOXOPHOBIC/Atmospherics/Height Fog Per Object");
			transparencyMaterial.name = sharedMaterial.name + " (Generic Fog)";
		}
		else if (customFogMaterial != null)
		{
			if (customFogMaterial.HasProperty("_IsHeightFogShader"))
			{
				transparencyMaterial = customFogMaterial;
				transparencyMaterial.name = sharedMaterial.name + " (Custom Fog)";
			}
			else
			{
				transparencyMaterial = new Material(instanceMaterial);
				transparencyMaterial.shader = Shader.Find("BOXOPHOBIC/Atmospherics/Height Fog Per Object");
				transparencyMaterial.name = sharedMaterial.name + " (Generic Fog)";
			}
		}
		if (transparencyMaterial.HasProperty("_IsStandardPipeline"))
		{
			transparencyRenderQueue = 3002;
		}
		else
		{
			transparencyRenderQueue = 3102;
		}
		instanceMaterial.renderQueue = transparencyRenderQueue;
		transparencyMaterial.renderQueue = transparencyRenderQueue;
		base.gameObject.GetComponent<MeshRenderer>().material = instanceMaterial;
		transparencyGO.GetComponent<MeshRenderer>().material = transparencyMaterial;
	}

	private bool GameObjectIsInvalid()
	{
		bool result = false;
		if (base.gameObject.GetComponent<MeshRenderer>() == null)
		{
			messageNoRenderer = true;
			result = true;
		}
		else if (base.gameObject.GetComponent<MeshRenderer>().sharedMaterials.Length > 1)
		{
			messageMultiMaterials = true;
			result = true;
		}
		else if (base.gameObject.GetComponent<MeshRenderer>().sharedMaterial == null)
		{
			messageNoMaterial = true;
			result = true;
		}
		return result;
	}
}
