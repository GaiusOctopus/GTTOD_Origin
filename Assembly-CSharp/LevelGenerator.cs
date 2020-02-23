using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	public List<GameObject> RoofSegments;

	[HideInInspector]
	public List<int> UsedIDs;

	private Transform RoofObject;

	private int RoofID;

	private void Start()
	{
		QuadrantOne();
	}

	public void QuadrantOne()
	{
		RoofID = Random.Range(0, RoofSegments.Count);
		RoofObject = Object.Instantiate(RoofSegments[RoofID], base.transform.position, base.transform.rotation).transform;
		RoofObject.Rotate(base.transform.up * 0f);
		RoofObject.transform.parent = base.transform;
		UsedIDs.Add(RoofID);
		QuadrantTwo();
	}

	public void QuadrantTwo()
	{
		RoofID = Random.Range(0, RoofSegments.Count);
		if (!UsedIDs.Contains(RoofID))
		{
			RoofObject = Object.Instantiate(RoofSegments[RoofID], base.transform.position, base.transform.rotation).transform;
			RoofObject.Rotate(base.transform.up * 90f);
			RoofObject.transform.parent = base.transform;
			UsedIDs.Add(RoofID);
			QuadrantThree();
		}
		else
		{
			QuadrantTwo();
		}
	}

	public void QuadrantThree()
	{
		RoofID = Random.Range(0, RoofSegments.Count);
		if (!UsedIDs.Contains(RoofID))
		{
			RoofObject = Object.Instantiate(RoofSegments[RoofID], base.transform.position, base.transform.rotation).transform;
			RoofObject.Rotate(base.transform.up * 180f);
			RoofObject.transform.parent = base.transform;
			UsedIDs.Add(RoofID);
			QuadrantFour();
		}
		else
		{
			QuadrantThree();
		}
	}

	public void QuadrantFour()
	{
		RoofID = Random.Range(0, RoofSegments.Count);
		if (!UsedIDs.Contains(RoofID))
		{
			RoofObject = Object.Instantiate(RoofSegments[RoofID], base.transform.position, base.transform.rotation).transform;
			RoofObject.Rotate(base.transform.up * 270f);
			RoofObject.transform.parent = base.transform;
			UsedIDs.Add(RoofID);
		}
		else
		{
			QuadrantFour();
		}
	}

	public void RemoveGenerators()
	{
		foreach (Transform item in base.transform)
		{
			Object.Destroy(item.gameObject);
		}
	}
}
