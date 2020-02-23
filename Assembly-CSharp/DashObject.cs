using UnityEngine;

public class DashObject : MonoBehaviour
{
	public LayerMask AvailableLayers;

	private Transform EndPoint;

	private Transform MyDashObject;

	private Transform Player;

	private Transform Direction;

	private ac_CharacterController CharacterController;

	private Rigidbody PlayerPhysics;

	private RaycastHit Ray;

	private float TimeToRelease = 1f;

	private void Start()
	{
		Player = GameManager.GM.Player.transform;
		CharacterController = Player.GetComponent<ac_CharacterController>();
		PlayerPhysics = Player.GetComponent<Rigidbody>();
		MyDashObject = new GameObject("DashObject").transform;
		MyDashObject.parent = base.transform;
		if (Physics.Raycast(base.transform.position, base.transform.forward, out Ray, 35f, AvailableLayers))
		{
			EndPoint = new GameObject("EndPoint").transform;
			EndPoint.position = Ray.point;
			Direction = new GameObject("DirectionObject").transform;
			Direction.transform.position = base.transform.position;
			Direction.LookAt(EndPoint);
			EndPoint.position = Ray.point + Direction.transform.forward * -5f;
		}
		else
		{
			EndPoint = new GameObject("EndPoint").transform;
			EndPoint.position = base.transform.position + CharacterController.MainCamera.transform.forward * 25f;
			Direction = new GameObject("DirectionObject").transform;
			Direction.transform.position = base.transform.position;
			Direction.LookAt(EndPoint);
		}
		PlayerPhysics.isKinematic = true;
		CharacterController.WallBump(AddForce: false);
		CharacterController.CanWallRun = false;
		MyDashObject.position = Player.position;
		Player.parent = MyDashObject;
		CharacterController.Blur();
	}

	private void Update()
	{
		float num = Vector3.Distance(Player.position, EndPoint.position);
		TimeToRelease -= Time.deltaTime;
		if (num < 2f || TimeToRelease <= 0f)
		{
			CharacterController.ReParent();
			PlayerPhysics.isKinematic = false;
			PlayerPhysics.velocity = Vector3.zero;
			Object.Destroy(Direction.gameObject);
			Object.Destroy(base.gameObject);
			CharacterController.CanWallRun = true;
			PlayerPhysics.AddForce(Direction.transform.forward * 1250f);
		}
		MyDashObject.position = Vector3.MoveTowards(MyDashObject.position, EndPoint.position, 3f);
	}
}
