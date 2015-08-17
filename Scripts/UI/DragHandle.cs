using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SRF.UI
{
	public class DragHandle : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
	{

		public LayoutElement TargetLayoutElement;

		public RectTransform TargetRectTransform;

		public RectTransform.Axis Axis = RectTransform.Axis.Horizontal;

		private float _startValue;
		private float _delta;
		void Start()
		{
			Verify();
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

			if (Axis == RectTransform.Axis.Horizontal)
				_delta += eventData.delta.x;
			else
				_delta += eventData.delta.y;

			SetCurrentValue(Mathf.Max(_startValue + _delta, GetMinSize()));
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

	}
}