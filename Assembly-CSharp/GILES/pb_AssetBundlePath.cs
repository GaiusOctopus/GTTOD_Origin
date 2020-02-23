using System;
using System.Runtime.Serialization;

namespace GILES
{
	[Serializable]
	public class pb_AssetBundlePath : ISerializable
	{
		public string assetBundleName;

		public string filePath;

		public pb_AssetBundlePath(string InAssetBundleName, string InFilePath)
		{
			assetBundleName = InAssetBundleName;
			filePath = InFilePath;
		}

		public pb_AssetBundlePath(SerializationInfo info, StreamingContext context)
		{
			assetBundleName = (string)info.GetValue("assetBundleName", typeof(string));
			filePath = (string)info.GetValue("filePath", typeof(string));
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("assetBundleName", assetBundleName, typeof(string));
			info.AddValue("filePath", filePath, typeof(string));
		}

		public override string ToString()
		{
			return "Bundle: " + assetBundleName + "\nPath: " + filePath;
		}
	}
}
