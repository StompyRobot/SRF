#if ENABLE_4_6_FEATURES

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SRF.UI
{

	/// <summary>
	/// Copies the preferred size of another layout element (useful for a parent object basing its sizing from a child element).
	/// This does have very quirky behaviour, though.
	/// </summary>
	[RequireComponent(typeof(RectTransform))]
	[ExecuteInEditMode]
	[AddComponentMenu(Internal.ComponentMenuPaths.CopyPreferredSize)]
	public class CopyPreferredSize : UIBehaviour, ILayoutElement
	{

		public RectTransform CopySource;

		public float PaddingWidth;
		public float PaddingHeight;

		public float minWidth
		{
			get
			{
				if (CopySource == null || !IsActive())
					return -1f;
				return LayoutUtility.GetMinWidth(CopySource) + PaddingWidth;
			}
		}

		public float preferredWidth
		{
			get
			{
				if (CopySource == null || !IsActive())
					return -1f;
				return LayoutUtility.GetPreferredWidth(CopySource) + PaddingWidth;
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
				return LayoutUtility.GetFlexibleHeight(CopySource) + PaddingHeight;
			}
		}

		public float preferredHeight
		{
			get
			{
				if (CopySource == null || !IsActive())
					return -1f;
				return LayoutUtility.GetPreferredHeight(CopySource) + PaddingHeight;
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