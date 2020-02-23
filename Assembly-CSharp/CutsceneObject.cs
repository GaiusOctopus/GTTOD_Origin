using System.Collections;
using UnityEngine;

public class CutsceneObject : MonoBehaviour
{
	[Header("Cutscene Set-Up")]
	public Camera CutsceneCam;

	public float FOVSpeed = 10f;

	public float TimeBeforeZoom = 1f;

	[Header("Cutscene Objects")]
	public GameObject Cutscene;

	public GameObject CutsceneWorldObject;

	public Transform EndPosition;

	public float CutsceneLength;

	public string MessageToSend;

	private ac_CharacterController Player;

	private GameManager GM;

	private float MainFOV;

	private bool inScene;

	private bool InRange;

	private bool ZoomingOut;

	private void Start()
	{
		GM = GameManager.GM;
		Player = GM.Player.GetComponent<ac_CharacterController>();
	}

	private void Update()
	{
		if (!inScene && InRange && (KeyBindingManager.GetKeyDown(KeyAction.Interact) || Input.GetButtonDown("X")))
		{
			inScene = true;
			StartCoroutine(CutsceneEffect());
			StartCoroutine(CutsceneFOV());
			MainFOV = PlayerPrefs.GetFloat("FieldOfView", 65f);
			CutsceneCam.fieldOfView = PlayerPrefs.GetFloat("FieldOfView", 65f);
		}
		if (!inScene)
		{
			return;
		}
		if (!ZoomingOut)
		{
			if (CutsceneCam.fieldOfView > 65f)
			{
				CutsceneCam.fieldOfView -= Time.deltaTime * FOVSpeed;
			}
		}
		else if (CutsceneCam.fieldOfView < MainFOV)
		{
			CutsceneCam.fieldOfView += Time.deltaTime * FOVSpeed / 2f;
		}
	}

	private IEnumerator CutsceneEffect()
	{
		Cutscene.SetActive(value: true);
		CutsceneWorldObject.SetActive(value: false);
		Player.CutsceneStart();
		Player.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z - 5f);
		yield return new WaitForSeconds(CutsceneLength);
		Player.CutsceneEnd(EndPosition);
		Cutscene.SetActive(value: false);
		Player.gameObject.GetComponent<InventoryScript>().PrintFancyMessage(MessageToSend);
		GM.GetComponent<GTTODManager>().ChooseAmbience(1);
	}

	private IEnumerator CutsceneFOV()
	{
		ZoomingOut = false;
		yield return new WaitForSeconds(TimeBeforeZoom);
		ZoomingOut = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			InRange = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			InRange = false;
		}
	}
}
