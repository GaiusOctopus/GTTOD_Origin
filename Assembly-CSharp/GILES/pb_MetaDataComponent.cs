using GILES.Serialization;
using UnityEngine;

namespace GILES
{
	[pb_JsonIgnore]
	[DisallowMultipleComponent]
	public class pb_MetaDataComponent : MonoBehaviour
	{
		public pb_MetaData metadata = new pb_MetaData();

		public void SetAssetBundleData(string bundleName, string assetPath)
		{
			metadata.SetAssetBundleData(bundleName, assetPath);
		}

		public bool UpdateFileId()
		{
			return false;
		}

		public string GetFileId()
		{
			return metadata.fileId;
		}
	}
}
