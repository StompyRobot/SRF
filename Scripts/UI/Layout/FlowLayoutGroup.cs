#if ENABLE_4_6_FEATURES
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SRF.UI.Layout
{

	/// <summary>
	/// Layout Group controller that arranges children in rows, fitting as many on a line until total width exceeds parent bounds
	/// </summary>
	[AddComponentMenu(Internal.ComponentMenuPaths.FlowLayoutGroup)]
	public class FlowLayoutGroup : LayoutGroup
	{

		public float Spacing = 0f;

		public override void CalculateLayoutInputHorizontal()
		{

			base.CalculateLayoutInputHorizontal();

			var minWidth = GetGreatestMinimumChildWidth() + padding.left + padding.right;

			SetLayoutInputForAxis(minWidth, -1, -1, 0);

		}

		public override void SetLayoutHorizontal()
		{
			SetLayout(rectTransform.rect.width, 0, false);
		}

		public override void SetLayoutVertical()
		{
			SetLayout(rectTransform.rect.width, 1, false);
		}

		public override void CalculateLayoutInputVertical()
		{
			SetLayout(rectTransform.rect.width, 1, true);
		}

		protected bool IsCenterAlign
		{
			get
			{
				return childAlignment == TextAnchor.LowerCenter || childAlignment == TextAnchor.MiddleCenter ||
				       childAlignment == TextAnchor.UpperCenter;
			}
		}

		protected bool IsRightAlign
		{
			get
			{
				return childAlignment == TextAnchor.LowerRight || childAlignment == TextAnchor.MiddleRight ||
				       childAlignment == TextAnchor.UpperRight;
			}
		}

		protected bool IsMiddleAlign
		{
			get
			{
				return childAlignment == TextAnchor.MiddleLeft || childAlignment == TextAnchor.MiddleRight ||
				       childAlignment == TextAnchor.MiddleCenter;
			}
		}

		protected bool IsLowerAlign
		{
			get
			{
				return childAlignment == TextAnchor.LowerLeft || childAlignment == TextAnchor.LowerRight ||
					   childAlignment == TextAnchor.LowerCenter;
			}
		}

		/// <summary>
		/// Holds the rects that will make up the current row being processed
		/// </summary>
		private readonly SRList<RectTransform> _rowList = new SRList<RectTransform>(); 

		/// <summary>
		/// Main layout method
		/// </summary>
		/// <param name="width">Width to calculate the layout with</param>
		/// <param name="axis">0 for horizontal axis, 1 for vertical</param>
		/// <param name="layoutInput">If true, sets the layout input for the axis. If false, sets child position for axis</param>
		public float SetLayout(float width, int axis, bool layoutInput)
		{

			// Width that is available after padding is subtracted
			var workingWidth = rectTransform.rect.width - padding.left - padding.right;

			// Accumulates the total height of the rows, including spacing and padding.
			var totalHeight = (float)padding.top;

			var currentRowWidth = 0f;
			var currentRow = 0;
			var currentRowHeight = 0f;

			for (var i = 0; i < rectChildren.Count; i++) {

				var child = rectChildren[i];

				var childWidth = LayoutUtility.GetPreferredSize(child, 0);
				var childHeight = LayoutUtility.GetPreferredSize(child, 1);

				// Max child width is layout group with - padding
				childWidth = Mathf.Min(childWidth, workingWidth);

				// If adding this element would exceed the bounds of the row,
				// go to a new line after processing the current row
				if (currentRowWidth + childWidth > workingWidth) {

					currentRowWidth -= Spacing;

					// Process current row elements positioning
					if (!layoutInput) {

						LayoutRow(_rowList, currentRowWidth, currentRowHeight, workingWidth, padding.left, totalHeight, axis);

					}
				
					// Clear existing row
					_rowList.Clear();

					// Increment row count
					currentRow++;

					// Add the current row height to total height accumulator, and reset to 0 for the next row
					totalHeight += currentRowHeight;
					totalHeight += Spacing;

					currentRowHeight = 0;
					currentRowWidth = 0;

				}

				currentRowWidth += childWidth;
				_rowList.Add(child);

				// We need the largest element height to determine the starting position of the next line
				if (childHeight > currentRowHeight) {
					currentRowHeight = childHeight;
				}

				currentRowWidth += Spacing;

			}

			if (!layoutInput) {

				// Layout the final row
				LayoutRow(_rowList, currentRowWidth, currentRowHeight, workingWidth, padding.left, totalHeight, axis);

			}

			_rowList.Clear();

			// Add the last rows height to the height accumulator
			totalHeight += currentRowHeight;

			if (layoutInput) {

				if(axis == 1)
					SetLayoutInputForAxis(totalHeight, totalHeight, -1, axis);

			}

			return totalHeight;

		}

		protected void LayoutRow(IList<RectTransform> contents, float rowWidth, float rowHeight, float maxWidth, float xOffset, float yOffset, int axis)
		{

			var xPos = xOffset;

			if (IsCenterAlign)
				xPos += (maxWidth - rowWidth) * 0.5f;
			else if (IsRightAlign)
				xPos += (maxWidth - rowWidth);

			for (var j = 0; j < _rowList.Count; j++) {

				var rowChild = _rowList[j];

				var rowChildWidth = LayoutUtility.GetPreferredSize(rowChild, 0);
				var rowChildHeight = LayoutUtility.GetPreferredSize(rowChild, 1);

				rowChildWidth = Mathf.Min(rowChildWidth, maxWidth);

				var yPos = yOffset;

				if (IsMiddleAlign)
					yPos += (rowHeight - rowChildHeight) * 0.5f;
				else if (IsLowerAlign)
					yPos += (rowHeight - rowChildHeight);

				if (axis == 0)
					SetChildAlongAxis(rowChild, 0, xPos, rowChildWidth);
				else
					SetChildAlongAxis(rowChild, 1, yPos, rowChildHeight);

				xPos += rowChildWidth + Spacing;

			}

		}

		public float GetGreatestMinimumChildWidth()
		{

			var max = 0f;

			for (var i = 0; i < rectChildren.Count; i++) {

				var w = LayoutUtility.GetMinWidth(rectChildren[i]);

				max = Mathf.Max(w, max);

			}

			return max;

		}

	}


}

#endif
