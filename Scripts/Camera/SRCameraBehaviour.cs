using UnityEngine;
using System.Collections;

public abstract class SRCameraBehaviour : SRMonoBehaviour
{

	private SRCamera _camera;

	public SRCamera Camera
	{
		get
		{

			if (_camera == null)
				_camera = GetComponent<SRCamera>();

			return _camera;

		}
	}

	/// <summary>
	/// Position the camera is interpolating towards
	/// </summary>
	public virtual Vector3 TargetPosition
	{
		get { return _targetPosition; }
		set { _targetPosition = value; }
	}

	/// <summary>
	/// Angle the camera is rotating towards
	/// </summary>
	public virtual Quaternion TargetRotation
	{
		get { return _targetRotation; }
		set { _targetRotation = value; }
	}

	/// <summary>
	/// Zoom level the camera is interpolating towards
	/// </summary>
	public virtual float TargetZoom
	{
		get { return _targetZoom; }
		set { _targetZoom = value; }
	}

	protected virtual Plane GamePlane { get { return _defaultGamePlane; } }

	private readonly Plane _defaultGamePlane = new Plane(Vector3.up, Vector3.zero);

	private Vector3 _targetPosition;
	private float _targetZoom;
	private Quaternion _targetRotation;

	protected virtual void Awake()
	{
		enabled = false;
		Camera.RegisterCameraBehaviour(this);
		AssertNotNull(Camera, "Camera");
	}

	protected virtual void OnEnable()
	{
		Camera.MakeActive(this);
	}

	protected virtual void OnDisable()
	{
		
	}

	/// <summary>
	/// Activate this camera behaviour
	/// </summary>
	public void Activate()
	{
		Camera.MakeActive(this);
	}

	/// <summary>
	/// Get the game plane position under the screen position.
	/// </summary>
	/// <param name="pos">Screen position</param>
	/// <param name="snap">Snap to target position before raycasting.</param>
	/// <returns>Point on the gameplay plane where ray from screen position intersects</returns>
	public virtual Vector3 ScreenPosToGamePlane(Vector2 pos, bool snap)
	{

		var ray = Camera.Camera.ScreenPointToRay(pos);

		float enter;

		if (!GamePlane.Raycast(ray, out enter))
			return Vector3.zero;

		var worldPos = ray.GetPoint(enter);

		return worldPos;

	}


}
