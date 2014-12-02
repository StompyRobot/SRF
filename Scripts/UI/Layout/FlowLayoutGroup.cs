#if ENABLE_4_6_FEATURES
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

			// Just set the horizontal size to the current width of the layout group, since width dictates layout
			var width = rectTransform.rect.width;
			SetLayoutInputForAxis(width, width, -1, 0);

		}

		public override void CalculateLayoutInputVertical()
		{
			SetLayout(1, true);
		}

		/// <summary>
		/// Main layout method
		/// </summary>
		/// <param name="axis">0 for horizontal axis, 1 for vertical</param>
		/// <param name="layoutInput">If true, sets the layout input for the axis. If false, sets child position for axis</param>
		public void SetLayout(int axis, bool layoutInput)
		{

			var layoutWidth = rectTransform.rect.width;

			// Width that is available after padding is subtracted
			var workingWidth = rectTransform.rect.width - padding.left - padding.right;

			// Accumulates the total height of the rows, not including spacing.
			var totalHeight = 0f;

			var currentRowWidth = 0f;
			var currentRow = 0;
			var currentRowHeight = 0f;

			for (var i = 0; i < rectChildren.Count; i++) {

				var child = rectChildren[i];

				var childWidth = LayoutUtility.GetPreferredSize(child, 0);
				var childHeight = LayoutUtility.GetPreferredSize(child, 1);

				// Max child width is layout group with - padding
				childWidth = Mathf.Min(childWidth, layoutWidth - padding.left - padding.right);
				
				var xPos = currentRowWidth;

				currentRowWidth += childWidth;
				// If adding this element would exceed the bounds of the row,
				// go to a new line
				if (currentRowWidth > workingWidth) {

					// Increment row count
					currentRow++;

					// Reset x position accumulator
					xPos = 0f;

					// Set current row width to the width of the child that triggered
					// the new line
					currentRowWidth = childWidth;

					// Add the current row height to total height accumulator, and reset to 0 for the next row
					totalHeight += currentRowHeight;
					currentRowHeight = 0;

				}

				// We need the largest element height to determine the starting position of the next line
				if (childHeight > currentRowHeight) {
					currentRowHeight = childHeight;
				}

				if (!layoutInput) {

					if (axis == 0)
						SetChildAlongAxis(child, 0, padding.left + xPos, childWidth);
					else
						SetChildAlongAxis(child, 1, (currentRow*Spacing) + totalHeight + padding.top, childHeight);

				}
				currentRowWidth += Spacing;

			}

			// Add the last rows height to the height accumulator
			totalHeight += currentRowHeight;

			if (layoutInput) {

				totalHeight = totalHeight + (Spacing * currentRow) + padding.top + padding.bottom;

				if(axis == 1)
					SetLayoutInputForAxis(totalHeight, totalHeight, -1, axis);

			}

		}

		public override void SetLayoutHorizontal()
		{
			SetLayout(0, false);
		}

		public override void SetLayoutVertical()
		{
			SetLayout(1, false);
		}

	}

}

#endif
