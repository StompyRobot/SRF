using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;
using Debug = UnityEngine.Debug;

public static class SRDebug
{

	[DebuggerNonUserCode]
	[DebuggerStepThrough]
	public static void AssertNotNull(object value, string message = null, SRMonoBehaviour instance = null)
	{ 

		if (!EqualityComparer<System.Object>.Default.Equals(value, null))
			return;

		message = message != null ? "NotNullAssert Failed: {0}".Fmt(message) : "Assert Failed";

		Debug.LogError(message, instance);

		if (instance != null)
			instance.enabled = false;

		throw new NullReferenceException(message);

	}

	[DebuggerNonUserCode]
	[DebuggerStepThrough]
	public static void Assert(bool condition, string message = null, SRMonoBehaviour instance = null)
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
	public static void EditorAssertNotNull(object value, string message = null, SRMonoBehaviour instance = null)
	{
		AssertNotNull(value, message, instance);
	}

	[Conditional("UNITY_EDITOR")]
	[DebuggerNonUserCode]
	[DebuggerStepThrough]
	public static void EditorAssert(bool condition, string message = null, SRMonoBehaviour instance = null)
	{
		Assert(condition, message, instance);
	}

}