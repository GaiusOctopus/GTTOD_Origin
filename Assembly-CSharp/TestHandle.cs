using UnityEngine;
using UnityEngine.UI;

public class TestHandle : MonoBehaviour
{
	public GameObject clientObject;

	private Client clientScript;

	public InputField InputField;

	private void Start()
	{
		clientScript = clientObject.GetComponent<Client>();
		clientScript.Connect();
	}

	private void Update()
	{
	}

	public void sendMessage()
	{
		clientScript.sendMessage(InputField.text);
	}

	public void handleMessage(Message message)
	{
		Debug.Log(message.username + " : " + message.content);
	}
}
