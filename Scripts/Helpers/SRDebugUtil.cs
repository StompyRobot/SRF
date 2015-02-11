using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = System.Object;

public static class SRDebugUtil
{

	public static bool IsFixedUpdate { get; set; }

	public const int LineBufferCount = 512;

	[DebuggerNonUserCode]
	[DebuggerStepThrough]
	public static void AssertNotNull(object value, string message = null, MonoBehaviour instance = null)
	{ 

		if (!EqualityComparer<Object>.Default.Equals(value, null))
			return;

		message = message != null ? "NotNullAssert Failed: {0}".Fmt(message) : "Assert Failed";

		Debug.LogError(message, instance);

		if (instance != null)
			instance.enabled = false;

		throw new NullReferenceException(message);

	}

	[DebuggerNonUserCode]
	[DebuggerStepThrough]
	public static void Assert(bool condition, string message = null, MonoBehaviour instance = null)
	{

		if (condition)
			return;

		message = message != null ? "Assert Failed: {0}".Fmt(message) : "Assert Failed";

		Debug.LogError(message, instance);
		throw new Exception(message);

	}

	[Conditional("UNITY_EDITOR")]
	[DebuggerNonUserCode]
	[DebuggerStepThrough]
	public static void EditorAssertNotNull(object value, string message = null, MonoBehaviour instance = null)
	{
		AssertNotNull(value, message, instance);
	}

	[Conditional("UNITY_EDITOR")]
	[DebuggerNonUserCode]
	[DebuggerStepThrough]
	public static void EditorAssert(bool condition, string message = null, MonoBehaviour instance = null)
	{
		Assert(condition, message, instance);
	}

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

		if(_lineBuffer == null)
			_lineBuffer = new Line[LineBufferCount];

		if(_fixedUpdateBuffer == null)
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