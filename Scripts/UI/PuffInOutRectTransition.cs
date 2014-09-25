using DG.Tweening;
using UnityEngine;

namespace SRF.Scripts.UI
{

	[RequireComponent(typeof(RectTransform))]
	public class PuffInOutRectTransition : SRMonoBehaviourEx
	{

		[RequiredField(true)]
		public RectTransform RectTransform;

		[RequiredField(true)]
		public CanvasGroup CanvasGroup;

		public float Duration = 1.0f;

		public float ScaleMult = 1.1f;

		public Ease Ease = Ease.OutQuint;

		public void Show(bool anim)
		{

			if (anim) {

				RectTransform.DOScale(Vector3.one, AnimationTimings.GetGUITiming(Duration))
				             .SetEase(Ease)
				             .SetUpdate(true);

				DOTween.To(() => CanvasGroup.alpha, value => CanvasGroup.alpha = value, 1f,
					AnimationTimings.GetGUITiming(Duration))
				       .SetEase(Ease)
				       .SetUpdate(true);

				DOTween.Play();

			} else {

				RectTransform.localScale = Vector3.one;
				CanvasGroup.alpha = 1.0f;

			}

		}

		public void Hide(bool anim)
		{

			if (anim) {

				RectTransform.DOScale(Vector3.one * ScaleMult, AnimationTimings.GetGUITiming(Duration))
							 .SetEase(Ease)
							 .SetUpdate(true);

				DOTween.To(() => CanvasGroup.alpha, value => CanvasGroup.alpha = value, 0f,
					AnimationTimings.GetGUITiming(Duration))
					   .SetEase(Ease)
					   .SetUpdate(true);

				DOTween.Play();

			} else {

				RectTransform.localScale = Vector3.one * ScaleMult;
				CanvasGroup.alpha = 0.0f;

			}

		}

	}

}
