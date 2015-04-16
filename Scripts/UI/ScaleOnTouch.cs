using SRF.Internal;
using UnityEngine;
using UnityEngine.EventSystems;

#if ENABLE_4_6_FEATURES

namespace SRF.UI
{

	[AddComponentMenu(ComponentMenuPaths.ScaleOnTouch)]
	public class ScaleOnTouch : SRMonoBehaviourEx, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IDragHandler
	{

		public float InteractSpeed = 15f;
		public float ResetSpeed = 5f; 

		public Vector3 Scale = Vector3.one;

		[Tooltip("If true, multiply the initial scale by the Scale field. If false, use the Scale field as the absolute scale")]
		public bool Mult = true;

		private Vector3 _initialScale;

		private bool _isDown;

		protected override void Start()
		{

			base.Start();

			_initialScale = CachedTransform.localScale;

		}

		protected override void Update()
		{

			base.Update();

			var speed = ResetSpeed;
			var targetScale = _initialScale;

			if (_isDown) {

				speed = InteractSpeed;

				if (Mult)
					targetScale =  Vector3Extensions.Multiply(_initialScale, Scale);
				else
					targetScale = Scale;

			}

			CachedTransform.localScale = SRMath.SpringLerp(CachedTransform.localScale, targetScale, speed,
				Time.unscaledDeltaTime);

		}

		public void OnPointerDown(PointerEventData eventData)
		{
			_isDown = true;
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			_isDown = false;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			_isDown = false;
		}

		public void OnDrag(PointerEventData eventData)
		{
			_isDown = true;
		}

	}
}


#endif