using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Pixelplacement
{
	public class Server : NetworkDiscovery
	{
		[Tooltip("Must match the client's primary quality of service.")]
		public QosType primaryQualityOfService = QosType.Reliable;

		[Tooltip("Must match the client's secondary quality of service.")]
		public QosType secondaryQualityOfService = QosType.Reliable;

		[Tooltip("Optional name for this device to be sent to clients for connection identification.")]
		public string customDeviceId;

		[Tooltip("Must match the client's broadcasting port.")]
		public int broadcastingPort;

		public int maxConnections;

		public uint initialBandwidth;

		private static NetworkServerSimple _server = new NetworkServerSimple();

		private static string _randomIdKey = "RandomIdKey";

		public static int ConnectedCount
		{
			get;
			private set;
		}

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

		public static string DeviceId
		{
			get;
			private set;
		}

		public static bool Running
		{
			get;
			private set;
		}

		public static bool Connected => ConnectedCount > 0;

		private int ServerPort => base.broadcastPort + 1;

		public static event Action OnPlayerConnected;

		public static event Action OnPlayerDisconnected;

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
			maxConnections = 1;
			base.showGUI = false;
			broadcastingPort = 47777;
			initialBandwidth = 500000u;
			primaryQualityOfService = QosType.UnreliableSequenced;
			secondaryQualityOfService = QosType.Reliable;
		}

		private void Start()
		{
			base.broadcastPort = broadcastingPort;
			base.broadcastData = ServerPort.ToString();
			if (string.IsNullOrEmpty(customDeviceId))
			{
				GenerateID();
				DeviceId = PlayerPrefs.GetString(_randomIdKey);
				base.broadcastData = base.broadcastData + "_" + DeviceId;
			}
			else
			{
				base.broadcastData = base.broadcastData + "_" + customDeviceId;
			}
			base.broadcastData += "~!~";
			Init();
			ConnectionConfig connectionConfig = new ConnectionConfig();
			PrimaryChannel = connectionConfig.AddChannel(primaryQualityOfService);
			SecondaryChannel = connectionConfig.AddChannel(secondaryQualityOfService);
			connectionConfig.InitialBandwidth = initialBandwidth;
			HostTopology topology = new HostTopology(connectionConfig, maxConnections);
			_server.Listen(ServerPort, topology);
			_server.RegisterHandler(32, HandleConnect);
			_server.RegisterHandler(33, HandleDisconnect);
			_server.RegisterHandler(3001, HandleFloat);
			_server.RegisterHandler(3002, HandleFloatArray);
			_server.RegisterHandler(3003, HandleInt);
			_server.RegisterHandler(3004, HandleIntArray);
			_server.RegisterHandler(3005, HandleVector2);
			_server.RegisterHandler(3006, HandleVector2Array);
			_server.RegisterHandler(3007, HandleVector3);
			_server.RegisterHandler(3008, HandleVector3Array);
			_server.RegisterHandler(3024, HandleQuaternion);
			_server.RegisterHandler(3025, HandleQuaternionArray);
			_server.RegisterHandler(3009, HandleVector4);
			_server.RegisterHandler(3010, HandleVector4Array);
			_server.RegisterHandler(3011, HandleRect);
			_server.RegisterHandler(3012, HandleRectArray);
			_server.RegisterHandler(3013, HandleString);
			_server.RegisterHandler(3014, HandleStringArray);
			_server.RegisterHandler(3015, HandleByte);
			_server.RegisterHandler(3016, HandleByteArray);
			_server.RegisterHandler(3017, HandleColor);
			_server.RegisterHandler(3018, HandleColorArray);
			_server.RegisterHandler(3019, HandleColor32);
			_server.RegisterHandler(3020, HandleColor32Array);
			_server.RegisterHandler(3021, HandleRectByteArray);
			_server.RegisterHandler(3022, HandleBool);
			_server.RegisterHandler(3023, HandleBoolArray);
			base.transform.parent = null;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		private void OnDestroy()
		{
			_server.Stop();
		}

		private void OnEnable()
		{
			Running = true;
		}

		private void OnDisable()
		{
			Running = false;
		}

		private void Init()
		{
			Initialize();
			StartAsServer();
		}

		private void GenerateID()
		{
			if (PlayerPrefs.HasKey(_randomIdKey))
			{
				return;
			}
			string text = "";
			string[] array = Guid.NewGuid().ToString().Split('-');
			for (int i = 0; i < array.Length; i++)
			{
				int num = 0;
				string text2 = array[i];
				foreach (char c in text2)
				{
					int result = 0;
					if (int.TryParse(c.ToString(), out result))
					{
						num += result;
					}
				}
				text += ((int)Mathf.Repeat(num, 9f)).ToString();
			}
			PlayerPrefs.SetString(_randomIdKey, text);
		}

		public static void RegisterHandler(short msgType, NetworkMessageDelegate handler)
		{
			_server.RegisterHandler(msgType, handler);
		}

		public static void UnregisterHandler(short msgType)
		{
			_server.UnregisterHandler(msgType);
		}

		public static void Disconnect()
		{
			_server.DisconnectAllConnections();
		}

		public static void Send(short msgType, MessageBase message, int qualityOfServiceChannel = 0)
		{
			foreach (NetworkConnection connection in _server.connections)
			{
				connection?.SendByChannel(msgType, message, qualityOfServiceChannel);
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

		private void Update()
		{
			_server.Update();
		}

		private void HandleConnect(NetworkMessage message)
		{
			ConnectedCount++;
			if (Server.OnPlayerConnected != null)
			{
				Server.OnPlayerConnected();
			}
		}

		private void HandleDisconnect(NetworkMessage message)
		{
			ConnectedCount--;
			if (Server.OnPlayerDisconnected != null)
			{
				Server.OnPlayerDisconnected();
			}
		}

		private void HandleFloat(NetworkMessage message)
		{
			if (Server.OnFloat != null)
			{
				Server.OnFloat(message.ReadMessage<FloatMessage>());
			}
		}

		private void HandleFloatArray(NetworkMessage message)
		{
			if (Server.OnFloatArray != null)
			{
				Server.OnFloatArray(message.ReadMessage<FloatArrayMessage>());
			}
		}

		private void HandleInt(NetworkMessage message)
		{
			if (Server.OnInt != null)
			{
				Server.OnInt(message.ReadMessage<IntMessage>());
			}
		}

		private void HandleIntArray(NetworkMessage message)
		{
			if (Server.OnIntArray != null)
			{
				Server.OnIntArray(message.ReadMessage<IntArrayMessage>());
			}
		}

		private void HandleVector2(NetworkMessage message)
		{
			if (Server.OnVector2 != null)
			{
				Server.OnVector2(message.ReadMessage<Vector2Message>());
			}
		}

		private void HandleVector2Array(NetworkMessage message)
		{
			if (Server.OnVector2Array != null)
			{
				Server.OnVector2Array(message.ReadMessage<Vector2ArrayMessage>());
			}
		}

		private void HandleVector3(NetworkMessage message)
		{
			if (Server.OnVector3 != null)
			{
				Server.OnVector3(message.ReadMessage<Vector3Message>());
			}
		}

		private void HandleVector3Array(NetworkMessage message)
		{
			if (Server.OnVector3Array != null)
			{
				Server.OnVector3Array(message.ReadMessage<Vector3ArrayMessage>());
			}
		}

		private void HandleQuaternion(NetworkMessage message)
		{
			if (Server.OnQuaternion != null)
			{
				Server.OnQuaternion(message.ReadMessage<QuaternionMessage>());
			}
		}

		private void HandleQuaternionArray(NetworkMessage message)
		{
			if (Server.OnQuaternionArray != null)
			{
				Server.OnQuaternionArray(message.ReadMessage<QuaternionArrayMessage>());
			}
		}

		private void HandleVector4(NetworkMessage message)
		{
			if (Server.OnVector4 != null)
			{
				Server.OnVector4(message.ReadMessage<Vector4Message>());
			}
		}

		private void HandleVector4Array(NetworkMessage message)
		{
			if (Server.OnVector4Array != null)
			{
				Server.OnVector4Array(message.ReadMessage<Vector4ArrayMessage>());
			}
		}

		private void HandleRect(NetworkMessage message)
		{
			if (Server.OnRect != null)
			{
				Server.OnRect(message.ReadMessage<RectMessage>());
			}
		}

		private void HandleRectArray(NetworkMessage message)
		{
			if (Server.OnRectArray != null)
			{
				Server.OnRectArray(message.ReadMessage<RectArrayMessage>());
			}
		}

		private void HandleString(NetworkMessage message)
		{
			if (Server.OnString != null)
			{
				Server.OnString(message.ReadMessage<StringMessage>());
			}
		}

		private void HandleStringArray(NetworkMessage message)
		{
			if (Server.OnStringArray != null)
			{
				Server.OnStringArray(message.ReadMessage<StringArrayMessage>());
			}
		}

		private void HandleByte(NetworkMessage message)
		{
			if (Server.OnByte != null)
			{
				Server.OnByte(message.ReadMessage<ByteMessage>());
			}
		}

		private void HandleByteArray(NetworkMessage message)
		{
			if (Server.OnByteArray != null)
			{
				Server.OnByteArray(message.ReadMessage<ByteArrayMessage>());
			}
		}

		private void HandleColor(NetworkMessage message)
		{
			if (Server.OnColor != null)
			{
				Server.OnColor(message.ReadMessage<ColorMessage>());
			}
		}

		private void HandleColorArray(NetworkMessage message)
		{
			if (Server.OnColorArray != null)
			{
				Server.OnColorArray(message.ReadMessage<ColorArrayMessage>());
			}
		}

		private void HandleColor32(NetworkMessage message)
		{
			if (Server.OnColor32 != null)
			{
				Server.OnColor32(message.ReadMessage<Color32Message>());
			}
		}

		private void HandleColor32Array(NetworkMessage message)
		{
			if (Server.OnColor32Array != null)
			{
				Server.OnColor32Array(message.ReadMessage<Color32ArrayMessage>());
			}
		}

		private void HandleRectByteArray(NetworkMessage message)
		{
			if (Server.OnRectByteArray != null)
			{
				Server.OnRectByteArray(message.ReadMessage<RectByteArrayMessage>());
			}
		}

		private void HandleBool(NetworkMessage message)
		{
			if (Server.OnBool != null)
			{
				Server.OnBool(message.ReadMessage<BoolMessage>());
			}
		}

		private void HandleBoolArray(NetworkMessage message)
		{
			if (Server.OnBoolArray != null)
			{
				Server.OnBoolArray(message.ReadMessage<BoolArrayMessage>());
			}
		}
	}
}
