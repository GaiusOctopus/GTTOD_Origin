using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GILES.Interface
{
	public class pb_PrefabBrowserItemButton : Button
	{
		private const int PREVIEW_LAYER = 31;

		private const int PreviewWidth = 256;

		private const int PreviewHeight = 256;

		public string prefabId = "";

		private static readonly Quaternion CAMERA_VIEW_ANGLE = Quaternion.Euler(30f, -30f, 0f);

		public GameObject asset;

		public float cameraRotateSpeed = 50f;

		private Quaternion cameraRotation = CAMERA_VIEW_ANGLE;

		private RawImage previewComponent;

		private bool doSpin;

		private Texture2D previewImage;

		private GameObject instance;

		private Light[] sceneLights;

		private bool[] lightWasEnabled;

		private static Camera _previewCamera = null;

		private static RenderTexture _renderTexture;

		private static Light _previewLightA = null;

		private static Light _previewLightB = null;

		private static Camera previewCamera
		{
			get
			{
				if (_previewCamera == null)
				{
					_previewCamera = new GameObject().AddComponent<Camera>();
					_previewCamera.gameObject.name = "Prefab Browser Asset Preview Camera";
					_previewCamera.cullingMask = int.MinValue;
					_previewCamera.transform.localRotation = CAMERA_VIEW_ANGLE;
					_previewCamera.gameObject.SetActive(value: false);
				}
				return _previewCamera;
			}
		}

		private static RenderTexture renderTexture
		{
			get
			{
				if (_renderTexture == null)
				{
					_renderTexture = new RenderTexture(256, 256, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
					_renderTexture.autoGenerateMips = false;
					_renderTexture.useMipMap = false;
				}
				return _renderTexture;
			}
		}

		private static Light previewLightA
		{
			get
			{
				if (_previewLightA == null)
				{
					GameObject gameObject = new GameObject();
					gameObject.name = "Asset Preview Lighting";
					gameObject.transform.localRotation = Quaternion.Euler(15f, 330f, 0f);
					_previewLightA = gameObject.AddComponent<Light>();
					_previewLightA.type = LightType.Directional;
					_previewLightA.intensity = 0.5f;
				}
				return _previewLightA;
			}
		}

		private static Light previewLightB
		{
			get
			{
				if (_previewLightB == null)
				{
					GameObject gameObject = new GameObject();
					gameObject.name = "Asset Preview Lighting";
					gameObject.transform.localRotation = Quaternion.Euler(15f, 150f, 0f);
					_previewLightB = gameObject.AddComponent<Light>();
					_previewLightB.type = LightType.Directional;
					_previewLightB.intensity = 0.5f;
				}
				return _previewLightB;
			}
		}

		protected override void Start()
		{
			base.Start();
			base.onClick.AddListener(Instantiate);
		}

		public void Initialize()
		{
			prefabId = asset.DemandComponent<pb_MetaDataComponent>().GetFileId();
			previewImage = new Texture2D(256, 256);
			if (!SetupAndRenderPreview(previewImage))
			{
				pb_ObjectUtility.Destroy(previewImage);
				previewImage = null;
			}
			base.gameObject.AddComponent<Mask>();
			base.gameObject.AddComponent<VerticalLayoutGroup>();
			Image image = base.gameObject.DemandComponent<Image>();
			image.color = pb_GUIUtility.ITEM_BACKGROUND_COLOR;
			image.sprite = null;
			GameObject gameObject = base.gameObject.AddChild();
			if (previewImage != null)
			{
				Text text = gameObject.AddComponent<Text>();
				text.font = pb_GUIUtility.DefaultFont();
				text.alignment = TextAnchor.MiddleCenter;
				text.text = asset.name;
			}
			else
			{
				Text text2 = gameObject.AddComponent<Text>();
				text2.font = pb_GUIUtility.DefaultFont();
				text2.alignment = TextAnchor.MiddleCenter;
				text2.text = asset.name;
			}
		}

		private void Update()
		{
			if (doSpin)
			{
				previewCamera.transform.RotateAround(Vector3.zero, Vector3.up, cameraRotateSpeed * Time.deltaTime);
				RenderPreview();
			}
		}

		private void Instantiate()
		{
			Camera main = Camera.main;
			Vector3 inPoint = (pb_Selection.activeGameObject == null) ? Vector3.zero : pb_Selection.activeGameObject.transform.position;
			Vector3 inNormal = (pb_Selection.activeGameObject == null) ? Vector3.up : (pb_Selection.activeGameObject.transform.localRotation * Vector3.up);
			Plane plane = new Plane(inNormal, inPoint);
			Ray ray = new Ray(main.transform.position, main.transform.forward);
			float enter = 0f;
			GameObject gameObject = (!plane.Raycast(ray, out enter)) ? pb_Scene.Instantiate(asset, Camera.main.gameObject.GetComponent<pb_SceneCamera>().SpawnPosition.position, asset.transform.rotation) : pb_Scene.Instantiate(asset, Camera.main.gameObject.GetComponent<pb_SceneCamera>().SpawnPosition.position, asset.transform.rotation);
			Undo.RegisterStates(new List<IUndo>
			{
				new UndoSelection(),
				new UndoInstantiate(gameObject)
			}, "Create new object");
			pb_Selection.SetSelection(gameObject);
			if (!(pb_Selection.activeGameObject != null))
			{
				pb_SceneCamera.Focus(gameObject);
			}
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			if (!(previewComponent == null) && SetupPreviewRender())
			{
				previewComponent.texture = renderTexture;
				doSpin = true;
			}
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			if (!(previewComponent == null))
			{
				doSpin = false;
				cameraRotation = previewCamera.transform.localRotation;
				RenderPreview();
				previewImage.ReadPixels(new Rect(0f, 0f, renderTexture.width, renderTexture.height), 0, 0);
				previewImage.Apply();
				previewComponent.texture = previewImage;
				RenderTexture.active = null;
				renderTexture.DiscardContents();
				renderTexture.Release();
				pb_ObjectUtility.Destroy(instance);
			}
		}

		private bool SetupAndRenderPreview(Texture2D texture)
		{
			if (!SetupPreviewRender())
			{
				return false;
			}
			RenderPreview();
			texture.ReadPixels(new Rect(0f, 0f, renderTexture.width, renderTexture.height), 0, 0);
			texture.Apply();
			RenderTexture.active = null;
			renderTexture.DiscardContents();
			renderTexture.Release();
			pb_ObjectUtility.Destroy(instance);
			return true;
		}

		private bool SetupPreviewRender()
		{
			if (asset.GetComponent<Renderer>() == null)
			{
				return false;
			}
			sceneLights = Object.FindObjectsOfType<Light>();
			lightWasEnabled = new bool[sceneLights.Length];
			instance = Object.Instantiate(asset, Vector3.zero, Quaternion.identity);
			Renderer component = instance.GetComponent<Renderer>();
			instance.transform.position = -component.bounds.center;
			instance.layer = 31;
			previewCamera.transform.localRotation = cameraRotation;
			pb_AssetPreview.PrepareCamera(previewCamera, instance, 256, 256);
			previewCamera.targetTexture = renderTexture;
			return true;
		}

		private void RenderPreview()
		{
			for (int i = 0; i < sceneLights.Length; i++)
			{
				lightWasEnabled[i] = sceneLights[i].enabled;
				sceneLights[i].enabled = false;
			}
			RenderTexture.active = renderTexture;
			instance.SetActive(value: true);
			previewLightA.gameObject.SetActive(value: true);
			previewLightB.gameObject.SetActive(value: true);
			previewCamera.Render();
			instance.SetActive(value: false);
			previewLightA.gameObject.SetActive(value: false);
			previewLightB.gameObject.SetActive(value: false);
			for (int j = 0; j < sceneLights.Length; j++)
			{
				sceneLights[j].enabled = lightWasEnabled[j];
			}
		}
	}
}
