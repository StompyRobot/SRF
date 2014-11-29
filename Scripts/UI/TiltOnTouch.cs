#if ENABLE_4_6_FEATURES

using UnityEngine;
using UnityEngine.EventSystems;

namespace SRF.UI
{

	[AddComponentMenu(Internal.ComponentMenuPaths.TiltOnTouch)]
	[RequireComponent(typeof(RectTransform))]
	public class TiltOnTouch : SRMonoBehaviourEx, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IDragHandler
	{

		public const float InteractSpeed = 15f;
		public const float ResetSpeed = 5f; 

		public RectTransform RectTransform { get { return (RectTransform) CachedTransform; } }

		public Vector2 TiltRange = new Vector2(20, 10);

		private int? _pointer;
		private Camera _camera;
		private Vector2 _position;

		private Quaternion _targetRotation;

		private Quaternion TargetRotation(Vector2 screenPos, Camera c)
		{

			var prevRot = CachedTransform.localRotation;

			CachedTransform.localRotation = Quaternion.identity;

			var ray = c.ScreenPointToRay(screenPos);
			var worldPos = ray.GetPoint(100);

			var localPos = CachedTransform.InverseTransformPoint(worldPos);

			var w = RectTransform.rect.width * 0.5f;
			var h = RectTransform.rect.height * 0.5f;

			var x = localPos.x/w;
			var y = localPos.y/h;

			var rotX = Mathf.Lerp(-TiltRange.x, TiltRange.x, (x*0.5f) + 0.5f);
			var rotY = Mathf.Lerp(-TiltRange.y, TiltRange.y, (y*0.5f) + 0.5f);

			CachedTransform.localRotation = prevRot;

			return Quaternion.Euler(rotY, -rotX, 0);

		}

		protected override void Update()
		{
			base.Update();

			var speed = ResetSpeed;

			if (_pointer.HasValue) {

				speed = InteractSpeed;
				_targetRotation = TargetRotation(_position, _camera);

			} else {

				_targetRotation = Quaternion.identity;

			}
			
			CachedTransform.localRotation = SRMath.SpringLerp(CachedTransform.localRotation, _targetRotation, speed,
				Time.unscaledDeltaTime);

		}

		public void OnPointerDown(PointerEventData eventData)
		{
			
			_pointer = eventData.pointerId;
			_camera = eventData.pressEventCamera;
			_position = eventData.position;

		}

		public void OnPointerUp(PointerEventData eventData)
		{

			_pointer = null;
			_camera = null;

		}

		public void OnPointerExit(PointerEventData eventData)
		{

			_pointer = null;
			_camera = null;

		}

		public void OnDrag(PointerEventData eventData)
		{

			eventData.pointerDrag = CachedGameObject;
			_position = eventData.position;

		}

	}
}

#endif