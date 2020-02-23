using System;
using System.Collections.Generic;
using UnityEngine;

public class ac_ObjectPool : MonoBehaviour
{
	[Serializable]
	public class ac_LocalPools
	{
		[Header("Pool Information")]
		public string PoolName;

		public GameObject PoolObject;

		public int PoolCount;

		[Header("Parent To Object Below?")]
		public bool ParentMe;

		[Header("Parent Index:")]
		public Transform Category;

		[SerializeField]
		private List<GameObject> LocalPool;

		private int CurrentObject;

		private bool FirstItem = true;

		public void InitializeLocalPools()
		{
			if (PoolCount > 0)
			{
				for (int i = 0; i < PoolCount; i++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(PoolObject);
					LocalPool.Add(gameObject);
					ParentObject(gameObject.transform, ParentMe, Category);
				}
			}
		}

		public GameObject GetObject()
		{
			if (FirstItem)
			{
				FirstItem = false;
				return LocalPool[0];
			}
			NextObject();
			return LocalPool[CurrentObject];
		}

		public void NextObject()
		{
			CurrentObject++;
			if (CurrentObject == LocalPool.Count)
			{
				CurrentObject = 0;
			}
		}
	}

	[Header("My Object Pools")]
	public List<ac_LocalPools> Pools;

	private List<string> PoolNames = new List<string>();

	private static Transform Parent;

	private void Start()
	{
		Parent = base.transform;
		SavePoolStrings();
		InitializePools();
	}

	private void SavePoolStrings()
	{
		for (int i = 0; i < Pools.Count; i++)
		{
			PoolNames.Add(Pools[i].PoolName);
		}
	}

	private void InitializePools()
	{
		for (int i = 0; i < Pools.Count; i++)
		{
			Pools[i].InitializeLocalPools();
		}
	}

	public GameObject GetObject(string poolName)
	{
		for (int i = 0; i < PoolNames.Count; i++)
		{
			if (string.Equals(PoolNames[i], poolName))
			{
				return Pools[i].GetObject();
			}
		}
		return null;
	}

	public static void ParentObject(Transform Object, bool ShouldParent, Transform ParentObject)
	{
		if (ShouldParent)
		{
			Object.transform.parent = ParentObject;
		}
		else
		{
			Object.transform.parent = Parent;
		}
	}
}
