using System.Collections;
using UnityEngine;

public class ScreenStandardization : MonoBehaviour
{
	public Material ScreenMaterial;

	private void Start()
	{
		StartCoroutine(Standardize());
	}

	private IEnumerator Standardize()
	{
		yield return new WaitForSeconds(0.5f);
		ScreenMaterial.SetTexture("_EmissionMap", ScreenMaterial.GetTexture("_MainTex"));
	}
}
