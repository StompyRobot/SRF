#if ENABLE_UNITYEVENTS

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SRF.UI
{

	[RequireComponent(typeof(RectTransform))]
	[ExecuteInEditMode]
	public class CopyPreferredSize : UIBehaviour, ILayoutElement
	{

		public RectTransform CopySource;

		public float minWidth
		{
			get
			{
				if (CopySource == null || !IsActive())
					return -1f;
				return LayoutUtility.GetMinWidth(CopySource);
			}
		}

		public float preferredWidth
		{
			get
			{
				if (CopySource == null || !IsActive())
					return -1f;
				return LayoutUtility.GetPreferredWidth(CopySource);
			}
		}

		public float flexibleWidth
		{
			get
			{
				if (CopySource == null || !IsActive())
					return -1f;
				return LayoutUtility.GetFlexibleWidth(CopySource);
			}
		}

		public float minHeight
		{
			get
			{
				if (CopySource == null || !IsActive())
					return -1f;
				return LayoutUtility.GetFlexibleHeight(CopySource);
			}
		}

		public float preferredHeight
		{
			get
			{
				if (CopySource == null || !IsActive())
					return -1f;
				return LayoutUtility.GetPreferredHeight(CopySource);
			}
		}

		public float flexibleHeight
		{
			get
			{
				if (CopySource == null || !IsActive())
					return -1f;
				return LayoutUtility.GetFlexibleHeight(CopySource);
			}
		}

		public int layoutPriority
		{
			get { return 1; }
		}


		public void CalculateLayoutInputHorizontal()
		{
			foreach (var component in CopySource.GetComponents(typeof (ILayoutElement))) {
				(component as ILayoutElement).CalculateLayoutInputHorizontal();
			}
		}

		public void CalculateLayoutInputVertical()
		{
			foreach (var component in CopySource.GetComponents(typeof(ILayoutElement))) {
				(component as ILayoutElement).CalculateLayoutInputVertical();
			}
		}

		protected override void OnEnable()
		{
			SetDirty();
		}

		protected override void OnTransformParentChanged()
		{
			SetDirty();
		}

		protected override void OnDisable()
		{
			SetDirty();
		}

		protected override void OnDidApplyAnimationProperties()
		{
			SetDirty();
		}

		protected override void OnBeforeTransformParentChanged()
		{
			SetDirty();
		}

		protected void SetDirty()
		{

			if (!IsActive())
				return;

			LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);

		}

	}
}

#endif