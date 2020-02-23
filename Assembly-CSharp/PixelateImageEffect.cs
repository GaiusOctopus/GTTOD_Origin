using UnityEngine;

public class PixelateImageEffect : MonoBehaviour
{
	[Range(1f, 50f)]
	public int tileSize = 10;

	private Shader shader;

	private Material mtrl;

	private void Awake()
	{
		shader = Shader.Find("Hidden/PixelateImageEffect");
		if (!shader.isSupported)
		{
			base.enabled = false;
		}
		else
		{
			mtrl = new Material(shader);
		}
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		if (mtrl == null || mtrl.shader == null || !mtrl.shader.isSupported)
		{
			base.enabled = false;
			return;
		}
		mtrl.SetFloat("_TileSize", tileSize);
		Graphics.Blit(src, dest, mtrl, 0);
	}

	private void OnDestroy()
	{
		shader = null;
		if (mtrl != null)
		{
			Object.DestroyImmediate(mtrl);
			mtrl = null;
		}
	}
}
