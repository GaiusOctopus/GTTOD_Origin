using UnityEngine;

[ExecuteInEditMode]
public class RadianBlurEffect : MonoBehaviour
{
	[Range(0.1f, 1f)]
	public float DownSample;

	public Material RadianBlur;

	[Range(0f, 1f)]
	public float BlurCenterX;

	[Range(0f, 1f)]
	public float BlurCenterY;

	[Range(0f, 0.1f)]
	public float BlurRatio;

	[Range(0f, 0.5f)]
	public float ClearDis;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (RadianBlur == null)
		{
			Graphics.Blit(source, destination);
			return;
		}
		RenderTexture temporary = RenderTexture.GetTemporary((int)((float)source.width * DownSample), (int)((float)source.height * DownSample), 0, source.format);
		Graphics.Blit(source, temporary);
		RadianBlur.SetFloat("_RadianCenterX", BlurCenterX);
		RadianBlur.SetFloat("_RadianCenterY", BlurCenterY);
		RadianBlur.SetFloat("_BlurRatio", BlurRatio);
		RadianBlur.SetFloat("_ClearDis", ClearDis);
		Graphics.Blit(temporary, destination, RadianBlur);
		RenderTexture.ReleaseTemporary(temporary);
	}
}
