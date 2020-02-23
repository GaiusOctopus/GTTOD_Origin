using Boxophobic;
using UnityEngine;

[HelpURL("https://docs.google.com/document/d/1pIzIHIZ-cSh2ykODSZCbAPtScJ4Jpuu7lS3rNEHCLbc/edit#heading=h.hd5jt8lucuqq")]
[RequireComponent(typeof(BoxCollider))]
[ExecuteInEditMode]
public class HeightFogOverride : MonoBehaviour
{
	[BMessage("Info", "The Height Fog Global object is missing from your scene! Please add it before using the Height Fog Override component!", 5f, 0f)]
	public bool messageNoHeightFogGlobal;

	[BCategory("Settings")]
	public bool categorySettings;

	[Tooltip("Choose if the fog settings are set on game start or updated in realtime for animation purposes.")]
	public FogUpdateMode updateMode;

	[BCategory("Volume")]
	public bool categoryVolume;

	public float volumeDistanceFade = 3f;

	[Range(0f, 1f)]
	public float volumeVisibility = 0.2f;

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
	public Color fogColor = new Color(0f, 1f, 0f, 1f);

	public float fogDistanceStart;

	public float fogDistanceEnd = 30f;

	public float fogHeightStart;

	public float fogHeightEnd = 5f;

	[BCategory("Skybox")]
	public bool categotySkybox;

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
	public bool firstTime = true;

	[HideInInspector]
	public bool upgradedTo100;

	private Material localMaterial;

	private Collider volumeCollider;

	private HeightFogGlobal globalFog;

	private Camera cam;

	private bool distanceSent;

	private void Start()
	{
		if (firstTime)
		{
			base.gameObject.name = "Height Fog Override";
			firstTime = false;
		}
		volumeCollider = GetComponent<Collider>();
		volumeCollider.isTrigger = true;
		if (GameObject.Find("Height Fog Global") != null)
		{
			GameObject gameObject = GameObject.Find("Height Fog Global");
			globalFog = gameObject.GetComponent<HeightFogGlobal>();
			if (!upgradedTo100)
			{
				directionalMode = globalFog.directionalMode;
				noiseMode = globalFog.noiseMode;
				upgradedTo100 = true;
			}
			messageNoHeightFogGlobal = false;
		}
		else
		{
			messageNoHeightFogGlobal = true;
		}
		localMaterial = new Material(Shader.Find("BOXOPHOBIC/Atmospherics/Height Fog Preset"));
		localMaterial.name = "Local";
		SetLocalMaterial();
	}

	private void LateUpdate()
	{
		GetCamera();
		if (!(cam == null) && !(globalFog == null))
		{
			Vector3 position = cam.transform.position;
			Vector3 b = volumeCollider.ClosestPoint(position);
			float num = Vector3.Distance(position, b);
			if (num > volumeDistanceFade && !distanceSent)
			{
				globalFog.overrideCamToVolumeDistance = float.PositiveInfinity;
				distanceSent = true;
			}
			else if (num < volumeDistanceFade)
			{
				globalFog.overrideMaterial = localMaterial;
				globalFog.overrideCamToVolumeDistance = num;
				globalFog.overrideVolumeDistanceFade = volumeDistanceFade;
				distanceSent = false;
			}
			if (!Application.isPlaying || updateMode == FogUpdateMode.Realtime)
			{
				SetLocalMaterial();
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(fogColor.r, fogColor.g, fogColor.b, volumeVisibility);
		Gizmos.DrawCube(base.transform.position, new Vector3(base.transform.lossyScale.x, base.transform.lossyScale.y, base.transform.lossyScale.z));
		Gizmos.DrawCube(base.transform.position, new Vector3(base.transform.lossyScale.x + volumeDistanceFade * 2f, base.transform.lossyScale.y + volumeDistanceFade * 2f, base.transform.lossyScale.z + volumeDistanceFade * 2f));
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
}
