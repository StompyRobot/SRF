#if ENABLE_4_6_FEATURES

//#define PROFILE

using System;
using SRF.Internal;
using UnityEngine;
using UnityEngine.UI;

namespace SRF.UI.Layout
{

	public interface IVirtualView
	{

		void SetDataContext(object data);

	}

	/// <summary>
	/// 
	/// </summary>
	[AddComponentMenu(ComponentMenuPaths.VirtualVerticalLayoutGroup)]
	public class VirtualVerticalLayoutGroup : LayoutGroup
	{

		[Serializable]
		private class Row
		{

			public int Index;
			public RectTransform Rect;
			public IVirtualView View;
			public StyleRoot Root;

		}

		public RectTransform ItemPrefab;

		public StyleSheet RowStyleSheet;
		public StyleSheet AltRowStyleSheet;

		//public IList<object> Items { get { return _itemList.AsReadOnly(); } }

		/// <summary>
		/// Rows to show above and below the visible rect to reduce pop-in
		/// </summary>
		public int RowPadding = 2;

		public float Spacing;

		private readonly SRList<object> _itemList = new SRList<object>();
		private readonly SRList<int> _visibleItemList = new SRList<int>(); 

		private SRList<Row> _visibleRows = new SRList<Row>(); 
		private SRList<Row> _rowCache = new SRList<Row>();

		private ScrollRect _scrollRect;
		private bool _isDirty;

		private ScrollRect ScrollRect
		{
			get
			{

				if (_scrollRect == null)
					_scrollRect = GetComponentInParent<ScrollRect>();

				return _scrollRect;

			}
		}

		public override float minHeight
		{
			get { return _itemList.Count * ItemHeight + padding.top + padding.bottom + Spacing * _itemList.Count; }
		}

		private bool AlignBottom
		{
			get
			{
				return childAlignment == TextAnchor.LowerRight || childAlignment == TextAnchor.LowerCenter || 
				       childAlignment == TextAnchor.LowerLeft;
			}
		}

		private bool AlignTop
		{
			get
			{
				return childAlignment == TextAnchor.UpperLeft || childAlignment == TextAnchor.UpperCenter ||
				       childAlignment == TextAnchor.UpperRight;
			}
		}

		private float _itemHeight = -1;

		private float ItemHeight
		{
			get
			{

				if (_itemHeight <= 0) {

					_itemHeight = LayoutUtility.GetPreferredSize(ItemPrefab, 1);

					if (_itemHeight.ApproxZero()) {
						Debug.LogWarning("[VirtualVerticalLayoutGroup] ItemPrefab must have a preferred size greater than 0");
						_itemHeight = 10;
					}

				}

				return _itemHeight;

			}
		}

		protected override void Awake()
		{

			base.Awake();

			ScrollRect.onValueChanged.AddListener(OnScrollRectValueChanged);

			var view = ItemPrefab.GetComponent(typeof (IVirtualView));

			if (view  == null) {
				Debug.LogWarning(
					"[VirtualVerticalLayoutGroup] ItemPrefab does not have a component inheriting from IVirtualView, so no data binding can occur");
			}

		}

		public void AddItem(object item)
		{
			_itemList.Add(item);
			_isDirty = true;
		}

		public void RemoveItem(object item)
		{
			_itemList.Remove(item);
			_isDirty = true;
		}

		private void OnScrollRectValueChanged(Vector2 d)
		{
			ScrollUpdate();
		}

		protected override void Start()
		{

			base.Start();
			ScrollUpdate();

		}

		protected override void OnEnable()
		{

			base.OnEnable();
			ScrollUpdate();

		}

		protected void Update()
		{

			if (!AlignBottom && !AlignTop) {

				Debug.LogWarning("[VirtualVerticalLayoutGroup] Only Lower or Upper alignment is supported.", this);
				childAlignment = TextAnchor.UpperLeft;

			}

			if (_isDirty) {
				ScrollUpdate();
				_isDirty = false;
			}

			//Profiler.BeginSample("ScrollUpdate");
			//ScrollUpdate();
			//Profiler.EndSample();

		}

		protected void ScrollUpdate()
		{

			if (!Application.isPlaying)
				return;

			var pos = rectTransform.anchoredPosition;
			var startY = pos.y;

			var isDirty = false;

#if PROFILE
			Profiler.BeginSample("Item Visible Check");
#endif

			// Find items which should be visible but aren't
			for (var i = 0; i < _itemList.Count; i++) {

				if (IsVisible(i, startY) && !_visibleItemList.Contains(i)) {

					//Debug.Log("[VirtualVerticalLayoutGroup] Showing Item {0}".Fmt(i));

					var row = GetRow(i);
					_visibleRows.Add(row);
					_visibleItemList.Add(i);
					isDirty = true;

				}

			}

#if PROFILE
			Profiler.EndSample();

			Profiler.BeginSample("Visible Rows Cull");
#endif

			// Find items which are visible but shouldn't be
			for (var i = _visibleRows.Count - 1; i >= 0; i--) {

				var row = _visibleRows[i];

				if (!IsVisible(row.Index, startY)) {

					//Debug.Log("[VirtualVerticalLayoutGroup] Hiding Item {0}".Fmt(row.Index));

					_visibleItemList.Remove(row.Index);
					_visibleRows.Remove(row);
					RecycleRow(row);
					isDirty = true;

				}
			}
			
#if PROFILE
			Profiler.EndSample();
#endif

			if(isDirty)
				LayoutRebuilder.MarkLayoutForRebuild(rectTransform);

		}

		private bool IsVisible(int index, float yPos)
		{


			var rowStartY = ItemHeight*index + Spacing*index;
			var viewHeight = ((RectTransform)ScrollRect.transform).rect.height;

			var distanceFromViewTop = rowStartY - yPos;

			var pad = ItemHeight*RowPadding;

			if (distanceFromViewTop > -pad && distanceFromViewTop < viewHeight + pad) {
				return true;
			}

			return false;

		}

		public override void CalculateLayoutInputVertical()
		{

			SetLayoutInputForAxis(minHeight, minHeight, -1, 1);

		}

		public override void SetLayoutHorizontal()
		{

			var width = rectTransform.rect.width - padding.left - padding.right;

			for (var i = 0; i < _visibleRows.Count; i++) {

				var item = _visibleRows[i];

				SetChildAlongAxis(item.Rect, 0, padding.left, width);

			}

			// Hide non-active rows to one side
			for (var i = 0; i < _rowCache.Count; i++) {

				var item = _rowCache[i];

				SetChildAlongAxis(item.Rect, 0, -width - padding.left, width);

			}

		}

		public override void SetLayoutVertical()
		{

			if (!Application.isPlaying)
				return;

			for (var i = 0; i < _visibleRows.Count; i++) {

				var item = _visibleRows[i];

				SetChildAlongAxis(item.Rect, 1, item.Index * ItemHeight + padding.top + Spacing * item.Index, ItemHeight);

			}

		}

		#region Row Pooling

		private Row GetRow(int forIndex)
		{

			if (_rowCache.Count == 0) {

				var newRow = CreateRow();
				PopulateRow(forIndex, newRow);
				return newRow;

			}

			Row row = null;
			Row altRow = null;

			var target = forIndex%2;

			// Try and find a row which previously had this index, so we can reuse it
			for (var i = 0; i < _rowCache.Count; i++) {

				row = _rowCache[i];

				if (row.Index == forIndex) {

					_rowCache.RemoveAt(i);
					break;

				}

				if (row.Index%2 == target)
					altRow = row;

				row = null;

			}

			if (row == null && altRow != null) {

				_rowCache.Remove(altRow);
				row = altRow;
				PopulateRow(forIndex, row);

			} else if (row == null) {

				row = _rowCache.PopLast();
				PopulateRow(forIndex, row);

			}

			//row.Rect.gameObject.SetActive(true);

			return row;

		}

		private void RecycleRow(Row row)
		{

			_rowCache.Add(row);
			//row.Rect.gameObject.SetActive(false);

		}

		private void PopulateRow(int index, Row row)
		{

			row.Index = index;

			if (row.View != null)
				row.View.SetDataContext(_itemList[index]);

			if (RowStyleSheet != null || AltRowStyleSheet != null) {

				row.Root.StyleSheet = index%2 == 0 ? RowStyleSheet : AltRowStyleSheet;

			}

		}

		private Row CreateRow()
		{

			var item = new Row();

			var row = SRInstantiate.Instantiate(ItemPrefab);
			item.Rect = row;
			item.View = row.GetComponent(typeof (IVirtualView)) as IVirtualView;

			if (RowStyleSheet != null || AltRowStyleSheet != null) {
				item.Root = row.gameObject.GetComponentOrAdd<StyleRoot>();
				item.Root.StyleSheet = RowStyleSheet;
			}

			row.SetParent(rectTransform, false);



			return item;

		}

		#endregion

	}


}

#endif
