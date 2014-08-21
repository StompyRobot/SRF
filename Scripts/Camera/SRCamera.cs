using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Holoville.HOTween;
using UnityEngine;
using System.Linq;

/// <summary>
/// Holds references to the camera transforms, and ensures only one camera behaviour is active at a time.
/// </summary>
public sealed class SRCamera : SRMonoBehaviour
{

	public static IList<SRCamera> Cameras { get { return _cameras.AsReadOnly(); } }

	private static readonly SRList<SRCamera> _cameras = new SRList<SRCamera>();

	private static SRCamera _mainCam;

	public static SRCamera Main
	{
		get
		{

			if (_mainCam == null) {
				_mainCam = GetCamera("main");
			}

			return _mainCam;

		}
	}

	public static SRCamera GetCamera(string tag)
	{
		return _cameras.FirstOrDefault(p => String.Equals(p.Tag, tag, StringComparison.CurrentCultureIgnoreCase));
	}

	public string Tag = "";

	/// <summary>
	/// Transform to use for positioning the camera
	/// </summary>
	public Transform CameraPositionTransform;

	/// <summary>
	/// Transform to use for rotating the camera
	/// </summary>
	public Transform CameraRotationTransform;

	/// <summary>
	/// Transform to use for zooming the camera
	/// </summary>
	public Transform CameraZoomTransform;

	/// <summary>
	/// Actual main camera
	/// </summary>
	public Camera Camera;

	public float FieldOfView
	{
		get { return Camera.fieldOfView; }
		set
		{
			for (var i = 0; i < _cameraList.Length; i++) {
				_cameraList[i].fieldOfView = value;
			}
		}
	}

	/// <summary>
	/// Camera anchor position
	/// </summary>
	public Vector3 Position
	{
		get { return CameraPositionTransform.position; }
		set { CameraPositionTransform.position = value; }
	}

	/// <summary>
	/// Camera zoom
	/// </summary>
	public float Zoom
	{
		get
		{

			if (Camera.isOrthoGraphic)
				return Camera.orthographicSize;
			
			return -CameraZoomTransform.localPosition.z;

		}
		set
		{

			if(Camera.isOrthoGraphic)
				Camera.orthographicSize = value;
			else
				CameraZoomTransform.localPosition = new Vector3(0,0,-value);

		}
	}

	public Quaternion Rotation
	{
		get { return CameraRotationTransform.localRotation; }
		set { CameraRotationTransform.localRotation = value; }
	}

	/// <summary>
	/// Get a read-only list of behaviours attached to this camera
	/// </summary>
	public IList<SRCameraBehaviour> Behaviours
	{
		get { return _readOnlyCameraBehaviours; }
	}

	/// <summary>
	/// Currently active camera behaviour
	/// </summary>
	public SRCameraBehaviour ActiveBehavour
	{
		get { return _activeBehavour; }
	}

	public bool IsTweening
	{
		get { return HOTween.IsTweening(this); }
	}

	private SRList<SRCameraBehaviour> _cameraBehaviours = new SRList<SRCameraBehaviour>();
	private Camera[] _cameraList;
	private ReadOnlyCollection<SRCameraBehaviour> _readOnlyCameraBehaviours;
	private SRCameraBehaviour _activeBehavour;

	void Awake()
	{

		_readOnlyCameraBehaviours = _cameraBehaviours.AsReadOnly();

		// Check for existing camera behaviours
		foreach (var beh in GetComponents<SRCameraBehaviour>()) {
			RegisterCameraBehaviour(beh);
		}

		_cameraList = CachedGameObject.GetComponentsInChildren<Camera>();

		_cameras.Add(this);

	}

	void OnDestroy()
	{
		_cameras.Remove(this);
	}

	/// <summary>
	/// Request that the provided camera behaviour be made active.
	/// </summary>
	/// <param name="requester"></param>
	public void MakeActive(SRCameraBehaviour requester)
	{

		if (requester == _activeBehavour)
			return;

		// Keep a reference to the previously active behaviour
		var prev = _activeBehavour;

		// Set new active behaviour, so the OnDisable call for the previous camera can check what new behaviour is active
		_activeBehavour = requester;

		// Disable previous behaviour
		if (prev != null) {
			prev.enabled = false;
		}

		// Enable new behaviour
		_activeBehavour.enabled = true;

	}


	/// <summary>
	/// Get the game plane position under the screen position.
	/// </summary>
	/// <param name="pos">Screen position</param>
	/// <returns>Point on the gameplay plane where ray from screen position intersects</returns>
	public Vector3 ScreenPosToGamePlane(Vector2 pos)
	{

		return ActiveBehavour.ScreenPosToGamePlane(pos, false);

	}

	/// <summary>
	/// Get the camera behaviour of type T. Expensive operation (GC, MS)
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public T GetCameraBehaviour<T>() where T : SRCameraBehaviour
	{

		var type = typeof (T);

		for (int i = 0; i < _cameraBehaviours.Count; i++) {

			// Ensure that it matches the exact type
			if (type == _cameraBehaviours[i].GetType())
				return _cameraBehaviours[i] as T;

		}

		return null;
	}

	public void TweenPosition(Vector3 targetPosition, float duration, EaseType ease)
	{

		TweenProperty("Position", targetPosition, duration, ease);

	}

	public void TweenRotation(Quaternion targetRotation, float duration, EaseType ease)
	{

		TweenProperty("Rotation", targetRotation, duration, ease);

	}

	public void TweenZoom(float targetZoom, float duration, EaseType ease)
	{

		TweenProperty("Zoom", targetZoom, duration, ease);

	}

	public void TweenFov(float targetFov, float duration, EaseType ease)
	{

		TweenProperty("FieldOfView", FieldOfView, duration, ease);

	}

	private void TweenProperty(string prop, object value, float duration, EaseType ease)
	{

		HOTween.To(this, duration,
			new TweenParms().Prop(prop, value)
			                .Ease(ease)
			                .UpdateType(UpdateType.TimeScaleIndependentUpdate));

	}

	public void CancelTweens()
	{

		HOTween.Kill(this);

	}

	/// <summary>
	/// Register a camera behaviour (should only be called by SRCameraBehaviour.Awake())
	/// </summary>
	/// <param name="behaviour"></param>
	internal void RegisterCameraBehaviour(SRCameraBehaviour behaviour)
	{

		if (_cameraBehaviours.Contains(behaviour)) {
			//Debug.LogWarning("Duplicate registration of camera behaviour");
			return;
		}

		_cameraBehaviours.Add(behaviour);

		//if(_activeBehavour == null)
		//	behaviour.Activate();

	}

}
