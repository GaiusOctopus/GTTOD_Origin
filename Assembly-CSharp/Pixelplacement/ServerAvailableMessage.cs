using UnityEngine.Networking;

namespace Pixelplacement
{
	public class ServerAvailableMessage : MessageBase
	{
		public string ip;

		public int port;

		public string deviceId;

		public ServerAvailableMessage()
		{
		}

		public ServerAvailableMessage(string ip, int port, string deviceId)
		{
			this.ip = ip;
			this.port = port;
			this.deviceId = deviceId;
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(ip);
			writer.WritePackedUInt32((uint)port);
			writer.Write(deviceId);
		}

		public override void Deserialize(NetworkReader reader)
		{
			ip = reader.ReadString();
			port = (int)reader.ReadPackedUInt32();
			deviceId = reader.ReadString();
		}
	}
}
