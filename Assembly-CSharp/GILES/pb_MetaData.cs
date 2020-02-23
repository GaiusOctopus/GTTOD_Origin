using GILES.Serialization;
using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace GILES
{
	[Serializable]
	public class pb_MetaData : ISerializable
	{
		public const string GUID_NOT_FOUND = "MetaData_NoGUIDPresent";

		public const string ASSET_BUNDLE = "MetaData_BundleAsset";

		public const string ASSET_INSTANCE = "MetaData_InstanceAsset";

		[SerializeField]
		private string _fileId = "MetaData_NoGUIDPresent";

		[SerializeField]
		private pb_AssetBundlePath _assetBundlePath;

		[SerializeField]
		private AssetType _assetType = AssetType.Instance;

		public string[] tags = new string[0];

		public pb_ComponentDiff componentDiff;

		public string fileId => _fileId;

		public pb_AssetBundlePath assetBundlePath => _assetBundlePath;

		public AssetType assetType => _assetType;

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("_fileId", _fileId, typeof(string));
			info.AddValue("_assetBundlePath", _assetBundlePath, typeof(pb_AssetBundlePath));
			info.AddValue("_assetType", _assetType, typeof(AssetType));
			info.AddValue("componentDiff", componentDiff, typeof(pb_ComponentDiff));
		}

		public pb_MetaData(SerializationInfo info, StreamingContext context)
		{
			_fileId = (string)info.GetValue("_fileId", typeof(string));
			_assetBundlePath = (pb_AssetBundlePath)info.GetValue("_assetBundlePath", typeof(pb_AssetBundlePath));
			_assetType = (AssetType)info.GetValue("_assetType", typeof(AssetType));
			componentDiff = (pb_ComponentDiff)info.GetValue("componentDiff", typeof(pb_ComponentDiff));
		}

		public pb_MetaData()
		{
			_assetType = AssetType.Instance;
			_fileId = "MetaData_NoGUIDPresent";
			_assetBundlePath = null;
			componentDiff = new pb_ComponentDiff();
		}

		public void SetAssetBundleData(string bundleName, string assetPath)
		{
			_fileId = "MetaData_BundleAsset";
			_assetType = AssetType.Bundle;
			_assetBundlePath = new pb_AssetBundlePath(bundleName, assetPath);
		}

		public void SetFileId(string id)
		{
			_assetType = AssetType.Resource;
			_assetBundlePath = null;
			_fileId = id;
		}
	}
}
