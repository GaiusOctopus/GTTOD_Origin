using UnityEngine;

public class ThingyWingy : MonoBehaviour
{
	public Material m_Mat;

	[Range(0.003f, 1f)]
	public float m_Succ = 0.1f;

	[Range(-10f, -0.75f)]
	public float m_Distortion = -2f;

	private float m_MouseX;

	private float m_MouseY;

	private bool m_TraceMouse;

	private int m_ID_Center;

	private int m_ID_DarkRange;

	private int m_ID_Distortion;

	private void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
		}
		m_MouseX = (m_MouseY = 0.5f);
		m_ID_Center = Shader.PropertyToID("_Center");
		m_ID_DarkRange = Shader.PropertyToID("_DarkRange");
		m_ID_Distortion = Shader.PropertyToID("_Distortion");
	}

	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		m_Mat.SetVector(m_ID_Center, new Vector4(0.5f, 0.5f, 0f, 0f));
		m_Mat.SetFloat(m_ID_DarkRange, m_Succ);
		m_Mat.SetFloat(m_ID_Distortion, m_Distortion);
		Graphics.Blit(sourceTexture, destTexture, m_Mat);
	}
}
