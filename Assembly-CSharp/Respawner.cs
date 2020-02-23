using UnityEngine;

public class Respawner : MonoBehaviour
{
	public int MyIndex;

	public int MyWeight;

	public float MaxDistance = 200f;

	private AIManager MyManager;

	private Transform Target;

	public void Start()
	{
		MyManager = GameManager.GM.GetComponent<AIManager>();
		Target = GameManager.GM.Player.transform;
		MyManager.AddEnemy(MyWeight, this);
	}

	private void Update()
	{
		if (Vector3.Distance(base.transform.position, Target.position) > MaxDistance)
		{
			RemoveEnemy();
			KillOff();
		}
	}

	public void StoreMyIndex(int IndexSent)
	{
		MyIndex = IndexSent;
	}

	public void RemoveEnemy()
	{
		MyManager.RemoveEnemy(MyWeight, MyIndex);
	}

	public void KillOff()
	{
		Object.Destroy(base.gameObject);
	}

	public void CompletelyRemove()
	{
		MyManager.RemoveEnemy(MyWeight, MyIndex);
		Object.Destroy(base.gameObject);
	}
}
