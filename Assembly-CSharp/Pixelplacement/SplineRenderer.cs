using UnityEngine;

namespace Pixelplacement
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(LineRenderer))]
	[RequireComponent(typeof(Spline))]
	public class SplineRenderer : MonoBehaviour
	{
		public int segmentsPerCurve = 25;

		[Range(0f, 1f)]
		public float startPercentage;

		[Range(0f, 1f)]
		public float endPercentage = 1f;

		private LineRenderer _lineRenderer;

		private Spline _spline;

		private bool _initialized;

		private int _previousAnchorsLength;

		private int _previousSegmentsPerCurve;

		private int _vertexCount;

		private float _previousStart;

		private float _previousEnd;

		private void Reset()
		{
			_lineRenderer = GetComponent<LineRenderer>();
			_initialized = false;
			_lineRenderer.startWidth = 0.03f;
			_lineRenderer.endWidth = 0.03f;
			_lineRenderer.startColor = Color.white;
			_lineRenderer.endColor = Color.yellow;
			_lineRenderer.material = (Resources.Load("SplineRenderer") as Material);
		}

		private void Update()
		{
			if (!_initialized)
			{
				_lineRenderer = GetComponent<LineRenderer>();
				_spline = GetComponent<Spline>();
				ConfigureLineRenderer();
				UpdateLineRenderer();
				_initialized = true;
			}
			if (segmentsPerCurve != _previousSegmentsPerCurve || _previousAnchorsLength != _spline.Anchors.Length)
			{
				ConfigureLineRenderer();
				UpdateLineRenderer();
			}
			if (_spline.Anchors.Length <= 1)
			{
				_lineRenderer.positionCount = 0;
				return;
			}
			SplineAnchor[] anchors = _spline.Anchors;
			foreach (SplineAnchor splineAnchor in anchors)
			{
				if (splineAnchor.RenderingChange)
				{
					splineAnchor.RenderingChange = false;
					UpdateLineRenderer();
				}
			}
			if (startPercentage != _previousStart || endPercentage != _previousEnd)
			{
				UpdateLineRenderer();
				_previousStart = startPercentage;
				_previousEnd = endPercentage;
			}
		}

		private void UpdateLineRenderer()
		{
			if (_spline.Anchors.Length >= 2)
			{
				for (int i = 0; i < _vertexCount; i++)
				{
					float t = (float)i / (float)(_vertexCount - 1);
					float percentage = Mathf.Lerp(startPercentage, endPercentage, t);
					_lineRenderer.SetPosition(i, _spline.GetPosition(percentage));
				}
			}
		}

		private void ConfigureLineRenderer()
		{
			segmentsPerCurve = Mathf.Max(0, segmentsPerCurve);
			_vertexCount = segmentsPerCurve * (_spline.Anchors.Length - 1) + 2;
			if (Mathf.Sign(_vertexCount) == 1f)
			{
				_lineRenderer.positionCount = _vertexCount;
			}
			_previousSegmentsPerCurve = segmentsPerCurve;
			_previousAnchorsLength = _spline.Anchors.Length;
		}
	}
}
