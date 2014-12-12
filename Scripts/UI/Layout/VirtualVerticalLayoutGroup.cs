#if ENABLE_4_6_FEATURES

//#define PROFILE

using System;
using SRF.Internal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
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
	public class VirtualVerticalLayoutGroup : LayoutGroup, IPointerClickHandler
	{

		[Serializable]
		public class SelectedItemChangedEvent : UnityEvent<object> { }

		[Serializable]
		private class Row
		{

			public int Index;
			public RectTransform Rect;
			public IVirtualView View;
			public StyleRoot Root;
			public object Data;

		}

		public RectTransform ItemPrefab;

		public StyleSheet RowStyleSheet;
		public StyleSheet AltRowStyleSheet;
		public StyleSheet SelectedRowStyleSheet;

		/// <summary>
		/// Rows to show above and below the visible rect to reduce pop-in
		/// </summary>
		public int RowPadding = 2;

		/// <summary>
		/// Spacing to add between rows
		/// </summary>
		public float Spacing;

		[SerializeField]
		private SelectedItemChangedEvent _selectedItemChanged;

		public SelectedItemChangedEvent SelectedItemChanged { get { return _selectedItemChanged; } set { _selectedItemChanged = value; } }

		public object SelectedItem
		{
			get { return _selectedItem; }
			set
			{

				if (_selectedItem == value)
					return;

				var newSelectedIndex = value == null ? -1 : _itemList.IndexOf(value);

				// Ensure that the new selected item is present in the item list
				if (value != null && newSelectedIndex < 0)
					throw new InvalidOperationException("Cannot select item not present in layout");

				// Invalidate old selected item row
				if (_selectedItem != null) {
					InvalidateItem(_selectedIndex);
				}

				_selectedItem = value;
				_selectedIndex = newSelectedIndex;

				// Invalidate the newly selected item
				if (_selectedItem != null) {

					InvalidateItem(_selectedIndex);

				}

				_isDirty = true;

				if (_selectedItemChanged != null)
					_selectedItemChanged.Invoke(_selectedItem);

			}
		}


		public override float minHeight
		{
			get { return _itemList.Count * ItemHeight + padding.top + padding.bottom + Spacing * _itemList.Count; }
		}

		#region Public Data Methods

		public void AddItem(object item)
		{

			_itemList.Add(item);
			_isDirty = true;

		}

		public void RemoveItem(object item)
		{

			if (SelectedItem == item)
				SelectedItem = null;

			var index = _itemList.IndexOf(item);

			InvalidateItem(index);
			_itemList.Remove(item);

			RefreshIndexCache();

			_isDirty = true;

		}

		public void ClearItems()
		{

			for (var i = _visibleRows.Count - 1; i >= 0; i--) {
				InvalidateItem(_visibleRows[i].Index);
			}

			_itemList.Clear();
			_isDirty = true;

		}

		#endregion

		#region Internal Properties

		private ScrollRect ScrollRect
		{
			get
			{

				if (_scrollRect == null)
					_scrollRect = GetComponentInParent<ScrollRect>();

				return _scrollRect;

			}
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


		#endregion

		private readonly SRList<object> _itemList = new SRList<object>();
		private readonly SRList<int> _visibleItemList = new SRList<int>();

		private SRList<Row> _visibleRows = new SRList<Row>();
		private SRList<Row> _rowCache = new SRList<Row>();

		private ScrollRect _scrollRect;
		private bool _isDirty;
		private object _selectedItem;
		private int _selectedIndex;
		private int _visibleItemCount;

		protected override void Awake()
		{

			base.Awake();

			ScrollRect.onValueChanged.AddListener(OnScrollRectValueChanged);

			var view = ItemPrefab.GetComponent(typeof(IVirtualView));

			if (view == null) {
				Debug.LogWarning(
					"[VirtualVerticalLayoutGroup] ItemPrefab does not have a component inheriting from IVirtualView, so no data binding can occur");
			}

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

			// Check for queued update (due to add/remove/clear)
			if (_isDirty) {

				ScrollUpdate();
				_isDirty = false;

			}

		}

		/// <summary>
		/// Invalidate a single row (before removing, or changing selection status)
		/// </summary>
		/// <param name="itemIndex"></param>
		protected void InvalidateItem(int itemIndex)
		{

			if (!_visibleItemList.Contains(itemIndex))
				return;

			_visibleItemList.Remove(itemIndex);

			for (var i = 0; i < _visibleRows.Count; i++) {

				if (_visibleRows[i].Index == itemIndex) {

					RecycleRow(_visibleRows[i]);
					_visibleRows.RemoveAt(i);
					break;

				}

			}

		}

		/// <summary>
		/// After removing or inserting a row, ensure that the cached indexes (used for layout) match up
		/// with the item index in the list
		/// </summary>
		protected void RefreshIndexCache()
		{

			for (var i = 0; i < _visibleRows.Count; i++) {
				_visibleRows[i].Index = _itemList.IndexOf(_visibleRows[i].Data);
			}

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

					_visibleItemList.Remove(row.Index);
					_visibleRows.Remove(row);
					RecycleRow(row);
					isDirty = true;

				}
			}
			
#if PROFILE
			Profiler.EndSample();
#endif

			// If something visible has explicitly been changed, or the visible row count has changed
			if(isDirty || _visibleItemCount != _visibleRows.Count)
				LayoutRebuilder.MarkLayoutForRebuild(rectTransform);

			_visibleItemCount = _visibleRows.Count;

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

			// Position visible rows at 0 x
			for (var i = 0; i < _visibleRows.Count; i++) {

				var item = _visibleRows[i];

				SetChildAlongAxis(item.Rect, 0, padding.left, width);

			}

			// Hide non-active rows to one side. More efficient than enabling/disabling them
			for (var i = 0; i < _rowCache.Count; i++) {

				var item = _rowCache[i];

				SetChildAlongAxis(item.Rect, 0, -width - padding.left, width);

			}

		}

		public override void SetLayoutVertical()
		{

			if (!Application.isPlaying)
				return;

			// Position visible rows by the index of the item they represent
			for (var i = 0; i < _visibleRows.Count; i++) {

				var item = _visibleRows[i];

				SetChildAlongAxis(item.Rect, 1, item.Index * ItemHeight + padding.top + Spacing * item.Index, ItemHeight);

			}

		}

		public void OnPointerClick(PointerEventData eventData)
		{

			var hitObject = eventData.pointerPressRaycast.gameObject;

			if (hitObject == null)
				return;

			var hitPos = hitObject.transform.position;
			var localPos = rectTransform.InverseTransformPoint(hitPos);
			var row = Mathf.FloorToInt(Mathf.Abs(localPos.y)/ItemHeight);

			if (row >= 0 && row < _itemList.Count)
				SelectedItem = _itemList[row];
			else
				SelectedItem = null;

		}

		#region Row Pooling and Provisioning

		private Row GetRow(int forIndex)
		{

			// If there are no rows available in the cache, create one from scratch
			if (_rowCache.Count == 0) {

				var newRow = CreateRow();
				PopulateRow(forIndex, newRow);
				return newRow;

			}

			var data = _itemList[forIndex];

			Row row = null;
			Row altRow = null;

			// Determine if the row we're looking for is an alt row
			var target = forIndex%2;

			// Try and find a row which previously had this data, so we can reuse it
			for (var i = 0; i < _rowCache.Count; i++) {

				row = _rowCache[i];

				// If this row previously represented this data, just use that one.
				if (row.Data == data) {

					_rowCache.RemoveAt(i);
					PopulateRow(forIndex, row);
					break;

				}

				// Cache a row which is was the same alt state as the row we're looking for, in case
				// we don't find an exact match.
				if (row.Index%2 == target)
					altRow = row;

				// Didn't match, reset to null
				row = null;

			}
			
			// If an exact match wasn't found, but a row with the same alt-status was found, use that one.
			if (row == null && altRow != null) {

				_rowCache.Remove(altRow);
				row = altRow;
				PopulateRow(forIndex, row);

			} else if (row == null) {

				// No match found, use the last added item in the cache
				row = _rowCache.PopLast();
				PopulateRow(forIndex, row);

			}

			return row;

		}

		private void RecycleRow(Row row)
		{

			_rowCache.Add(row);

		}

		private void PopulateRow(int index, Row row)
		{

			row.Index = index;

			// If the provided row didn't previously have this data, pass it to the view
			if (row.Data != _itemList[index] && row.View != null) {

				row.Data = _itemList[index];
				row.View.SetDataContext(_itemList[index]);

			}

			// If we're using stylesheets
			if (RowStyleSheet != null || AltRowStyleSheet != null || SelectedRowStyleSheet != null) {

				// If there is a selected row stylesheet, and this is the selected row, use that one
				if (SelectedRowStyleSheet != null && SelectedItem == row.Data) {

					row.Root.StyleSheet = SelectedRowStyleSheet;

				} else {

					// Otherwise just use the stylesheet suitable for the row alt-status
					row.Root.StyleSheet = index%2 == 0 ? RowStyleSheet : AltRowStyleSheet;

				}

			}

		}

		private Row CreateRow()
		{

			var item = new Row();

			var row = SRInstantiate.Instantiate(ItemPrefab);
			item.Rect = row;
			item.View = row.GetComponent(typeof (IVirtualView)) as IVirtualView;

			if (RowStyleSheet != null || AltRowStyleSheet != null || SelectedRowStyleSheet != null) {
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
