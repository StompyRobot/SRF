using System;
using System.Diagnostics;
using UnityEngine;
using System.Collections;
using Debug = UnityEngine.Debug;

/// <summary>
/// Base class for SRAutoSingleton so that the SingletonContainer static field is shared between all generic
/// types.
/// </summary>
public abstract class SRAutoSingletonBase : SRMonoBehaviour
{

	

}

/// <summary>
/// Singleton MonoBehaviour class which automatically creates an instance if one does not already exist.
/// </summary>
public abstract class SRAutoSingleton<T> : SRAutoSingletonBase where T : SRAutoSingleton<T>
{

	private static T _instance = null;
	/// <summary>
	/// Get (or create) the instance of this Singleton
	/// </summary>
	public static T Instance
	{
		[DebuggerStepThrough]
		get
		{
			// Instance required for the first time, we look for it
			if (_instance == null) {
				var go = new GameObject("_" + typeof (T).Name);
				go.AddComponent<T>(); // _instance set by Awake() constructor
			}
			return _instance;
		}
	}

	public static bool HasInstance
	{
		get { return _instance != null; }
	}

	// If no other monobehaviour request the instance in an awake function
	// executing before this one, no need to search the object.
	protected virtual void Awake()
	{

		if (_instance != null) {
			Debug.LogWarning("More than one singleton object of type {0} exists.".Fmt(typeof(T).Name));
			return;
		}

		_instance = (T)this;

	}

	// Make sure the instance isn't referenced anymore when the user quit, just in case.
	private void OnApplicationQuit()
	{
		_instance = null;
	}

}