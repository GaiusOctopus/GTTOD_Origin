using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace GILES.Serialization
{
	[Serializable]
	public class pb_SceneNode : ISerializable
	{
		public string name;

		public pb_Transform transform;

		public List<pb_SceneNode> children;

		public pb_MetaData metadata;

		public List<pb_ISerializable> components;

		public pb_SceneNode()
		{
		}

		public pb_SceneNode(SerializationInfo info, StreamingContext context)
		{
			name = (string)info.GetValue("name", typeof(string));
			transform = (pb_Transform)info.GetValue("transform", typeof(pb_Transform));
			children = (List<pb_SceneNode>)info.GetValue("children", typeof(List<pb_SceneNode>));
			metadata = (pb_MetaData)info.GetValue("metadata", typeof(pb_MetaData));
			if (metadata.assetType == AssetType.Instance)
			{
				components = (List<pb_ISerializable>)info.GetValue("components", typeof(List<pb_ISerializable>));
			}
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("name", name, typeof(string));
			info.AddValue("transform", transform, typeof(pb_Transform));
			info.AddValue("children", children, typeof(List<pb_SceneNode>));
			info.AddValue("metadata", metadata, typeof(pb_MetaData));
			if (metadata == null || metadata.assetType == AssetType.Instance)
			{
				info.AddValue("components", components, typeof(List<pb_SerializableObject<Component>>));
			}
		}

		public pb_SceneNode(GameObject root)
		{
			name = root.name;
			components = new List<pb_ISerializable>();
			pb_MetaDataComponent pb_MetaDataComponent = root.GetComponent<pb_MetaDataComponent>();
			if (pb_MetaDataComponent == null)
			{
				pb_MetaDataComponent = root.AddComponent<pb_MetaDataComponent>();
			}
			metadata = pb_MetaDataComponent.metadata;
			if (metadata.assetType == AssetType.Instance)
			{
				Component[] array = root.GetComponents<Component>();
				foreach (Component component in array)
				{
					if (!(component == null) && !(component is Transform) && !component.GetType().GetCustomAttributes(inherit: true).Any((object x) => x is pb_JsonIgnoreAttribute))
					{
						components.Add(pb_Serialization.CreateSerializableObject(component));
					}
				}
			}
			this.transform = new pb_Transform();
			this.transform.SetTRS(root.transform);
			children = new List<pb_SceneNode>();
			foreach (Transform item in root.transform)
			{
				if (item.gameObject.activeSelf)
				{
					children.Add(new pb_SceneNode(item.gameObject));
				}
			}
		}

		public GameObject ToGameObject()
		{
			GameObject gameObject;
			if (metadata.assetType == AssetType.Instance)
			{
				gameObject = new GameObject();
				foreach (pb_ISerializable component in components)
				{
					gameObject.AddComponent(component);
				}
			}
			else
			{
				GameObject gameObject2 = pb_ResourceManager.LoadPrefabWithMetadata(metadata);
				if (gameObject2 != null)
				{
					gameObject = UnityEngine.Object.Instantiate(gameObject2);
					metadata.componentDiff.ApplyPatch(gameObject);
				}
				else
				{
					gameObject = new GameObject();
				}
			}
			gameObject.DemandComponent<pb_MetaDataComponent>().metadata = metadata;
			gameObject.name = name;
			gameObject.transform.SetTRS(transform);
			foreach (pb_SceneNode child in children)
			{
				child.ToGameObject().transform.SetParent(gameObject.transform);
			}
			return gameObject;
		}

		public static explicit operator GameObject(pb_SceneNode node)
		{
			return node.ToGameObject();
		}
	}
}
