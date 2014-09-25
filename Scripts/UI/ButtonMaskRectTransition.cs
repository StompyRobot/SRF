using DG.Tweening;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SRF.UI
{

	[RequireComponent(typeof(RectTransform))]
	public class ButtonMaskRectTransition : SRMonoBehaviourEx
	{

		[HideInInspector]
		[RequiredField(true)]
		public RectTransform RectTransform;

		[RequiredField(true)]
		public CanvasGroup CanvasGroup;

		public float Duration = 1.0f;

		public Ease Ease = Ease.OutQuint;

		public bool ModifySource = true;

		[RequiredField(true)]
		public Mask Mask;

		public bool HideOnLoad = true;

		private RectTransform _source;

		private Vector3 _maskPosition;
		private Vector2 _maskAnchorPosition;
		private Vector2 _maskSizeDelta;

		private Vector3 _sourcePosition;
		private Vector2 _sourceAnchorPosition;
		private Vector2 _sourceSizeDelta;
		private LayoutGroup _sourceLayoutGroup;


		private bool _hasStarted;

		private bool _isOpen;

		private SRList<Tween> _activeTweens = new SRList<Tween>(); 

		protected override void Start()
		{

			base.Awake();

			if (_hasStarted)
				return;

			_maskAnchorPosition = RectTransform.anchoredPosition;
			_maskSizeDelta = RectTransform.sizeDelta;
			_maskPosition = RectTransform.position;

			_hasStarted = true;

			if (HideOnLoad) {

				CanvasGroup.blocksRaycasts = false;
				CanvasGroup.interactable = false;
				CanvasGroup.alpha = 0;

			}

		}

		public void Reset()
		{

			RectTransform.anchoredPosition = _maskAnchorPosition;
			RectTransform.sizeDelta = _maskSizeDelta;
			RectTransform.position = _maskPosition;

			CanvasGroup.alpha = 1.0f;

		}

		public void ExpandFrom(RectTransform source)
		{

			StopTweens();
			Debug.Log("Expand");

			if (!_hasStarted)
				Start();

			if (_isOpen) {
				Close();
				return;
			}

			_isOpen = true;

			// Only save new source position if the source is different from the previous source
			// in case of activation during a transition
			if (_source != source) {

				// Save source button position (for return transition later)
				_sourcePosition = source.position;
				_sourceAnchorPosition = source.anchoredPosition;
				_sourceSizeDelta = source.sizeDelta;
				_sourceLayoutGroup = source.GetComponentInParent<LayoutGroup>();

				if (_sourceLayoutGroup != null) {

					if (_sourceLayoutGroup.enabled == false)
						_sourceLayoutGroup = null;
					else
						_sourceLayoutGroup.enabled = false;

				}

			}

			// If modifying the source button
			if (ModifySource) {

				_source = source;


				// Tween button to fill graphic rect
				_activeTweens.Add(DOTween.To(() => _source.position, value => _source.position = value,
					_maskPosition,
					AnimationTimings.GetGUITiming(Duration)).SetEase(Ease).Play());

				_activeTweens.Add(
					DOTween.To(() => _source.sizeDelta, value => _source.sizeDelta = value, _maskSizeDelta,
						AnimationTimings.GetGUITiming(Duration)).SetEase(Ease).Play());

				var sourceCanvasGroup = _source.GetComponent<CanvasGroup>();

				if (sourceCanvasGroup != null) {

					sourceCanvasGroup.blocksRaycasts = false;
					sourceCanvasGroup.interactable = false;

					_activeTweens.Add(DOTween.To(() => sourceCanvasGroup.alpha, value => sourceCanvasGroup.alpha = value, 0,
						AnimationTimings.GetGUITiming(Duration)).SetEase(Ease).Play());

				}
				 
			}

			RectTransform.position = source.position;
			RectTransform.sizeDelta = source.sizeDelta;

			_activeTweens.Add(DOTween.To(() => RectTransform.anchoredPosition, value => RectTransform.anchoredPosition = value, _maskAnchorPosition,
				AnimationTimings.GetGUITiming(Duration)).SetEase(Ease).Play());

			_activeTweens.Add(DOTween.To(() => RectTransform.sizeDelta, value => RectTransform.sizeDelta = value, _maskSizeDelta,
				AnimationTimings.GetGUITiming(Duration)).SetEase(Ease).Play());

			_activeTweens.Add(DOTween.To(() => CanvasGroup.alpha, value => CanvasGroup.alpha = value, 1f,
	AnimationTimings.GetGUITiming(Duration)).SetEase(Ease).Play());
			CanvasGroup.blocksRaycasts = true;
			CanvasGroup.interactable = true;

			DOTween.Play();

		}

		public void Close()
		{

			StopTweens();

			// If modifying the source button
			if (_source != null) {

				_activeTweens.Add(DOTween.To(() => _source.anchoredPosition, value => _source.anchoredPosition = value,
					_sourceAnchorPosition,
					AnimationTimings.GetGUITiming(Duration)).SetEase(Ease).OnComplete(() => {
						
						if (_sourceLayoutGroup != null)
							_sourceLayoutGroup.enabled = true;

						_source = null;

					}).Play());

				_activeTweens.Add(DOTween.To(() => _source.sizeDelta, value => _source.sizeDelta = value,
					_sourceSizeDelta,
					AnimationTimings.GetGUITiming(Duration)).SetEase(Ease).Play());

				var sourceCanvasGroup = _source.GetComponent<CanvasGroup>();

				if (sourceCanvasGroup != null) {

					sourceCanvasGroup.blocksRaycasts = true;
					sourceCanvasGroup.interactable = true;

					_activeTweens.Add(DOTween.To(() => sourceCanvasGroup.alpha, value => sourceCanvasGroup.alpha = value, 1f,
						AnimationTimings.GetGUITiming(Duration)).SetEase(Ease).Play());

				}

			}

			_activeTweens.Add(DOTween.To(() => RectTransform.position, value => RectTransform.position = value, _sourcePosition,
				AnimationTimings.GetGUITiming(Duration)).SetEase(Ease).Play());

			_activeTweens.Add(DOTween.To(() => RectTransform.sizeDelta, value => RectTransform.sizeDelta = value, _sourceSizeDelta,
				AnimationTimings.GetGUITiming(Duration)).SetEase(Ease).Play());

			_activeTweens.Add(DOTween.To(() => CanvasGroup.alpha, value => CanvasGroup.alpha = value, 0f,
	AnimationTimings.GetGUITiming(Duration)).SetEase(Ease).Play());
			CanvasGroup.blocksRaycasts = false;
			CanvasGroup.interactable = false;

			_isOpen = false;


		}

		void StopTweens()
		{

			for (var i = 0; i < _activeTweens.Count; i++) {
				
				if(_activeTweens[i].IsPlaying())
					_activeTweens[i].Kill();

			}

			_activeTweens.Clear();

		}

	}

}
