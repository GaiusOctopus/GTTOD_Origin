using UnityEngine.Networking;

namespace Pixelplacement
{
	public class BoolMessage : MessageBase
	{
		public bool value;

		public string id;

		public BoolMessage()
		{
		}

		public BoolMessage(bool value, string id)
		{
			this.value = value;
			this.id = id;
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(value);
			writer.Write(id);
		}

		public override void Deserialize(NetworkReader reader)
		{
			value = reader.ReadBoolean();
			id = reader.ReadString();
		}
	}
}
