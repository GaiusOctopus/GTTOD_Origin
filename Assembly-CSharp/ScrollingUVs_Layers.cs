using UnityEngine;

public class ScrollingUVs_Layers : MonoBehaviour
{
	public Vector2 uvAnimationRate = new Vector2(1f, 0f);

	public string textureName = "_MainTex";

	public Material MyMaterial;

	private Vector2 uvOffset = Vector2.zero;

	private Transform Player;

	private void Start()
	{
		Player = GameManager.GM.Player.transform;
	}

	private void LateUpdate()
	{
		uvOffset += uvAnimationRate * Time.deltaTime;
		MyMaterial.SetTextureOffset(textureName, uvOffset);
	}
}
