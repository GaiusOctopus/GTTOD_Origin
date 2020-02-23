using UnityEngine.Networking;

namespace Pixelplacement
{
	public class StringMessage : MessageBase
	{
		public string value;

		public string id;

		public StringMessage()
		{
		}

		public StringMessage(string value, string id)
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
			value = reader.ReadString();
			id = reader.ReadString();
		}
	}
}
