using UnityEngine.Networking;

namespace Pixelplacement
{
	public class FloatMessage : MessageBase
	{
		public float value;

		public string id;

		public FloatMessage()
		{
		}

		public FloatMessage(float value, string id)
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
			value = reader.ReadSingle();
			id = reader.ReadString();
		}
	}
}
