using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Client : MonoBehaviour
{
	public GameObject handler;

	public string server;

	public int port;

	private TcpClient client;

	private NetworkStream stream;

	public string steamID;

	private bool connected;

	public void Connect()
	{
		TcpClient tcpClient = new TcpClient(server, port);
		stream = tcpClient.GetStream();
		byte[] bytes = Encoding.ASCII.GetBytes(steamID);
		stream.Write(bytes, 0, bytes.Length);
		Debug.Log("Tried to send a message!");
		connected = true;
	}

	private void Update()
	{
		if (stream.DataAvailable)
		{
			byte[] array = new byte[256];
			_ = string.Empty;
			int count = stream.Read(array, 0, array.Length);
			Message value = JsonUtility.FromJson<Message>(Encoding.ASCII.GetString(array, 0, count));
			handler.SendMessage("handleMessage", value);
		}
	}

	public void sendMessage(string message)
	{
		if (connected)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(message);
			stream.Write(bytes, 0, bytes.Length);
		}
	}
}
