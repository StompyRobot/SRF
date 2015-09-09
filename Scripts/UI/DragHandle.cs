using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SRF.UI
{
	public class DragHandle : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
	{

		public float MaxSize = -1;

		public LayoutElement TargetLayoutElement;

		public RectTransform TargetRectTransform;

		public RectTransform.Axis Axis = RectTransform.Axis.Horizontal;

		public bool Invert = false;

		private float Mult {get { return Invert ? -1 : 1; } }

		private float _startValue;
		private float _delta;

		private CanvasScaler _canvasScaler;

		void Start()
		{
			Verify();
			_canvasScaler = GetComponentInParent<CanvasScaler>();
		}

		bool Verify()
		{
			if (TargetLayoutElement == null && TargetRectTransform == null) {
				Debug.LogWarning("DragHandle: TargetLayoutElement and TargetRectTransform are both null. Disabling behaviour.");
				enabled = false;
				return false;
			}

			return true;
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (!Verify()) return;

			//Debug.Log("OnBeginDrag");

			_startValue = GetCurrentValue();
			_delta = 0;
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (!Verify()) return;

			//Debug.Log("OnEndDrag");

			SetCurrentValue(Mathf.Max(_startValue + _delta, GetMinSize()));
			_delta = 0;
			CommitCurrentValue();
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (!Verify()) return;

			//Debug.Log("OnDrag");

			var delta = 0f;

			if (Axis == RectTransform.Axis.Horizontal)
				delta += eventData.delta.x;
			else
				delta += eventData.delta.y;

			if (_canvasScaler != null)
				delta /= _canvasScaler.scaleFactor;

			delta *= Mult;
			_delta += delta;

			SetCurrentValue(Mathf.Clamp(_startValue + _delta, GetMinSize(), GetMaxSize()));
		}

		private float GetCurrentValue()
		{

			if (TargetLayoutElement != null) {

				return Axis == RectTransform.Axis.Horizontal
					? TargetLayoutElement.preferredWidth
					: TargetLayoutElement.preferredHeight;

			}

			if (TargetRectTransform != null) {

				return Axis == RectTransform.Axis.Horizontal
					? TargetRectTransform.sizeDelta.x
					: TargetRectTransform.sizeDelta.y;

			}

			throw new InvalidOperationException();

		}

		private void SetCurrentValue(float value)
		{

			if (TargetLayoutElement != null) {

				if (Axis == RectTransform.Axis.Horizontal) {
					TargetLayoutElement.preferredWidth = value;
				} else {
					TargetLayoutElement.preferredHeight = value;
				}

				return;

			}

			if (TargetRectTransform != null) {

				var d = TargetRectTransform.sizeDelta;

				if (Axis == RectTransform.Axis.Horizontal) {
					d.x = value;
				} else {
					d.y = value;
				}

				TargetRectTransform.sizeDelta = d;

				return;

			}

			throw new InvalidOperationException();

		}

		private void CommitCurrentValue()
		{

			if (TargetLayoutElement != null) {

				if (Axis == RectTransform.Axis.Horizontal) {
					TargetLayoutElement.preferredWidth = ((RectTransform)TargetLayoutElement.transform).sizeDelta.x;
				} else {
					TargetLayoutElement.preferredHeight = ((RectTransform)TargetLayoutElement.transform).sizeDelta.y;
				}
			}

		}

		private float GetMinSize()
		{
			if (TargetLayoutElement == null) return 0;
			return Axis == RectTransform.Axis.Horizontal ? TargetLayoutElement.minWidth : TargetLayoutElement.minHeight;
		}

		private float GetMaxSize()
		{
			if (MaxSize > 0)
				return MaxSize;
			return float.MaxValue;
		}

	}
}
