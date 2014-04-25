using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

/// <summary>
/// Base MonoBehaviour which provides useful common functionality
/// </summary>
public abstract class SRMonoBehaviour : MonoBehaviour
{

	/// <summary>
	/// Get the Transform component, using a cached reference if possible.
	/// </summary>
	public Transform CachedTransform
	{
		[DebuggerStepThrough]
		[DebuggerNonUserCode]
		get
		{
			if (_transform == null) {
				_transform = base.transform;
			}

			return _transform;
		}
	}

	/// <summary>
	/// Get the Collider component, using a cached reference if possible.
	/// </summary>
	public Collider CachedCollider
	{
		[DebuggerStepThrough]
		[DebuggerNonUserCode]
		get
		{
			if (_collider == null) {
				_collider = base.collider;
			}

			return _collider;
		}
	}

	/// <summary>
	/// Get the Rigidbody component, using a cached reference if possible.
	/// </summary>
	public Rigidbody CachedRigidBody
	{
		[DebuggerStepThrough]
		[DebuggerNonUserCode]
		get
		{
			if (_rigidBody == null) {
				_rigidBody = base.rigidbody;
			}

			return _rigidBody;
		}
	}

	/// <summary>
	/// Get the GameObject this behaviour is attached to, using a cached reference if possible.
	/// </summary>
	public GameObject CachedGameObject
	{
		[DebuggerStepThrough]
		[DebuggerNonUserCode]
		get
		{
			if (_gameObject == null) {
				_gameObject = base.gameObject;
			}

			return _gameObject;
		}
	}

	// Override existing getters for legacy usage

	// ReSharper disable InconsistentNaming
	public new Transform transform { get { return CachedTransform; } }
	public new Collider collider { get { return CachedCollider; } }
	public new Rigidbody rigidbody { get { return CachedRigidBody; } }
	public new GameObject gameObject { get { return CachedGameObject; } }
	// ReSharper restore InconsistentNaming

	private Collider _collider;
	private Transform _transform;
	private Rigidbody _rigidBody;
	private GameObject _gameObject;

	/// <summary>
	/// Assert that the value is not null, disable the object and print a debug error message if it is.
	/// </summary>
	/// <param name="value">Object to check</param>
	/// <param name="fieldName">Debug name to pass in</param>
	/// <returns>True if object is not null</returns>
	[DebuggerNonUserCode]
	[DebuggerStepThrough]
	protected void AssertNotNull(object value, string fieldName = null)
	{

		if (!EqualityComparer<System.Object>.Default.Equals(value, null))
			return;

		string message;

		if (fieldName != null)
			message = "Field {0} is null".Fmt(fieldName);
		else
			message = "NotNull assert failed";

		Debug.LogError(message, this);
		enabled = false;

		throw new NullReferenceException(message);

	}

	[DebuggerNonUserCode]
	[DebuggerStepThrough]
	protected void Assert(bool condition, string message = null)
	{

		if (condition)
			return;

		message = message != null ? "Assert Failed: {0}".Fmt(message) : "Assert Failed";

		Debug.LogError(message, this);
		throw new Exception(message);

	}

	/// <summary>
	/// Assert that the value is not null, disable the object and print a debug error message if it is.
	/// </summary>
	/// <param name="value">Object to check</param>
	/// <param name="fieldName">Debug name to pass in</param>
	/// <returns>True if object is not null</returns>
	[Conditional("UNITY_EDITOR")]
	[DebuggerNonUserCode]
	[DebuggerStepThrough]
	protected void EditorAssertNotNull(object value, string fieldName = null)
	{
		AssertNotNull(value, fieldName);
	}

	[Conditional("UNITY_EDITOR")]
	[DebuggerNonUserCode]
	[DebuggerStepThrough]
	protected void EditorAssert(bool condition, string message = null)
	{
		Assert(condition, message);
	}

}
