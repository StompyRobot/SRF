using UnityEngine;
using System.Collections;

/// <summary>
/// Camera behaviour designed for touch input response
/// </summary>
public class SRInputCameraBehaviour : SRCameraBehaviour
{

	protected bool UseVelocitySmoothing;
	protected float SmoothTime = 1f;

	protected bool IsDragging {get { return _isDragging; }}

	public bool EnableEdgeSmoothing = false;

	public float Drag = 1f;

	public float MaxRadius = 1500f;

	public Vector3 BaseRotation = new Vector3(0, 0, 0);
	public Vector3 Origin = new Vector3();

	public float TiltRange = 0f;
	public float YawRange = 0f;

	private float _springForce = 10f;


	public virtual float Zoom { get { return 300f; } }

	public Vector3 Velocity { get; set; }

	private bool _isDragging;
	private Vector3 _posVelocity;

	public virtual void MoveStart()
	{
		Velocity = Vector3.zero;
		_springForce = 30f;
		_isDragging = true;
	}

	public virtual void MoveComplete()
	{
		_springForce = 10f;
		TargetPosition = PositionLimit(TargetPosition);
		_isDragging = false;
	}

	public virtual void MoveDiff(Vector3 diff)
	{

		var newPos = TargetPosition + diff;
		diff *= EdgeSpring(newPos);
		TargetPosition = TargetPosition + diff;
		_isDragging = true;

	}

	protected virtual void Update()
	{

		// Apply velocity and drag to the camera
		if (Velocity.sqrMagnitude > 0) {

			TargetPosition += Velocity * RealTime.deltaTime;

			var newVelocity = (Velocity - (Velocity*(Drag)*RealTime.deltaTime)) * EdgeSpring(TargetPosition);

			if (newVelocity.sqrMagnitude < Velocity.sqrMagnitude)
				Velocity = newVelocity;

			var exceeds = LocalMagnitude(TargetPosition);
			if (exceeds > MaxRadius) {

				Velocity += -LocalPosition(TargetPosition).normalized*(exceeds-MaxRadius) * 3f;

			} else if (Velocity.sqrMagnitude < 0.01f) {

				Velocity = Vector3.zero;

			}

		}

		var targetRotation = CameraRotationForPosition(Camera.Position);

		var t = Quaternion.Euler(targetRotation);

		if (UseVelocitySmoothing) {
			Camera.Position = Vector3.SmoothDamp(Camera.Position, TargetPosition, ref _posVelocity, SmoothTime, 10000f,
				RealTime.deltaTime);
		} else {
			Camera.Position = NGUIMath.SpringLerp(Camera.Position, TargetPosition, _springForce, RealTime.deltaTime);
		}

		Camera.Rotation = NGUIMath.SpringLerp(Camera.Rotation, t, 15f, RealTime.deltaTime);
		Camera.Zoom = NGUIMath.SpringLerp(Camera.Zoom, Zoom, 15f, RealTime.deltaTime);

	}

	/// <summary>
	/// Get the game plane position under the screen position.
	/// </summary>
	/// <param name="pos">Screen position</param>
	/// <param name="snap">Snap to target position before raycasting</param>
	/// <returns>Point on the gameplay plane where ray from screen position intersects</returns>
	public override Vector3 ScreenPosToGamePlane(Vector2 pos, bool snap)
	{

		var origRotation = Camera.CameraRotationTransform.localRotation;
		var origPosition = Camera.Position;

		if (snap) {

			Camera.Position = TargetPosition;
			Camera.CameraRotationTransform.localRotation = Quaternion.Euler(CameraRotationForPosition(Camera.Position));

		}

		var worldPos = base.ScreenPosToGamePlane(pos, snap);

		Camera.Position = origPosition;
		Camera.CameraRotationTransform.localRotation = origRotation;

		return worldPos;

	}

	public virtual Vector3 CameraRotationForPosition(Vector3 position)
	{

		var local = position - Origin;

		var yaw = -YawRange * (local.x / MaxRadius);
		var tilt = TiltRange * (local.z / MaxRadius);

		return new Vector3(BaseRotation.x + tilt, BaseRotation.y + yaw, BaseRotation.z);

	}

	/// <summary>
	/// Limit the position to 
	/// </summary>
	/// <param name="targetPosition"></param>
	/// <param name="smooth">Enable t</param>
	/// <returns></returns>
	protected virtual Vector3 PositionLimit(Vector3 targetPosition)
	{

		return Origin + Vector3.ClampMagnitude(LocalPosition(targetPosition), MaxRadius);

	}

	protected Vector3 LocalPosition(Vector3 targetPosition)
	{
		return targetPosition - Origin;
	}

	protected float LocalMagnitude(Vector3 targetPosition)
	{
		return LocalPosition(targetPosition).magnitude;
	}

	/// <summary>
	/// Return value from 1.0f to 0, 1.0f being inside radius and 0 being outside (smoothly)
	/// </summary>
	/// <param name="pos"></param>
	/// <returns></returns>
	protected virtual float EdgeSpring(Vector3 pos)
	{

		var exceeds = Mathf.Clamp01((LocalMagnitude(pos) - MaxRadius) / (MaxRadius * 0.5f));

		return (1.0f - exceeds);

	}

}
