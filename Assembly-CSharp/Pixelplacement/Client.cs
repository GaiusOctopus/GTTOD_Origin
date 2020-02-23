using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Pixelplacement
{
	public class Client : NetworkDiscovery
	{
		[Tooltip("Must match the server's broadcasting port.")]
		public int broadcastingPort;

		[Tooltip("Must match the server's primary quality of service.")]
		public QosType primaryQualityOfService = QosType.Reliable;

		[Tooltip("Must match the server's secondary quality of service.")]
		public QosType secondaryQualityOfService = QosType.UnreliableFragmented;

		public uint initialBandwidth;

		private static NetworkClient _client;

		public static int PrimaryChannel
		{
			get;
			private set;
		}

		public static int SecondaryChannel
		{
			get;
			private set;
		}

		public static bool Running
		{
			get;
			private set;
		}

		public static bool Connected
		{
			get
			{
				if (_client == null)
				{
					return false;
				}
				return _client.isConnected;
			}
		}

		public static event Action OnConnected;

		public static event Action OnDisconnected;

		public static event Action<ServerAvailableMessage> OnServerAvailable;

		public static event Action<FloatMessage> OnFloat;

		public static event Action<FloatArrayMessage> OnFloatArray;

		public static event Action<IntMessage> OnInt;

		public static event Action<IntArrayMessage> OnIntArray;

		public static event Action<Vector2Message> OnVector2;

		public static event Action<Vector2ArrayMessage> OnVector2Array;

		public static event Action<Vector3Message> OnVector3;

		public static event Action<Vector3ArrayMessage> OnVector3Array;

		public static event Action<QuaternionMessage> OnQuaternion;

		public static event Action<QuaternionArrayMessage> OnQuaternionArray;

		public static event Action<Vector4Message> OnVector4;

		public static event Action<Vector4ArrayMessage> OnVector4Array;

		public static event Action<RectMessage> OnRect;

		public static event Action<RectArrayMessage> OnRectArray;

		public static event Action<StringMessage> OnString;

		public static event Action<StringArrayMessage> OnStringArray;

		public static event Action<ByteMessage> OnByte;

		public static event Action<ByteArrayMessage> OnByteArray;

		public static event Action<ColorMessage> OnColor;

		public static event Action<ColorArrayMessage> OnColorArray;

		public static event Action<Color32Message> OnColor32;

		public static event Action<Color32ArrayMessage> OnColor32Array;

		public static event Action<RectByteArrayMessage> OnRectByteArray;

		public static event Action<BoolMessage> OnBool;

		public static event Action<BoolArrayMessage> OnBoolArray;

		public void Reset()
		{
			broadcastingPort = 47777;
			initialBandwidth = 500000u;
			base.showGUI = false;
			base.broadcastData = "";
			primaryQualityOfService = QosType.Reliable;
			secondaryQualityOfService = QosType.UnreliableFragmented;
		}

		private void Start()
		{
			base.broadcastPort = broadcastingPort;
			Initialize();
			StartAsClient();
			_client = new NetworkClient();
			ConnectionConfig connectionConfig = new ConnectionConfig();
			PrimaryChannel = connectionConfig.AddChannel(primaryQualityOfService);
			SecondaryChannel = connectionConfig.AddChannel(secondaryQualityOfService);
			connectionConfig.InitialBandwidth = initialBandwidth;
			HostTopology topology = new HostTopology(connectionConfig, 1);
			_client.Configure(topology);
			_client.RegisterHandler(32, HandleConnected);
			_client.RegisterHandler(33, HandleDisconnected);
			_client.RegisterHandler(3001, HandleFloat);
			_client.RegisterHandler(3002, HandleFloatArray);
			_client.RegisterHandler(3003, HandleInt);
			_client.RegisterHandler(3004, HandleIntArray);
			_client.RegisterHandler(3005, HandleVector2);
			_client.RegisterHandler(3006, HandleVector2Array);
			_client.RegisterHandler(3007, HandleVector3);
			_client.RegisterHandler(3008, HandleVector3Array);
			_client.RegisterHandler(3024, HandleQuaternion);
			_client.RegisterHandler(3025, HandleQuaternionArray);
			_client.RegisterHandler(3009, HandleVector4);
			_client.RegisterHandler(3010, HandleVector4Array);
			_client.RegisterHandler(3011, HandleRect);
			_client.RegisterHandler(3012, HandleRectArray);
			_client.RegisterHandler(3013, HandleString);
			_client.RegisterHandler(3014, HandleStringArray);
			_client.RegisterHandler(3015, HandleByte);
			_client.RegisterHandler(3016, HandleByteArray);
			_client.RegisterHandler(3017, HandleColor);
			_client.RegisterHandler(3018, HandleColorArray);
			_client.RegisterHandler(3019, HandleColor32);
			_client.RegisterHandler(3020, HandleColor32Array);
			_client.RegisterHandler(3021, HandleRectByteArray);
			_client.RegisterHandler(3022, HandleBool);
			_client.RegisterHandler(3023, HandleBoolArray);
		}

		private void OnEnable()
		{
			Running = true;
		}

		private void OnDisable()
		{
			Running = false;
		}

		private void OnDestroy()
		{
			StopBroadcast();
		}

		public static void RegisterHandler(short msgType, NetworkMessageDelegate handler)
		{
			_client.RegisterHandler(msgType, handler);
		}

		public static void UnregisterHandler(short msgType)
		{
			_client.UnregisterHandler(msgType);
		}

		public static void Connect(string serverIp, int serverPort)
		{
			_client.Connect(serverIp, serverPort);
		}

		public static void Disconnect()
		{
			_client.Disconnect();
		}

		public static void Send(short msgType, MessageBase message, int qualityOfServiceChannel = 0)
		{
			if (Connected)
			{
				_client.SendByChannel(msgType, message, qualityOfServiceChannel);
			}
		}

		public static void Send(float value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3001, new FloatMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(float[] value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3002, new FloatArrayMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(int value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3003, new IntMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(int[] value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3004, new IntArrayMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(Vector2 value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3005, new Vector2Message(value, id), qualityOfServiceChannel);
		}

		public static void Send(Vector2[] value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3006, new Vector2ArrayMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(Vector3 value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3007, new Vector3Message(value, id), qualityOfServiceChannel);
		}

		public static void Send(Vector3[] value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3008, new Vector3ArrayMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(Quaternion value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3024, new QuaternionMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(Quaternion[] value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3025, new QuaternionArrayMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(Vector4 value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3009, new Vector4Message(value, id), qualityOfServiceChannel);
		}

		public static void Send(Vector4[] value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3010, new Vector4ArrayMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(Rect value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3011, new RectMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(Rect[] value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3012, new RectArrayMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(string value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3013, new StringMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(string[] value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3014, new StringArrayMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(byte value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3015, new ByteMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(byte[] value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3016, new ByteArrayMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(Color value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3017, new ColorMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(Color[] value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3018, new ColorArrayMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(Color32 value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3019, new Color32Message(value, id), qualityOfServiceChannel);
		}

		public static void Send(Color32[] value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3020, new Color32ArrayMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(Rect rect, byte[] bytes, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3021, new RectByteArrayMessage(rect, bytes, id), qualityOfServiceChannel);
		}

		public static void Send(bool value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3022, new BoolMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(bool[] value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send(3023, new BoolArrayMessage(value, id), qualityOfServiceChannel);
		}

		private void HandleFloat(NetworkMessage message)
		{
			if (Client.OnFloat != null)
			{
				Client.OnFloat(message.ReadMessage<FloatMessage>());
			}
		}

		private void HandleFloatArray(NetworkMessage message)
		{
			if (Client.OnFloatArray != null)
			{
				Client.OnFloatArray(message.ReadMessage<FloatArrayMessage>());
			}
		}

		private void HandleInt(NetworkMessage message)
		{
			if (Client.OnInt != null)
			{
				Client.OnInt(message.ReadMessage<IntMessage>());
			}
		}

		private void HandleIntArray(NetworkMessage message)
		{
			if (Client.OnIntArray != null)
			{
				Client.OnIntArray(message.ReadMessage<IntArrayMessage>());
			}
		}

		private void HandleVector2(NetworkMessage message)
		{
			if (Client.OnVector2 != null)
			{
				Client.OnVector2(message.ReadMessage<Vector2Message>());
			}
		}

		private void HandleVector2Array(NetworkMessage message)
		{
			if (Client.OnVector2Array != null)
			{
				Client.OnVector2Array(message.ReadMessage<Vector2ArrayMessage>());
			}
		}

		private void HandleVector3(NetworkMessage message)
		{
			if (Client.OnVector3 != null)
			{
				Client.OnVector3(message.ReadMessage<Vector3Message>());
			}
		}

		private void HandleVector3Array(NetworkMessage message)
		{
			if (Client.OnVector3Array != null)
			{
				Client.OnVector3Array(message.ReadMessage<Vector3ArrayMessage>());
			}
		}

		private void HandleQuaternion(NetworkMessage message)
		{
			if (Client.OnQuaternion != null)
			{
				Client.OnQuaternion(message.ReadMessage<QuaternionMessage>());
			}
		}

		private void HandleQuaternionArray(NetworkMessage message)
		{
			if (Client.OnQuaternionArray != null)
			{
				Client.OnQuaternionArray(message.ReadMessage<QuaternionArrayMessage>());
			}
		}

		private void HandleVector4(NetworkMessage message)
		{
			if (Client.OnVector4 != null)
			{
				Client.OnVector4(message.ReadMessage<Vector4Message>());
			}
		}

		private void HandleVector4Array(NetworkMessage message)
		{
			if (Client.OnVector4Array != null)
			{
				Client.OnVector4Array(message.ReadMessage<Vector4ArrayMessage>());
			}
		}

		private void HandleRect(NetworkMessage message)
		{
			if (Client.OnRect != null)
			{
				Client.OnRect(message.ReadMessage<RectMessage>());
			}
		}

		private void HandleRectArray(NetworkMessage message)
		{
			if (Client.OnRectArray != null)
			{
				Client.OnRectArray(message.ReadMessage<RectArrayMessage>());
			}
		}

		private void HandleString(NetworkMessage message)
		{
			if (Client.OnString != null)
			{
				Client.OnString(message.ReadMessage<StringMessage>());
			}
		}

		private void HandleStringArray(NetworkMessage message)
		{
			if (Client.OnStringArray != null)
			{
				Client.OnStringArray(message.ReadMessage<StringArrayMessage>());
			}
		}

		private void HandleByte(NetworkMessage message)
		{
			if (Client.OnByte != null)
			{
				Client.OnByte(message.ReadMessage<ByteMessage>());
			}
		}

		private void HandleByteArray(NetworkMessage message)
		{
			if (Client.OnByteArray != null)
			{
				Client.OnByteArray(message.ReadMessage<ByteArrayMessage>());
			}
		}

		private void HandleColor(NetworkMessage message)
		{
			if (Client.OnColor != null)
			{
				Client.OnColor(message.ReadMessage<ColorMessage>());
			}
		}

		private void HandleColorArray(NetworkMessage message)
		{
			if (Client.OnColorArray != null)
			{
				Client.OnColorArray(message.ReadMessage<ColorArrayMessage>());
			}
		}

		private void HandleColor32(NetworkMessage message)
		{
			if (Client.OnColor32 != null)
			{
				Client.OnColor32(message.ReadMessage<Color32Message>());
			}
		}

		private void HandleColor32Array(NetworkMessage message)
		{
			if (Client.OnColor32Array != null)
			{
				Client.OnColor32Array(message.ReadMessage<Color32ArrayMessage>());
			}
		}

		private void HandleRectByteArray(NetworkMessage message)
		{
			if (Client.OnRectByteArray != null)
			{
				Client.OnRectByteArray(message.ReadMessage<RectByteArrayMessage>());
			}
		}

		private void HandleBool(NetworkMessage message)
		{
			if (Client.OnBool != null)
			{
				Client.OnBool(message.ReadMessage<BoolMessage>());
			}
		}

		private void HandleBoolArray(NetworkMessage message)
		{
			if (Client.OnBoolArray != null)
			{
				Client.OnBoolArray(message.ReadMessage<BoolArrayMessage>());
			}
		}

		public override void OnReceivedBroadcast(string fromAddress, string data)
		{
			if (!Connected)
			{
				int num = fromAddress.LastIndexOf(':');
				if (num != -1)
				{
					fromAddress = fromAddress.Substring(num + 1);
				}
				string[] array = data.Split('_');
				int port = int.Parse(array[0]);
				string deviceId = array[1].Split(new string[1]
				{
					"~!~"
				}, StringSplitOptions.None)[0];
				if (Client.OnServerAvailable != null)
				{
					Client.OnServerAvailable(new ServerAvailableMessage(fromAddress, port, deviceId));
				}
			}
		}

		private void HandleConnected(NetworkMessage message)
		{
			if (Client.OnConnected != null)
			{
				Client.OnConnected();
			}
		}

		private void HandleDisconnected(NetworkMessage message)
		{
			if (Client.OnDisconnected != null)
			{
				Client.OnDisconnected();
			}
		}
	}
}
