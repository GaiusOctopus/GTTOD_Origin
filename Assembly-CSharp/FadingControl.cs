using UnityEngine;

public class FadingControl : MonoBehaviour
{
	public Transform Player;

	public float m_HaloBloom = 1f;

	public Texture2D m_SecondaryTex;

	public Texture2D m_StripeTex;

	public Color m_StripeColor = Color.green;

	public float m_StripeBloom = 3f;

	public float m_Blend2TexBloom = 3f;

	private Renderer m_Rd;

	private Material[] mats;

	public void Start()
	{
		m_Rd = GetComponent<Renderer>();
		mats = m_Rd.materials;
	}

	public void UpdateSelfParameters()
	{
		for (int i = 0; i < mats.Length; i++)
		{
			mats[i].SetFloat("_HaloBloom", m_HaloBloom);
			mats[i].SetTexture("_SecondaryTex", m_SecondaryTex);
			mats[i].SetTexture("_StripeTex", m_StripeTex);
			mats[i].SetColor("_StripeColor", m_StripeColor);
			mats[i].SetFloat("_Blend2TexBloom", m_Blend2TexBloom);
			mats[i].SetFloat("_StripeBloom", m_StripeBloom);
		}
	}

	private void Update()
	{
		SetMaterialsVector("_Pos", Player.position);
	}

	public void SetMaterialsFloat(string name, float f)
	{
		Material[] materials = m_Rd.materials;
		for (int i = 0; i < materials.Length; i++)
		{
			materials[i].SetFloat(name, f);
		}
	}

	public void SetMaterialsVector(string name, Vector4 v)
	{
		Material[] materials = m_Rd.materials;
		for (int i = 0; i < materials.Length; i++)
		{
			materials[i].SetVector(name, v);
		}
	}

	public void ApplyKeyword(string keyword, bool enable)
	{
		Material[] materials = m_Rd.materials;
		for (int i = 0; i < materials.Length; i++)
		{
			if (enable)
			{
				materials[i].EnableKeyword(keyword);
			}
			else
			{
				materials[i].DisableKeyword(keyword);
			}
		}
	}

	public void ApplyShader(Shader sdr)
	{
		Material[] materials = m_Rd.materials;
		for (int i = 0; i < materials.Length; i++)
		{
			materials[i].shader = sdr;
		}
	}
}
