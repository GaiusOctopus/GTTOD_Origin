using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace GILES
{
	[Serializable]
	public class pb_Transform : IEquatable<pb_Transform>, ISerializable
	{
		private bool dirty = true;

		[SerializeField]
		private Vector3 _position;

		[SerializeField]
		private Quaternion _rotation;

		[SerializeField]
		private Vector3 _scale;

		private Matrix4x4 matrix;

		public static readonly pb_Transform identity = new pb_Transform(Vector3.zero, Quaternion.identity, Vector3.one);

		public Vector3 position
		{
			get
			{
				return _position;
			}
			set
			{
				dirty = true;
				_position = value;
			}
		}

		public Quaternion rotation
		{
			get
			{
				return _rotation;
			}
			set
			{
				dirty = true;
				_rotation = value;
			}
		}

		public Vector3 scale
		{
			get
			{
				return _scale;
			}
			set
			{
				dirty = true;
				_scale = value;
			}
		}

		public Vector3 up => rotation * Vector3.up;

		public Vector3 forward => rotation * Vector3.forward;

		public Vector3 right => rotation * Vector3.right;

		public pb_Transform()
		{
			position = Vector3.zero;
			rotation = Quaternion.identity;
			scale = Vector3.one;
			matrix = Matrix4x4.identity;
			dirty = false;
		}

		public pb_Transform(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			this.position = position;
			this.rotation = rotation;
			this.scale = scale;
			matrix = Matrix4x4.TRS(position, rotation, scale);
			dirty = false;
		}

		public pb_Transform(Transform transform)
		{
			position = transform.position;
			rotation = transform.localRotation;
			scale = transform.localScale;
			matrix = Matrix4x4.TRS(position, rotation, scale);
			dirty = false;
		}

		public pb_Transform(pb_Transform transform)
		{
			position = transform.position;
			rotation = transform.rotation;
			scale = transform.scale;
			matrix = Matrix4x4.TRS(position, rotation, scale);
			dirty = false;
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("position", _position, typeof(Vector3));
			info.AddValue("rotation", _rotation, typeof(Quaternion));
			info.AddValue("scale", _scale, typeof(Vector3));
		}

		public pb_Transform(SerializationInfo info, StreamingContext context)
		{
			_position = (Vector3)info.GetValue("position", typeof(Vector3));
			_rotation = (Quaternion)info.GetValue("rotation", typeof(Quaternion));
			_scale = (Vector3)info.GetValue("scale", typeof(Vector3));
			dirty = true;
		}

		public void SetTRS(Transform trs)
		{
			position = trs.position;
			rotation = trs.localRotation;
			scale = trs.localScale;
			dirty = true;
		}

		private bool Approx(Vector3 lhs, Vector3 rhs)
		{
			if (Mathf.Abs(lhs.x - rhs.x) < Mathf.Epsilon && Mathf.Abs(lhs.y - rhs.y) < Mathf.Epsilon)
			{
				return Mathf.Abs(lhs.z - rhs.z) < Mathf.Epsilon;
			}
			return false;
		}

		private bool Approx(Quaternion lhs, Quaternion rhs)
		{
			if (Mathf.Abs(lhs.x - rhs.x) < Mathf.Epsilon && Mathf.Abs(lhs.y - rhs.y) < Mathf.Epsilon && Mathf.Abs(lhs.z - rhs.z) < Mathf.Epsilon)
			{
				return Mathf.Abs(lhs.w - rhs.w) < Mathf.Epsilon;
			}
			return false;
		}

		public bool Equals(pb_Transform rhs)
		{
			if (Approx(position, rhs.position) && Approx(rotation, rhs.rotation))
			{
				return Approx(scale, rhs.scale);
			}
			return false;
		}

		public override bool Equals(object rhs)
		{
			if (rhs is pb_Transform)
			{
				return Equals((pb_Transform)rhs);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return position.GetHashCode() ^ rotation.GetHashCode() ^ scale.GetHashCode();
		}

		public Matrix4x4 GetMatrix()
		{
			if (!dirty)
			{
				return matrix;
			}
			dirty = false;
			matrix = Matrix4x4.TRS(position, rotation, scale);
			return matrix;
		}

		public static pb_Transform operator -(pb_Transform lhs, pb_Transform rhs)
		{
			return new pb_Transform
			{
				position = lhs.position - rhs.position,
				rotation = Quaternion.Inverse(rhs.rotation) * lhs.rotation,
				scale = new Vector3(lhs.scale.x / rhs.scale.x, lhs.scale.y / rhs.scale.y, lhs.scale.z / rhs.scale.z)
			};
		}

		public static pb_Transform operator +(pb_Transform lhs, pb_Transform rhs)
		{
			return new pb_Transform
			{
				position = lhs.position + rhs.position,
				rotation = lhs.rotation * rhs.rotation,
				scale = new Vector3(lhs.scale.x * rhs.scale.x, lhs.scale.y * rhs.scale.y, lhs.scale.z * rhs.scale.z)
			};
		}

		public static pb_Transform operator +(Transform lhs, pb_Transform rhs)
		{
			return new pb_Transform
			{
				position = lhs.position + rhs.position,
				rotation = lhs.localRotation * rhs.rotation,
				scale = new Vector3(lhs.localScale.x * rhs.scale.x, lhs.localScale.y * rhs.scale.y, lhs.localScale.z * rhs.scale.z)
			};
		}

		public static bool operator ==(pb_Transform lhs, pb_Transform rhs)
		{
			if ((object)lhs != rhs)
			{
				return lhs.Equals(rhs);
			}
			return true;
		}

		public static bool operator !=(pb_Transform lhs, pb_Transform rhs)
		{
			return !(lhs == rhs);
		}

		public override string ToString()
		{
			return position.ToString("F2") + "\n" + rotation.ToString("F2") + "\n" + scale.ToString("F2");
		}
	}
}
