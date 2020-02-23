using Boxophobic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
[HelpURL("https://docs.google.com/document/d/1pIzIHIZ-cSh2ykODSZCbAPtScJ4Jpuu7lS3rNEHCLbc/edit#heading=h.kfvqsi6kusw4")]
public class HeightFogGlobal : MonoBehaviour
{
	[BCategory("Update")]
	public bool categoryUpdate;

	[Tooltip("Choose if the fog settings are set on game start or updated in realtime for animation purposes.")]
	public FogUpdateMode updateMode;

	[BCategory("Fog")]
	public bool categoryFog;

	[Tooltip("Shareable fog preset material.")]
	public Material fogPreset;

	[HideInInspector]
	public Material fogPresetOld;

	[BMessage("Info", "The is not a valid Fog Preset material! Please assign the correct shader first!", 10f, 0f)]
	public bool messageInvalidPreset;

	[Range(0f, 1f)]
	[Space(10f)]
	public float fogIntensity = 1f;

	[ColorUsage(false, true)]
	public Color fogColor = new Color(0.5f, 0.75f, 1f, 1f);

	public float fogDistanceStart;

	public float fogDistanceEnd = 30f;

	public float fogHeightStart;

	public float fogHeightEnd = 5f;

	[BCategory("Skybox")]
	public bool categorySkybox;

	[Range(0f, 1f)]
	public float skyboxFogHeight = 0.5f;

	[Range(0f, 1f)]
	public float skyboxFogFill;

	[BCategory("Directional")]
	public bool categoryDirectional;

	public FogDirectionalMode directionalMode;

	[Range(0f, 1f)]
	public float directionalIntensity = 1f;

	[ColorUsage(false, true)]
	public Color directionalColor = new Color(1f, 0.75f, 0.5f, 1f);

	[BCategory("Noise")]
	public bool categoryNoise;

	public FogNoiseMode noiseMode;

	[Range(0f, 1f)]
	public float noiseIntensity = 1f;

	public float noiseDistanceEnd = 60f;

	public float noiseScale = 1f;

	public Vector3 noiseSpeed = new Vector3(0f, 0f, 0f);

	[HideInInspector]
	public Material heightFogMaterial;

	[HideInInspector]
	public Material blendMaterial;

	[HideInInspector]
	public Material localMaterial;

	[HideInInspector]
	public Material overrideMaterial;

	[HideInInspector]
	public float overrideCamToVolumeDistance = 1f;

	[HideInInspector]
	public float overrideVolumeDistanceFade;

	private Camera cam;

	private void Awake()
	{
		base.gameObject.name = "Height Fog Global";
		base.gameObject.transform.position = Vector3.zero;
		base.gameObject.transform.rotation = Quaternion.identity;
		GetCamera();
		if (cam != null)
		{
			SetFogSphereSize();
			SetFogSpherePosition();
			cam.depthTextureMode = DepthTextureMode.Depth;
		}
		GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		Mesh sharedMesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
		Object.DestroyImmediate(gameObject);
		base.gameObject.GetComponent<MeshFilter>().sharedMesh = sharedMesh;
		localMaterial = new Material(Shader.Find("BOXOPHOBIC/Atmospherics/Height Fog Preset"));
		localMaterial.name = "Local";
		SetLocalMaterial();
		overrideMaterial = new Material(localMaterial);
		overrideMaterial.name = "Override";
		blendMaterial = new Material(localMaterial);
		blendMaterial.name = "Blend";
		heightFogMaterial = new Material(Shader.Find("Hidden/BOXOPHOBIC/Atmospherics/Height Fog Global"));
		heightFogMaterial.name = "Height Fog Global";
		RenderPipelineSetTransparentQueue();
		base.gameObject.GetComponent<MeshRenderer>().sharedMaterial = heightFogMaterial;
		SetGlobalShader();
	}

	private void Update()
	{
		if (base.gameObject.name != "Height Fog Global")
		{
			base.gameObject.name = "Height Fog Global";
		}
		if (!(cam == null))
		{
			SetFogSphereSize();
			SetFogSpherePosition();
			if (overrideCamToVolumeDistance > overrideVolumeDistanceFade)
			{
				blendMaterial.CopyPropertiesFromMaterial(localMaterial);
			}
			else if (overrideCamToVolumeDistance < overrideVolumeDistanceFade)
			{
				float t = 1f - overrideCamToVolumeDistance / overrideVolumeDistanceFade;
				blendMaterial.Lerp(localMaterial, overrideMaterial, t);
			}
			if (!Application.isPlaying || updateMode == FogUpdateMode.Realtime)
			{
				SetLocalMaterial();
			}
			SetGlobalShader();
			SetGlobalKeywords();
		}
	}

	private void GetCamera()
	{
		cam = null;
		if (Camera.current != null)
		{
			cam = Camera.current;
		}
		if (Camera.main != null)
		{
			cam = Camera.main;
		}
	}

	private void SetPresetToScript()
	{
		fogIntensity = fogPreset.GetFloat("_FogIntensity");
		fogColor = fogPreset.GetColor("_FogColor");
		fogDistanceStart = fogPreset.GetFloat("_FogDistanceStart");
		fogDistanceEnd = fogPreset.GetFloat("_FogDistanceEnd");
		fogHeightStart = fogPreset.GetFloat("_FogHeightStart");
		fogHeightEnd = fogPreset.GetFloat("_FogHeightEnd");
		skyboxFogHeight = fogPreset.GetFloat("_SkyboxFogHeight");
		skyboxFogFill = fogPreset.GetFloat("_SkyboxFogFill");
		directionalColor = fogPreset.GetColor("_DirectionalColor");
		directionalIntensity = fogPreset.GetFloat("_DirectionalIntensity");
		noiseIntensity = fogPreset.GetFloat("_NoiseIntensity");
		noiseDistanceEnd = fogPreset.GetFloat("_NoiseDistanceEnd");
		noiseScale = fogPreset.GetFloat("_NoiseScale");
		noiseSpeed = fogPreset.GetVector("_NoiseSpeed");
		if (fogPreset.GetInt("_DirectionalMode") == 1)
		{
			directionalMode = FogDirectionalMode.On;
		}
		else
		{
			directionalMode = FogDirectionalMode.Off;
		}
		if (fogPreset.GetInt("_NoiseMode") == 2)
		{
			noiseMode = FogNoiseMode.Procedural3D;
		}
		else
		{
			noiseMode = FogNoiseMode.Off;
		}
	}

	private void SetLocalMaterial()
	{
		localMaterial.SetFloat("_FogIntensity", fogIntensity);
		localMaterial.SetColor("_FogColor", fogColor);
		localMaterial.SetFloat("_FogDistanceStart", fogDistanceStart);
		localMaterial.SetFloat("_FogDistanceEnd", fogDistanceEnd);
		localMaterial.SetFloat("_FogHeightStart", fogHeightStart);
		localMaterial.SetFloat("_FogHeightEnd", fogHeightEnd);
		localMaterial.SetFloat("_SkyboxFogHeight", skyboxFogHeight);
		localMaterial.SetFloat("_SkyboxFogFill", skyboxFogFill);
		localMaterial.SetFloat("_DirectionalIntensity", directionalIntensity);
		localMaterial.SetColor("_DirectionalColor", directionalColor);
		localMaterial.SetFloat("_NoiseIntensity", noiseIntensity);
		localMaterial.SetFloat("_NoiseDistanceEnd", noiseDistanceEnd);
		localMaterial.SetFloat("_NoiseScale", noiseScale);
		localMaterial.SetVector("_NoiseSpeed", noiseSpeed);
		if (directionalMode == FogDirectionalMode.On)
		{
			localMaterial.SetInt("_DirectionalMode", 1);
			localMaterial.SetFloat("_DirectionalModeBlend", 1f);
		}
		else
		{
			localMaterial.SetInt("_DirectionalMode", 0);
			localMaterial.SetFloat("_DirectionalModeBlend", 0f);
		}
		if (noiseMode == FogNoiseMode.Procedural3D)
		{
			localMaterial.SetInt("_NoiseMode", 2);
			localMaterial.SetFloat("_NoiseModeBlend", 1f);
		}
		else
		{
			localMaterial.SetInt("_NoiseMode", 0);
			localMaterial.SetFloat("_NoiseModeBlend", 0f);
		}
	}

	private void SetGlobalShader()
	{
		Shader.SetGlobalFloat("AHF_FogIntensity", blendMaterial.GetFloat("_FogIntensity"));
		Shader.SetGlobalColor("AHF_FogColor", blendMaterial.GetColor("_FogColor"));
		Shader.SetGlobalFloat("AHF_FogDistanceStart", blendMaterial.GetFloat("_FogDistanceStart"));
		Shader.SetGlobalFloat("AHF_FogDistanceEnd", blendMaterial.GetFloat("_FogDistanceEnd"));
		Shader.SetGlobalFloat("AHF_FogHeightStart", blendMaterial.GetFloat("_FogHeightStart"));
		Shader.SetGlobalFloat("AHF_FogHeightEnd", blendMaterial.GetFloat("_FogHeightEnd"));
		Shader.SetGlobalFloat("AHF_SkyboxFogHeight", blendMaterial.GetFloat("_SkyboxFogHeight"));
		Shader.SetGlobalFloat("AHF_SkyboxFogFill", blendMaterial.GetFloat("_SkyboxFogFill"));
		Shader.SetGlobalFloat("AHF_DirectionalModeBlend", blendMaterial.GetFloat("_DirectionalModeBlend"));
		Shader.SetGlobalColor("AHF_DirectionalColor", blendMaterial.GetColor("_DirectionalColor"));
		Shader.SetGlobalFloat("AHF_DirectionalIntensity", blendMaterial.GetFloat("_DirectionalIntensity"));
		Shader.SetGlobalFloat("AHF_NoiseModeBlend", blendMaterial.GetFloat("_NoiseModeBlend"));
		Shader.SetGlobalFloat("AHF_NoiseIntensity", blendMaterial.GetFloat("_NoiseIntensity"));
		Shader.SetGlobalFloat("AHF_NoiseDistanceEnd", blendMaterial.GetFloat("_NoiseDistanceEnd"));
		Shader.SetGlobalFloat("AHF_NoiseScale", blendMaterial.GetFloat("_NoiseScale"));
		Shader.SetGlobalVector("AHF_NoiseSpeed", blendMaterial.GetVector("_NoiseSpeed"));
	}

	private void SetGlobalKeywords()
	{
		if (blendMaterial.GetFloat("_DirectionalModeBlend") > 0f)
		{
			Shader.DisableKeyword("AHF_DIRECTIONALMODE_OFF");
			Shader.EnableKeyword("AHF_DIRECTIONALMODE_ON");
		}
		else
		{
			Shader.DisableKeyword("AHF_DIRECTIONALMODE_ON");
			Shader.EnableKeyword("AHF_DIRECTIONALMODE_OFF");
		}
		if (blendMaterial.GetFloat("_NoiseModeBlend") > 0f)
		{
			Shader.DisableKeyword("AHF_NOISEMODE_OFF");
			Shader.EnableKeyword("AHF_NOISEMODE_PROCEDURAL3D");
		}
		else
		{
			Shader.DisableKeyword("AHF_NOISEMODE_PROCEDURAL3D");
			Shader.EnableKeyword("AHF_NOISEMODE_OFF");
		}
	}

	private void SetFogSphereSize()
	{
		float num = cam.farClipPlane - 1f;
		base.gameObject.transform.localScale = new Vector3(num, num, num);
	}

	private void SetFogSpherePosition()
	{
		base.transform.position = cam.transform.position;
	}

	private void RenderPipelineSetTransparentQueue()
	{
		if (heightFogMaterial.HasProperty("_IsStandardPipeline"))
		{
			heightFogMaterial.renderQueue = 3001;
		}
		else
		{
			heightFogMaterial.renderQueue = 3101;
		}
	}
}
