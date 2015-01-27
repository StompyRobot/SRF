using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SRF.UI
{

	[AddComponentMenu(Internal.ComponentMenuPaths.LongPressButton)]
	public class LongPressButton : Button
	{

		public float LongPressDuration = 0.9f;

		private bool _pressed;
		private bool _handled;
		private float _pressedTime;

		[SerializeField] private Button.ButtonClickedEvent _onLongPress = new ButtonClickedEvent();

		public ButtonClickedEvent onLongPress
		{
			get
			{
				return _onLongPress;
			}
			set
			{
				_onLongPress = value;
			}
		}

		public override void OnPointerExit(PointerEventData eventData)
		{

			base.OnPointerExit(eventData);
			_pressed = false;

		}

		public override void OnPointerDown(PointerEventData eventData)
		{

			base.OnPointerDown(eventData);

			if (eventData.button != PointerEventData.InputButton.Left)
				return;

			_pressed = true;
			_handled = false;
			_pressedTime = Time.realtimeSinceStartup;

		}

		public override void OnPointerUp(PointerEventData eventData)
		{

			if (!_handled) {
				base.OnPointerUp(eventData);
			}

			_pressed = false;


		}

		public override void OnPointerClick(PointerEventData eventData)
		{

			if (!_handled) {
				base.OnPointerClick(eventData);
			}

		}

		void Update()
		{

			if (!_pressed)
				return;

			if (Time.realtimeSinceStartup - _pressedTime >= LongPressDuration) {

				_pressed = false;
				_handled = true;
				onLongPress.Invoke();

			}

		}

	}

}