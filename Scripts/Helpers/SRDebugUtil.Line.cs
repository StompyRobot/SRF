using UnityEngine;
using System.Collections;

public static partial class SRDebugUtil
{

	private struct Line
	{

		public Vector3 Source;
		public Vector3 Dest;

		public Color Color;

	}

	private static Line[] _lineBuffer;
	private static Line[] _fixedUpdateBuffer;

	private static int _currentFrameLines;
	private static int _currentFixedFrameLines;
	private static Material _m;

	public static void DrawLine(Vector3 srcPos, Vector3 endPos, Color color)
	{

		if (_lineBuffer == null)
			_lineBuffer = new Line[LineBufferCount];

		if (_fixedUpdateBuffer == null)
			_fixedUpdateBuffer = new Line[LineBufferCount];

		if (_currentFrameLines >= _lineBuffer.Length) {
			Debug.LogWarning("[SRDebug] Line buffer overflowed");
			return;
		}

		var buffer = IsFixedUpdate ? _fixedUpdateBuffer : _lineBuffer;

		var i = IsFixedUpdate ? _currentFixedFrameLines++ : _currentFrameLines++;

		buffer[i].Source = srcPos;
		buffer[i].Dest = endPos;
		buffer[i].Color = color;

	}

	public static void DrawDebugFrame(Camera c)
	{

		if (_fixedUpdateBuffer == null || _lineBuffer == null)
			return;

		if (_currentFrameLines == 0 && _currentFixedFrameLines == 0)
			return;

		if (_m == null) {
			_m = new Material(Shader.Find("GUI/Text Shader"));
		}

		GL.PushMatrix();
		_m.SetPass(0);
		GL.LoadOrtho();

		GL.Begin(GL.LINES);

		for (var i = 0; i < _currentFrameLines; i++) {


			GL.Color(_lineBuffer[i].Color);
			GL.Vertex(c.WorldToViewportPoint(_lineBuffer[i].Source).xy());

			GL.Color(_lineBuffer[i].Color);
			GL.Vertex(c.WorldToViewportPoint(_lineBuffer[i].Dest).xy());

		}

		for (var i = 0; i < _currentFixedFrameLines; i++) {

			GL.Color(_fixedUpdateBuffer[i].Color);
			GL.Vertex(c.WorldToViewportPoint(_fixedUpdateBuffer[i].Source).xy());

			GL.Color(_fixedUpdateBuffer[i].Color);
			GL.Vertex(c.WorldToViewportPoint(_fixedUpdateBuffer[i].Dest).xy());

		}

		GL.End();

		GL.PopMatrix();

	}

	public static void FlushLines()
	{
		_currentFrameLines = 0;
	}

	public static void FlushFixedUpdateLines()
	{
		_currentFixedFrameLines = 0;
	}

}