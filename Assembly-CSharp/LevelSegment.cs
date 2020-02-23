using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSegment : MonoBehaviour
{
	[Header("Set-Up")]
	public bool ShouldSpawn = true;

	[ConditionalField("ShouldSpawn", null)]
	public bool CanStaggerHeight;

	[ConditionalField("ShouldSpawn", null)]
	public GameObject GenericPlatform;

	[ConditionalField("ShouldSpawn", null)]
	public GameObject RockPlatform;

	[ConditionalField("ShouldSpawn", null)]
	public GameObject Shield;

	[ConditionalField("ShouldSpawn", null)]
	public ParticleSystem LockEffect;

	[Header("Extra Settings")]
	[ConditionalField("ShouldSpawn", null)]
	public bool ChanceBig;

	public List<LevelSegment> NearSegments;

	[Header("Pieces")]
	public List<GameObject> GenericSegments;

	public List<GameObject> NaturalSegments;

	public List<GameObject> BigSegments;

	private Vector3 StartPosition;

	private Quaternion StartRotation;

	private Vector3 EndPosition;

	private Quaternion EndRotation;

	private bool Emerging;

	private bool Staggered;

	private bool HasStart;

	private bool Empty;

	private Transform MySegment;

	private void Start()
	{
		if (!HasStart)
		{
			StartPosition = base.transform.localPosition;
			StartRotation = base.transform.localRotation;
			HasStart = true;
		}
		if (ChanceBig)
		{
			if (Random.Range(0f, 100f) >= 50f)
			{
				int index = Random.Range(0, BigSegments.Count);
				MySegment = Object.Instantiate(BigSegments[index], base.transform.position, base.transform.rotation).transform;
				MySegment.parent = base.transform;
				GenericPlatform.SetActive(value: true);
				RockPlatform.SetActive(value: false);
				foreach (LevelSegment nearSegment in NearSegments)
				{
					nearSegment.DestroySegments();
				}
				EndPosition = base.transform.localPosition;
				EndRotation = base.transform.localRotation;
			}
			else
			{
				NormalSpawn();
			}
		}
		else
		{
			NormalSpawn();
		}
	}

	public void NormalSpawn()
	{
		if (ShouldSpawn)
		{
			GenericPlatform.SetActive(value: false);
			RockPlatform.SetActive(value: false);
			if (Random.Range(0f, 100f) >= 5f)
			{
				if (Random.Range(-1f, 1f) > 0f)
				{
					int index = Random.Range(0, GenericSegments.Count);
					MySegment = Object.Instantiate(GenericSegments[index], base.transform.position, base.transform.rotation).transform;
					MySegment.parent = base.transform;
					GenericPlatform.SetActive(value: true);
					RockPlatform.SetActive(value: false);
				}
				else
				{
					int index2 = Random.Range(0, NaturalSegments.Count);
					MySegment = Object.Instantiate(NaturalSegments[index2], base.transform.position, base.transform.rotation).transform;
					MySegment.parent = base.transform;
					GenericPlatform.SetActive(value: false);
					RockPlatform.SetActive(value: true);
				}
				if (CanStaggerHeight)
				{
					if (Random.Range(0f, 100f) <= 35f)
					{
						float num = Random.Range(1, 6) * 5;
						EndPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y + num, base.transform.localPosition.z);
						EndRotation = base.transform.localRotation;
						Staggered = true;
					}
					else
					{
						EndPosition = base.transform.localPosition;
						EndRotation = base.transform.localRotation;
					}
				}
				else
				{
					EndPosition = base.transform.localPosition;
					EndRotation = base.transform.localRotation;
				}
			}
			else
			{
				GenericPlatform.SetActive(value: false);
				RockPlatform.SetActive(value: false);
				Shield.SetActive(value: false);
				Empty = true;
			}
		}
		else
		{
			GenericPlatform.SetActive(value: true);
			RockPlatform.SetActive(value: false);
			EndPosition = base.transform.localPosition;
			EndRotation = base.transform.localRotation;
		}
		base.transform.localPosition = EndPosition;
		base.transform.localRotation = EndRotation;
	}

	public void HideSegment()
	{
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y - 150f, base.transform.localPosition.z);
		base.transform.localRotation = new Quaternion(base.transform.localRotation.x, base.transform.localRotation.y, 90f, 0f);
		Shield.SetActive(value: false);
		Emerging = true;
	}

	private void Update()
	{
		if (!Emerging)
		{
			return;
		}
		base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, EndPosition, Random.Range(150f, 75f) * 3f / Vector3.Distance(base.transform.position, GameManager.GM.Player.transform.position));
		base.transform.localRotation = Quaternion.RotateTowards(base.transform.localRotation, EndRotation, Random.Range(175f, 100f) * 3f / Vector3.Distance(base.transform.position, GameManager.GM.Player.transform.position));
		if (base.transform.localPosition == EndPosition && base.transform.localRotation == EndRotation)
		{
			base.transform.localPosition = EndPosition;
			base.transform.localRotation = EndRotation;
			LockEffect.Play();
			if (Staggered && !Empty)
			{
				Shield.SetActive(value: true);
			}
			Emerging = false;
		}
	}

	public void ResetSegment()
	{
		if (MySegment != null)
		{
			Object.Destroy(MySegment.gameObject);
			GenericPlatform.SetActive(value: true);
			RockPlatform.SetActive(value: false);
			Shield.SetActive(value: false);
			Empty = false;
			base.transform.localPosition = StartPosition;
			base.transform.localRotation = StartRotation;
			Start();
		}
	}

	public void DestroySegments()
	{
		StartCoroutine(DestroySegmentWait());
	}

	private IEnumerator DestroySegmentWait()
	{
		yield return new WaitForSeconds(1f);
		if (MySegment != null)
		{
			Object.Destroy(MySegment.gameObject);
		}
		base.transform.localPosition = StartPosition;
		base.transform.localRotation = StartRotation;
		EndPosition = StartPosition;
		EndRotation = StartRotation;
		GenericPlatform.SetActive(value: true);
	}
}
