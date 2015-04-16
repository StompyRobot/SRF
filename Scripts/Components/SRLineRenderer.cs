using SRF.Internal;
using UnityEngine;

namespace SRF.Components
{

	/// <summary>
	/// Wraps a Unity LineRenderer component. Provides lookup capabilities, as the default
	/// LineRenderer has no GetPosition method.
	/// </summary>
	[AddComponentMenu(ComponentMenuPaths.SRLineRenderer)]
	[RequireComponent(typeof(LineRenderer))]
	public class SRLineRenderer : MonoBehaviour
	{

		private LineRenderer _lineRenderer;
		public LineRenderer LineRenderer { get { return _lineRenderer; } }

		private SRList<Vector3> _points = new SRList<Vector3>();

		// Use this for initialization
		void Awake()
		{

			_lineRenderer = GetComponent<LineRenderer>();

			_points.Add(Vector3.zero);
			_points.Add(Vector3.zero);

		}

		public void SetVertexCount(int size)
		{

			if (_lineRenderer == null)
				Awake();

			if (_points.Count == size)
				return;

			_lineRenderer.SetVertexCount(size);

			while (_points.Count > size) {
				_points.RemoveAt(size - 1);
			}

			while (_points.Count < size) {
				_points.Add(Vector3.zero);
			}

			_points.Trim();

		}

		public void SetPosition(int index, Vector3 pos)
		{

			if (_lineRenderer == null)
				Awake();

			_lineRenderer.SetPosition(index, pos);
			_points[index] = pos;

		}

		public Vector3 GetPosition(int index)
		{

			if (_lineRenderer == null)
				Awake();

			return _points[index];
		}

	}

}