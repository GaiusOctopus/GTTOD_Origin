using UnityEngine.Networking;

namespace Pixelplacement
{
	public class IntMessage : MessageBase
	{
		public int value;

		public string id;

		public IntMessage()
		{
		}

		public IntMessage(int value, string id)
		{
			this.value = value;
			this.id = id;
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt32((uint)value);
			writer.Write(id);
		}

		public override void Deserialize(NetworkReader reader)
		{
			value = (int)reader.ReadPackedUInt32();
			id = reader.ReadString();
		}
	}
}
