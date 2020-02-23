using UnityEngine;

[ExecuteInEditMode]
public class RenderTextureHelper : MonoBehaviour
{
	public Camera MainCamera;

	public Camera CamCopy;

	public Material[] Materials;

	public LayerMask CullingMask;

	private void Start()
	{
		if (CamCopy == null && MainCamera != null)
		{
			SetCamera(MainCamera, createCopy: true);
		}
	}

	private void Update()
	{
		if (!(CamCopy == null) && Materials.Length != 0)
		{
			CamCopy.Render();
		}
	}

	public void SetCamera(Camera mainCamera, bool createCopy = false)
	{
		MainCamera = mainCamera;
		if (createCopy)
		{
			if (CamCopy != null)
			{
				Object.Destroy(CamCopy.gameObject);
			}
			CamCopy = new GameObject().AddComponent<Camera>();
			CamCopy.gameObject.name = "camCopy";
			CamCopy.CopyFrom(MainCamera);
			CamCopy.gameObject.hideFlags = HideFlags.DontSave;
			CamCopy.cullingMask = CullingMask;
			CamCopy.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
			CamCopy.enabled = false;
			CamCopy.transform.SetParent(MainCamera.transform);
			CamCopy.transform.localPosition = Vector3.zero;
			CamCopy.transform.localRotation = Quaternion.identity;
			Object.DestroyImmediate(CamCopy.GetComponent<RenderTextureHelper>());
		}
		UpdateRenderTexture();
	}

	public void SetCopyCamera(Camera copyCamera)
	{
		if (CamCopy != null && CamCopy != copyCamera)
		{
			Object.Destroy(CamCopy.gameObject);
		}
		CamCopy = copyCamera;
		UpdateRenderTexture();
	}

	public void UpdateRenderTexture()
	{
		if (MainCamera != null && Materials.Length != 0)
		{
			Material[] materials = Materials;
			for (int i = 0; i < materials.Length; i++)
			{
				materials[i].SetTexture("_GrabTexture", CamCopy.targetTexture);
			}
		}
	}
}
