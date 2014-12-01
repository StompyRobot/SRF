#if ENABLE_4_6_FEATURES

using UnityEngine;

namespace SRF.UI
{

	[ExecuteInEditMode]
	[RequireComponent(typeof (RectTransform))]
	public abstract class ResponsiveBase : SRMonoBehaviour
	{

		protected RectTransform RectTransform
		{
			get { return (RectTransform) CachedTransform; }
		}

		private bool _queueRefresh;

		protected void OnEnable()
		{
			_queueRefresh = true;
		}

		protected void OnRectTransformDimensionsChange()
		{
			_queueRefresh = true;
		}

		protected void Update()
		{

#if UNITY_EDITOR

			// Refresh whenever we can in the editor, since layout has quirky update behaviour
			// when not in play mode
			if (!Application.isPlaying) {

				Refresh();
				return;

			}

#endif

			if (_queueRefresh) {
				Refresh();
				_queueRefresh = false;
			}

		}

		protected abstract void Refresh();

		[ContextMenu("Refresh")]
		private void DoRefresh()
		{
			Refresh();
		}

	}

}

#endif
