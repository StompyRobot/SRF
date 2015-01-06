﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SRF.UI
{

	public class SRNumberButton : Button, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
	{

		private const float ExtraThreshold = 3f;

		public SRNumberSpinner TargetField;

		public int Amount = 1;

		public const float Delay = 0.4f;

		private bool _isDown;
		private float _delayTime;
		private float _downTime;

		public override void OnPointerDown(PointerEventData eventData)
		{

			base.OnPointerDown(eventData);

			Apply();

			_isDown = true;
			_downTime = Time.realtimeSinceStartup;
			_delayTime = _downTime + Delay;

		}

		public override void OnPointerUp(PointerEventData eventData)
		{

			base.OnPointerUp(eventData);

			_isDown = false;

		}

		protected virtual void Update()
		{

			if (_isDown) {

				if (_delayTime <= Time.realtimeSinceStartup) {

					Apply();

					var newDelay = Delay*0.5f;

					var extra = Mathf.RoundToInt((Time.realtimeSinceStartup-_downTime)/ExtraThreshold);
					Debug.Log(extra);
					for (var i = 0; i < extra; i++) {
						newDelay *= 0.5f;
					}
					Debug.Log(newDelay);
					_delayTime = Time.realtimeSinceStartup + newDelay;

				}

			}

		}

		private void Apply()
		{

			var currentValue = double.Parse(TargetField.text);
			currentValue += Amount;

			if (currentValue > TargetField.MaxValue)
				currentValue = TargetField.MaxValue;
			if (currentValue < TargetField.MinValue)
				currentValue = TargetField.MinValue;

			TargetField.text = currentValue.ToString();
			TargetField.onEndEdit.Invoke(TargetField.text);

		}

	}

}