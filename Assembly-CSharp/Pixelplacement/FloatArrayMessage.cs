using Unity;
using UnityEngine.Networking;

namespace Pixelplacement
{
	public class FloatArrayMessage : MessageBase
	{
		public float[] value;

		public string id;

		public FloatArrayMessage()
		{
		}

		public FloatArrayMessage(float[] value, string id)
		{
			this.value = value;
			this.id = id;
		}

		public override void Serialize(NetworkWriter writer)
		{
			GeneratedNetworkCode._WriteArraySingle_None(writer, value);
			writer.Write(id);
		}

		public override void Deserialize(NetworkReader reader)
		{
			value = GeneratedNetworkCode._ReadArraySingle_None(reader);
			id = reader.ReadString();
		}
	}
}
