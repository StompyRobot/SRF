using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Framework.Components
{
	[RequireComponent(typeof (MeshFilter), typeof (MeshRenderer))]
	public class TangentLineRenderer : SRMonoBehaviour
	{

		private class CustomLineVertex
		{

			public Vector3 Position = Vector3.zero;
			public Color Color = Color.white;
			public float Width = 1.0f;

		}

		private Mesh _mMesh;
		private int[] _mIndices;
		private Vector2[] _mUVs;
		private List<CustomLineVertex> _mPoints;

		private bool _isDirty = false;

		private void Awake()
		{
			_mMesh = new Mesh();
			_mPoints = new List<CustomLineVertex>();
			GetComponent<MeshFilter>().sharedMesh = _mMesh;
		}

		private void Start()
		{
			UpdateMesh(Camera.main);
		}

		private void OnWillRenderObject()
		{
			UpdateMesh(Camera.current);
		}

		private void UpdateMesh(Camera aCamera)
		{

			if (!_isDirty)
				return;

			_isDirty = false;

			Vector3[] vertices = _mMesh.vertices;
			Vector3[] normals = _mMesh.normals;
			Color[] colors = _mMesh.colors;
			Vector3 oldTangent = Vector3.zero;

			for (int i = 0; i < _mPoints.Count - 1; i++) {

				Vector3 faceNormal = Vector3.up;// (localViewPos - m_Points[i].position).normalized;
				Vector3 dir = (_mPoints[i + 1].Position - _mPoints[i].Position);
				Vector3 tangent = Vector3.Cross(dir, faceNormal).normalized;
				Vector3 offset = (oldTangent + tangent).normalized*_mPoints[i].Width/2.0f;

				vertices[i*2] = _mPoints[i].Position - offset;
				vertices[i*2 + 1] = _mPoints[i].Position + offset;
				normals[i*2] = normals[i*2 + 1] = faceNormal;
				colors[i*2] = colors[i*2 + 1] = _mPoints[i].Color;

				if (i == _mPoints.Count - 2) {

					// last two points
					vertices[i*2 + 2] = _mPoints[i + 1].Position - tangent*_mPoints[i + 1].Width/2.0f;
					vertices[i*2 + 3] = _mPoints[i + 1].Position + tangent*_mPoints[i + 1].Width/2.0f;
					normals[i*2 + 2] = normals[i*2 + 3] = faceNormal;
					colors[i*2 + 2] = colors[i*2 + 3] = _mPoints[i + 1].Color;

				}

				oldTangent = tangent;

			}

			_mMesh.vertices = vertices;
			_mMesh.normals = normals;
			_mMesh.colors = colors;
			_mMesh.uv = _mUVs;
			_mMesh.SetTriangleStrip(_mIndices, 0);
			_mMesh.RecalculateBounds();

		}

		public void SetVertexCount(int aCount, float uvCompress)
		{

			if (!Application.isPlaying)
				return;

			aCount = Mathf.Clamp(aCount, 0, 0xFFFF/2);

			if (_mPoints.Count > aCount)
				_mPoints.RemoveRange(aCount, _mPoints.Count - aCount);

			while (_mPoints.Count < aCount)
				_mPoints.Add(new CustomLineVertex());

			_mIndices = new int[_mPoints.Count*2];
			_mUVs = new Vector2[_mPoints.Count*2];

			_mMesh.vertices = new Vector3[_mPoints.Count*2];
			_mMesh.normals = new Vector3[_mPoints.Count*2];
			_mMesh.colors = new Color[_mPoints.Count*2];

			for (int i = 0; i < _mPoints.Count; i++) 
			{
				_mIndices[i*2] = i*2;
				_mIndices[i*2 + 1] = i*2 + 1;
				_mUVs[i*2] = _mUVs[i*2 + 1] = new Vector2(((float) i/(_mPoints.Count - 1))*uvCompress, 0);
				_mUVs[i*2 + 1].y = 1.0f;
			}

			_isDirty = true;

		}

		public void SetPosition(int aIndex, Vector3 aPosition)
		{

			if (aIndex < 0 || aIndex >= _mPoints.Count)
				return;

			_mPoints[aIndex].Position = aPosition;

			_isDirty = true;

		}

		public void SetWidth(int aIndex, float aWidth)
		{

			if (aIndex < 0 || aIndex >= _mPoints.Count)
				return;

			_mPoints[aIndex].Width = aWidth;

			_isDirty = true;

		}

		public void SetColor(int aIndex, Color aColor)
		{

			if (aIndex < 0 || aIndex >= _mPoints.Count)
				return;

			_mPoints[aIndex].Color = aColor;

			_isDirty = true;

		}

		public void SetWidth(float aStartWidth, float aEndWidth)
		{

			for (int i = 0; i < _mPoints.Count; i++) {
				_mPoints[i].Width = Mathf.Lerp(aStartWidth, aEndWidth, (float) i/(_mPoints.Count - 1));
			}

			_isDirty = true;

		}

		public void SetColor(Color aStart, Color aEnd)
		{

			for (int i = 0; i < _mPoints.Count; i++) {
				_mPoints[i].Color = Color.Lerp(aStart, aEnd, (float) i/(_mPoints.Count - 1));
			}

			_isDirty = true;

		}

		void OnDrawGizmos()
		{

			if (_mPoints == null)
				return;

			Gizmos.color = SRColors.Red;

			for (var i = 1; i < _mPoints.Count-1; i++) {

				Gizmos.DrawLine(CachedTransform.TransformPoint(_mPoints[i].Position),
					CachedTransform.TransformPoint(_mPoints[i + 1].Position));

			}

		}

	}
}