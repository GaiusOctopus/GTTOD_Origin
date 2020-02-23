using UnityEngine;

namespace Pixelplacement
{
	[ExecuteInEditMode]
	public class SplineAnchor : MonoBehaviour
	{
		public TangentMode tangentMode;

		private bool _initialized;

		[SerializeField]
		[HideInInspector]
		private Transform _masterTangent;

		[SerializeField]
		[HideInInspector]
		private Transform _slaveTangent;

		private TangentMode _previousTangentMode;

		private Vector3 _previousInPosition;

		private Vector3 _previousOutPosition;

		private Vector3 _previousAnchorPosition;

		private Bounds _skinnedBounds;

		private Transform _anchor;

		private Transform _inTangent;

		private Transform _outTangent;

		public bool RenderingChange
		{
			get;
			set;
		}

		public bool Changed
		{
			get;
			set;
		}

		public Transform Anchor
		{
			get
			{
				if (!_initialized)
				{
					Initialize();
				}
				return _anchor;
			}
			private set
			{
				_anchor = value;
			}
		}

		public Transform InTangent
		{
			get
			{
				if (!_initialized)
				{
					Initialize();
				}
				return _inTangent;
			}
			private set
			{
				_inTangent = value;
			}
		}

		public Transform OutTangent
		{
			get
			{
				if (!_initialized)
				{
					Initialize();
				}
				return _outTangent;
			}
			private set
			{
				_outTangent = value;
			}
		}

		private void Awake()
		{
			Initialize();
		}

		private void Update()
		{
			base.transform.localScale = Vector3.one;
			if (!_initialized)
			{
				Initialize();
			}
			Anchor.localPosition = Vector3.zero;
			if (_previousAnchorPosition != base.transform.position)
			{
				Changed = true;
				RenderingChange = true;
				_previousAnchorPosition = base.transform.position;
			}
			if (_previousTangentMode != tangentMode)
			{
				Changed = true;
				RenderingChange = true;
				TangentChanged();
				_previousTangentMode = tangentMode;
			}
			if (InTangent.localPosition != _previousInPosition)
			{
				Changed = true;
				RenderingChange = true;
				_previousInPosition = InTangent.localPosition;
				_masterTangent = InTangent;
				_slaveTangent = OutTangent;
				TangentChanged();
			}
			else if (OutTangent.localPosition != _previousOutPosition)
			{
				Changed = true;
				RenderingChange = true;
				_previousOutPosition = OutTangent.localPosition;
				_masterTangent = OutTangent;
				_slaveTangent = InTangent;
				TangentChanged();
			}
		}

		private void TangentChanged()
		{
			switch (tangentMode)
			{
			case TangentMode.Mirrored:
			{
				Vector3 b = _masterTangent.position - base.transform.position;
				_slaveTangent.position = base.transform.position - b;
				break;
			}
			case TangentMode.Aligned:
			{
				float d = Vector3.Distance(_slaveTangent.position, base.transform.position);
				Vector3 normalized = (_masterTangent.position - base.transform.position).normalized;
				_slaveTangent.position = base.transform.position - normalized * d;
				break;
			}
			}
			_previousInPosition = InTangent.localPosition;
			_previousOutPosition = OutTangent.localPosition;
		}

		private void Initialize()
		{
			_initialized = true;
			InTangent = base.transform.GetChild(0);
			OutTangent = base.transform.GetChild(1);
			Anchor = base.transform.GetChild(2);
			_masterTangent = InTangent;
			_slaveTangent = OutTangent;
			Anchor.hideFlags = HideFlags.HideInHierarchy;
			Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].sharedMaterial.hideFlags = HideFlags.HideInInspector;
			}
			MeshFilter[] componentsInChildren2 = GetComponentsInChildren<MeshFilter>();
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].hideFlags = HideFlags.HideInInspector;
			}
			MeshRenderer[] componentsInChildren3 = GetComponentsInChildren<MeshRenderer>();
			for (int i = 0; i < componentsInChildren3.Length; i++)
			{
				componentsInChildren3[i].hideFlags = HideFlags.HideInInspector;
			}
			SkinnedMeshRenderer[] componentsInChildren4 = GetComponentsInChildren<SkinnedMeshRenderer>();
			for (int i = 0; i < componentsInChildren4.Length; i++)
			{
				componentsInChildren4[i].hideFlags = HideFlags.HideInInspector;
			}
			_previousTangentMode = tangentMode;
			_previousInPosition = InTangent.localPosition;
			_previousOutPosition = OutTangent.localPosition;
			_previousAnchorPosition = base.transform.position;
		}

		public void SetTangentStatus(bool inStatus, bool outStatus)
		{
			InTangent.gameObject.SetActive(inStatus);
			OutTangent.gameObject.SetActive(outStatus);
		}

		public void Tilt(Vector3 angles)
		{
			_ = base.transform.localRotation;
			base.transform.Rotate(angles);
			Vector3 position = InTangent.position;
			Vector3 position2 = OutTangent.position;
			InTangent.position = position;
			OutTangent.position = position2;
		}
	}
}
