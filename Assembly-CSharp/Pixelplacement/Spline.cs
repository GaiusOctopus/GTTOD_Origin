using System;
using UnityEngine;

namespace Pixelplacement
{
	[ExecuteInEditMode]
	public class Spline : MonoBehaviour
	{
		public Color color = Color.yellow;

		[Range(0f, 1f)]
		public float toolScale = 0.1f;

		public TangentMode defaultTangentMode;

		public SplineDirection direction;

		public bool loop;

		public SplineFollower[] followers;

		private SplineAnchor[] _anchors;

		private int _curveCount;

		private int _previousAnchorCount;

		private int _previousChildCount;

		private bool _wasLooping;

		private bool _previousLoopChoice;

		private bool _anchorsChanged;

		private SplineDirection _previousDirection;

		private float _curvePercentage;

		private int _operatingCurve;

		private float _currentCurve;

		[SerializeField]
		[Tooltip("Optimize by removing all renderers in built projects.")]
		private bool _removeRenderers = true;

		[Tooltip("Visualizes how fast or slow a curve segment will be. This operation is expensive so turn it off when not needed.")]
		[SerializeField]
		private bool _showVelocityTicks;

		[Tooltip("How many velocity ticks to show per curve segment.")]
		[SerializeField]
		private int _velocityTickCount = 20;

		[SerializeField]
		[Range(0f, 1f)]
		private float _velocityTickScale = 0.5f;

		public SplineAnchor[] Anchors
		{
			get
			{
				if (loop != _wasLooping)
				{
					_previousAnchorCount = -1;
					_wasLooping = loop;
				}
				if (!loop)
				{
					if (base.transform.childCount != _previousAnchorCount || base.transform.childCount == 0)
					{
						_anchors = GetComponentsInChildren<SplineAnchor>();
						_previousAnchorCount = base.transform.childCount;
					}
					return _anchors;
				}
				if (base.transform.childCount != _previousAnchorCount || base.transform.childCount == 0)
				{
					_anchors = GetComponentsInChildren<SplineAnchor>();
					Array.Resize(ref _anchors, _anchors.Length + 1);
					_anchors[_anchors.Length - 1] = _anchors[0];
					_previousAnchorCount = base.transform.childCount;
				}
				return _anchors;
			}
		}

		public Color SecondaryColor => Color.Lerp(color, Color.black, 0.2f);

		public event Action OnSplineShapeChanged;

		private void OnDrawGizmos()
		{
			if (_showVelocityTicks)
			{
				for (int i = 0; i < _velocityTickCount; i++)
				{
					float percentage = (float)i / (float)_velocityTickCount;
					Gizmos.color = SecondaryColor;
					Gizmos.DrawSphere(GetPosition(percentage), toolScale * _velocityTickScale);
				}
			}
		}

		private void Awake()
		{
			Reset();
			if (!Application.isEditor && _removeRenderers)
			{
				MeshFilter[] componentsInChildren = GetComponentsInChildren<MeshFilter>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					UnityEngine.Object.Destroy(componentsInChildren[i]);
				}
				MeshRenderer[] componentsInChildren2 = GetComponentsInChildren<MeshRenderer>();
				for (int i = 0; i < componentsInChildren2.Length; i++)
				{
					UnityEngine.Object.Destroy(componentsInChildren2[i]);
				}
				SkinnedMeshRenderer[] componentsInChildren3 = GetComponentsInChildren<SkinnedMeshRenderer>();
				for (int i = 0; i < componentsInChildren3.Length; i++)
				{
					UnityEngine.Object.Destroy(componentsInChildren3[i]);
				}
			}
		}

		private void Reset()
		{
			if (Anchors.Length < 2)
			{
				AddAnchors(2 - Anchors.Length);
			}
		}

		private void Update()
		{
			if (followers != null && followers.Length != 0 && Anchors.Length >= 2)
			{
				bool flag = false;
				if (_anchorsChanged || _previousChildCount != base.transform.childCount || direction != _previousDirection || loop != _previousLoopChoice)
				{
					_previousChildCount = base.transform.childCount;
					_previousLoopChoice = loop;
					_previousDirection = direction;
					_anchorsChanged = false;
					flag = true;
				}
				for (int i = 0; i < followers.Length; i++)
				{
					if (followers[i].WasMoved | flag)
					{
						followers[i].UpdateOrientation(this);
					}
				}
			}
			if (Anchors.Length <= 1)
			{
				return;
			}
			for (int j = 0; j < Anchors.Length; j++)
			{
				if (Anchors[j].Changed)
				{
					if (this.OnSplineShapeChanged != null)
					{
						this.OnSplineShapeChanged();
					}
					Anchors[j].Changed = false;
					_anchorsChanged = true;
				}
				if (!loop)
				{
					if (j == 0)
					{
						Anchors[j].SetTangentStatus(inStatus: false, outStatus: true);
					}
					else if (j == Anchors.Length - 1)
					{
						Anchors[j].SetTangentStatus(inStatus: true, outStatus: false);
					}
					else
					{
						Anchors[j].SetTangentStatus(inStatus: true, outStatus: true);
					}
				}
				else
				{
					Anchors[j].SetTangentStatus(inStatus: true, outStatus: true);
				}
			}
		}

		public Vector3 Up(float percentage)
		{
			return Quaternion.LookRotation(GetDirection(percentage)) * Vector3.up;
		}

		public Vector3 Right(float percentage)
		{
			return Quaternion.LookRotation(GetDirection(percentage)) * Vector3.right;
		}

		public Vector3 Forward(float percentage)
		{
			return GetDirection(percentage);
		}

		public Vector3 GetDirection(float percentage)
		{
			CurveDetail curve = GetCurve(percentage);
			if (curve.currentCurve < 0)
			{
				return Vector3.zero;
			}
			SplineAnchor splineAnchor = Anchors[curve.currentCurve];
			SplineAnchor splineAnchor2 = Anchors[curve.currentCurve + 1];
			return BezierCurves.GetFirstDerivative(splineAnchor.Anchor.position, splineAnchor2.Anchor.position, splineAnchor.OutTangent.position, splineAnchor2.InTangent.position, curve.currentCurvePercentage).normalized;
		}

		public Vector3 GetPosition(float percentage, bool evenDistribution = true, int distributionSteps = 100)
		{
			CurveDetail curve = GetCurve(percentage);
			if (curve.currentCurve < 0)
			{
				return Vector3.zero;
			}
			SplineAnchor splineAnchor = Anchors[curve.currentCurve];
			SplineAnchor splineAnchor2 = Anchors[curve.currentCurve + 1];
			return BezierCurves.GetPoint(splineAnchor.Anchor.position, splineAnchor2.Anchor.position, splineAnchor.OutTangent.position, splineAnchor2.InTangent.position, curve.currentCurvePercentage, evenDistribution, distributionSteps);
		}

		public Vector3 GetPosition(float percentage, Vector3 relativeOffset, bool evenDistribution = true, int distributionSteps = 100)
		{
			Vector3 position = GetPosition(percentage, evenDistribution, distributionSteps);
			Quaternion rotation = Quaternion.LookRotation(GetDirection(percentage));
			Vector3 a = rotation * Vector3.up;
			Vector3 a2 = rotation * Vector3.right;
			Vector3 a3 = rotation * Vector3.forward;
			return position + a2 * relativeOffset.x + a * relativeOffset.y + a3 * relativeOffset.z;
		}

		public float ClosestPoint(Vector3 point, int divisions = 100)
		{
			if (divisions <= 0)
			{
				divisions = 1;
			}
			float num = float.MaxValue;
			_ = Vector3.zero;
			Vector3 zero = Vector3.zero;
			float result = 0f;
			float num2 = 0f;
			float num3 = 0f;
			for (float num4 = 0f; num4 < (float)(divisions + 1); num4 += 1f)
			{
				num2 = num4 / (float)divisions;
				num3 = (GetPosition(num2) - point).sqrMagnitude;
				if (num3 < num)
				{
					num = num3;
					result = num2;
				}
			}
			return result;
		}

		public GameObject[] AddAnchors(int count)
		{
			GameObject original = Resources.Load("Anchor") as GameObject;
			if (Anchors.Length == 0)
			{
				count = 2;
			}
			GameObject[] array = new GameObject[count];
			for (int i = 0; i < count; i++)
			{
				Transform transform = null;
				Transform transform2 = null;
				if (Anchors.Length == 1)
				{
					transform = base.transform;
					transform2 = Anchors[0].transform;
				}
				else if (Anchors.Length > 1)
				{
					transform = Anchors[Anchors.Length - 2].transform;
					transform2 = Anchors[Anchors.Length - 1].transform;
				}
				GameObject gameObject = UnityEngine.Object.Instantiate(original);
				gameObject.name = gameObject.name.Replace("(Clone)", "");
				SplineAnchor component = gameObject.GetComponent<SplineAnchor>();
				component.tangentMode = defaultTangentMode;
				gameObject.transform.parent = base.transform;
				gameObject.transform.rotation = Quaternion.LookRotation(base.transform.forward);
				component.InTangent.Translate(Vector3.up * 0.5f);
				component.OutTangent.Translate(Vector3.up * -0.5f);
				if (transform != null && transform2 != null)
				{
					Vector3 vector = (transform2.position - transform.position).normalized;
					if (vector == Vector3.zero)
					{
						vector = base.transform.forward;
					}
					gameObject.transform.position = transform2.transform.position + vector * 1.5f;
				}
				else
				{
					gameObject.transform.localPosition = Vector3.zero;
				}
				array[i] = gameObject;
			}
			return array;
		}

		public CurveDetail GetCurve(float percentage)
		{
			percentage = ((!loop) ? Mathf.Clamp01(percentage) : Mathf.Repeat(percentage, 1f));
			if (Anchors.Length == 2)
			{
				if (direction == SplineDirection.Backwards)
				{
					percentage = 1f - percentage;
				}
				return new CurveDetail(0, percentage);
			}
			_curveCount = Anchors.Length - 1;
			_currentCurve = (float)_curveCount * percentage;
			if ((int)_currentCurve == _curveCount)
			{
				_currentCurve = _curveCount - 1;
				_curvePercentage = 1f;
			}
			else
			{
				_curvePercentage = _currentCurve - (float)(int)_currentCurve;
			}
			_currentCurve = (int)_currentCurve;
			_operatingCurve = (int)_currentCurve;
			if (direction == SplineDirection.Backwards)
			{
				_curvePercentage = 1f - _curvePercentage;
				_operatingCurve = _curveCount - 1 - _operatingCurve;
			}
			return new CurveDetail(_operatingCurve, _curvePercentage);
		}
	}
}
