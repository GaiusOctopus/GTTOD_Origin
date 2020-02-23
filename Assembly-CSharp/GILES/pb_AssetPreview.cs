using UnityEngine;

namespace GILES
{
	public static class pb_AssetPreview
	{
		private static Camera _previewCamera;

		private static Camera previewCamera
		{
			get
			{
				if (_previewCamera == null)
				{
					GameObject gameObject = new GameObject();
					gameObject.name = "Asset Preview Camera";
					gameObject.transform.localRotation = Quaternion.Euler(30f, -30f, 0f);
					_previewCamera = gameObject.AddComponent<Camera>();
					gameObject.SetActive(value: false);
				}
				return _previewCamera;
			}
		}

		public static Texture2D GeneratePreview(Object obj, int width, int height)
		{
			Texture2D texture2D = null;
			GameObject gameObject = obj as GameObject;
			if (PrepareCamera(previewCamera, gameObject, width, height))
			{
				gameObject = Object.Instantiate(gameObject);
				gameObject.transform.position = Vector3.zero;
				RenderTexture renderTexture = RenderTexture.active = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default, 1);
				previewCamera.targetTexture = renderTexture;
				previewCamera.Render();
				texture2D = new Texture2D(width, height);
				texture2D.ReadPixels(new Rect(0f, 0f, renderTexture.width, renderTexture.height), 0, 0);
				texture2D.Apply();
				RenderTexture.ReleaseTemporary(renderTexture);
				pb_ObjectUtility.Destroy(gameObject);
			}
			return texture2D;
		}

		public static bool PrepareCamera(Camera cam, GameObject target, int width, int height)
		{
			if (target == null)
			{
				return false;
			}
			MeshFilter component = target.GetComponent<MeshFilter>();
			Bounds bounds;
			if (component != null && component.sharedMesh != null)
			{
				bounds = component.sharedMesh.bounds;
			}
			else
			{
				Renderer component2 = target.GetComponent<Renderer>();
				if (component2 == null)
				{
					return false;
				}
				bounds = component2.bounds;
			}
			float num = pb_ObjectUtility.CalcMinDistanceToBounds(cam, bounds);
			cam.transform.position = cam.transform.forward * (0f - (2f + num));
			return true;
		}
	}
}
