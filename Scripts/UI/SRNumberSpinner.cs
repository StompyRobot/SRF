#if ENABLE_4_6_FEATURES

using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SRF.UI
{

	public class SRNumberSpinner : InputField
	{

		public double MinValue = double.MinValue;
		public double MaxValue = double.MaxValue;

		public float DragSensitivity = 0.01f;

		protected override void Awake()
		{

			base.Awake();

			if (contentType != ContentType.IntegerNumber && contentType != ContentType.DecimalNumber) {

				Debug.LogError("[SRNumberSpinner] contentType must be integer or decimal. Defaulting to integer");
				contentType = ContentType.DecimalNumber;

			}

		}

		private double _dragStartAmount;
		private double _dragStep;
		private double _currentValue;

		public override void OnBeginDrag(PointerEventData eventData)
		{

			//base.OnBeginDrag(eventData);

			_dragStartAmount = double.Parse(text);
			_currentValue = _dragStartAmount;

			var minStep = 1f;

			// Use a larger minimum step for integer numbers, since there are no fractional values
			if (contentType == ContentType.IntegerNumber)
				minStep *= 10;

			_dragStep = Math.Max(minStep, _dragStartAmount*0.05f);

			if (isFocused)
				DeactivateInputField();

		}

		public override void OnDrag(PointerEventData eventData)
		{

			//base.OnDrag(eventData);

			var diff = eventData.delta.x;

			_currentValue += Math.Abs(_dragStep)*diff*DragSensitivity;
			_currentValue = Math.Round(_currentValue, 2);

			if (_currentValue > MaxValue)
				_currentValue = MaxValue;

			if (_currentValue < MinValue)
				_currentValue = MinValue;

			if (contentType == ContentType.IntegerNumber)
				text = ((int)Math.Round(_currentValue)).ToString();
			else
				text = _currentValue.ToString();

		}

		public override void OnEndDrag(PointerEventData eventData)
		{
			//base.OnEndDrag(eventData);

			if (_dragStartAmount != _currentValue) {

				DeactivateInputField();
				SendOnSubmit();

			}

		}

	}

}

#endif