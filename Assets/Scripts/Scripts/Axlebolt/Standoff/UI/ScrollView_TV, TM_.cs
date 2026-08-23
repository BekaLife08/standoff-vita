using I2.Loc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Axlebolt.Standoff.UI
{
	[RequireComponent(typeof(ScrollRect))]
	public abstract class ScrollView<TV, TM> : View where TV : View
	{
		public class RowData
		{
			public int Index;

			public readonly TV View;

			public TM Model;

			public bool Selected;

			public RowData(TV view)
			{
				Index = -1;
				View = view;
			}
		}

		private ScrollRect _scrollRect;

		[SerializeField]
		private int _poolSize;

		[SerializeField]
		private TV _rowTemplate;

		[SerializeField]
		private TextView _emptyView;

		private float _totalViewHeight;

		private readonly List<RowData> _rows = new List<RowData>();

		private readonly List<TM> _content = new List<TM>();

		private VerticalLayoutGroup _layout;

		private RectTransform _layoutTransform;

		private RectTransform _topSpring;

		private RectTransform _bottomSpring;

		private int _topOffset;

		private int _index;

		private float _totalHeight;

		private float _activeCount;

		private bool _initialized;

		public Action<TM> ItemSelectedHandler
		{
			get;
			set;
		}

		public Action<TM> ItemUnselectedHandler
		{
			get;
			set;
		}

		public string EmptyText
		{
			get;
			set;
		}

		public RowData SelectedRow
		{
			[CompilerGenerated]
			get
			{
				return _rows.FirstOrDefault((RowData row) => row.Index == SelectedIndex && SelectedIndex >= 0);
			}
		}

		public int SelectedIndex
		{
			get;
			private set;
		} = -1;


		protected TextView EmptyView
		{
			[CompilerGenerated]
			get
			{
				return _emptyView;
			}
		}

		protected virtual void Awake()
		{
			Init();
		}

		protected virtual void Init()
		{
			if (!_initialized)
			{
				_scrollRect = this.GetRequireComponent<ScrollRect>();
				_layout = _scrollRect.content.GetRequireComponent<VerticalLayoutGroup>();
				_layoutTransform = _layout.GetRequireComponent<RectTransform>();
				_topOffset = _layoutTransform.childCount;
				for (int i = 0; i < _layoutTransform.childCount; i++)
				{
					_layoutTransform.GetChild(i).gameObject.SetActive(value: false);
				}
				_layout.padding.top = 0;
				_layout.padding.bottom = 0;
				_rowTemplate.gameObject.SetActive(value: false);
				float height = _rowTemplate.GetRequireComponent<RectTransform>().rect.height;
				_totalViewHeight = height + _layout.spacing;
				for (int j = 0; j < _poolSize; j++)
				{
					TV val = UnityEngine.Object.Instantiate(_rowTemplate, _rowTemplate.transform.parent, worldPositionStays: false);
					val.gameObject.SetActive(value: false);
					val.gameObject.name += j.ToString();
					RowData rowData = new RowData(val);
					_rows.Add(rowData);
					BindSelectHandler(val, rowData);
				}
				EmptyText = ScriptLocalization.Common.ListIsEmpty;
				UpdateEmptyView();
				_scrollRect.onValueChanged.AddListener(OnValueChanged);
				_initialized = true;
			}
		}

		private void BindSelectHandler(TV instance, RowData rowData)
		{
			ScrollViewRow scrollViewRow = instance.gameObject.AddComponent<ScrollViewRow>();
			scrollViewRow.ClickHandler = delegate(PointerEventData eventData)
			{
				SetSelectedRow(rowData, eventData);
			};
		}

		private void SetSelectedRow(RowData rowData, PointerEventData eventData)
		{
			if (SelectedRow != null)
			{
				OnRowUnselected(SelectedRow);
				ItemUnselectedHandler?.Invoke(SelectedRow.Model);
			}
			rowData.Selected = true;
			SelectedIndex = rowData.Index;
			OnRowSelected(rowData, eventData);
			ItemSelectedHandler?.Invoke(rowData.Model);
		}

		protected abstract void UpdateRowView(RowData rowData);

		protected abstract void OnRowSelected(RowData rowData, PointerEventData eventData);

		protected abstract void OnRowUnselected(RowData rowData);

		public void SetSelectedRowIndex(int index)
		{
			RowData rowData = _rows.FirstOrDefault((RowData row) => row.Index == index);
			if (rowData != null)
			{
				SetSelectedRow(rowData, null);
			}
		}

		public RowData[] GetViews()
		{
			return _rows.ToArray();
		}

		private void CreateTopSpring()
		{
			if (!(_topSpring != null))
			{
				GameObject gameObject = new GameObject("TopSpring");
				gameObject.AddComponent<RectTransform>();
				gameObject.transform.SetParent(_layout.transform, worldPositionStays: false);
				gameObject.transform.SetSiblingIndex(_topOffset);
				RectTransform requireComponent = gameObject.GetRequireComponent<RectTransform>();
				requireComponent.sizeDelta = EmptySize();
				_topSpring = requireComponent;
			}
		}

		private void CreateBottomSpring()
		{
			if (!(_bottomSpring != null))
			{
				GameObject gameObject = new GameObject("BottomSpring");
				gameObject.AddComponent<RectTransform>();
				gameObject.transform.SetParent(_layout.transform, worldPositionStays: false);
				gameObject.transform.SetSiblingIndex(_layout.transform.childCount - 1);
				RectTransform requireComponent = gameObject.GetRequireComponent<RectTransform>();
				requireComponent.sizeDelta = EmptySize();
				_bottomSpring = requireComponent;
			}
		}

		private static Vector2 EmptySize()
		{
			return new Vector2(1f, 0f);
		}

		public void ReplaceContent(TM[] array)
		{
			_content.Clear();
			_content.AddRange(array);
			RecalculateTotalHeight();
			if (_index >= array.Length)
			{
				ResetScroll();
			}
			UpdateElements(reset: true);
		}

		public void SetContent(TM[] array)
		{
			_content.Clear();
			_content.AddRange(array);
			RecalculateTotalHeight();
			ResetScroll();
			UpdateElements(reset: true);
		}

		public void AddContent(TM[] array)
		{
			_content.AddRange(array);
			RecalculateTotalHeight();
			UpdateElements();
		}

		public void ReplaceContent(TM[] array, int index, int count)
		{
			if (index + count > _content.Count)
			{
				count = _content.Count - index;
			}
			_content.RemoveRange(index, count);
			_content.AddRange(array);
			RecalculateTotalHeight();
			UpdateElements(reset: true);
		}

		public void UpdateView(TM model)
		{
			foreach (RowData row in _rows)
			{
				if (row.Index >= 0 && row.Model.Equals(model))
				{
					UpdateRowView(row);
				}
			}
		}

		protected void UpdateEmptyView()
		{
			if (_content.Count == 0)
			{
				_emptyView.Text = EmptyText;
				_emptyView.Show();
			}
			else
			{
				_emptyView.Hide();
			}
		}

		private void ResetScroll()
		{
			_scrollRect.verticalNormalizedPosition = 1f;
			_index = 0;
			LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutTransform);
		}

		private void RecalculateTotalHeight()
		{
			_totalHeight = _totalViewHeight * (float)_content.Count;
		}

		protected float CalculatRowHeightValue()
		{
			return _totalViewHeight / _totalHeight;
		}

		protected virtual void OnValueChanged(Vector2 p)
		{
			Vector2 offsetMax = _layoutTransform.offsetMax;
			float y = offsetMax.y;
			int index = _index;
			_index = (int)((y - 2f * _totalViewHeight) / _totalViewHeight);
			if (_index < 0)
			{
				_index = 0;
			}
			int num = _index - index;
			if (num != 0)
			{
				OnValueChanged(num);
			}
		}

		protected virtual void OnValueChanged(int diff)
		{
			if (diff > 0 && diff < _poolSize)
			{
				List<RowData> range = _rows.GetRange(0, diff);
				_rows.RemoveRange(0, diff);
				_rows.AddRange(range);
				foreach (RowData item in range)
				{
					TV view = item.View;
					view.transform.SetSiblingIndex(_bottomSpring.GetSiblingIndex() - 1);
				}
			}
			else if (-diff > 0 && -diff < _poolSize)
			{
				diff *= -1;
				List<RowData> range2 = _rows.GetRange(_poolSize - diff, diff);
				_rows.RemoveRange(_poolSize - diff, diff);
				_rows.InsertRange(0, range2);
				range2.Reverse();
				foreach (RowData item2 in range2)
				{
					TV view2 = item2.View;
					view2.transform.SetSiblingIndex(_topSpring.GetSiblingIndex() + 1);
				}
			}
			UpdateElements();
		}

		private void ResetViews()
		{
			if (SelectedRow != null)
			{
				OnRowUnselected(SelectedRow);
			}
			SelectedIndex = -1;
			foreach (RowData row in _rows)
			{
				row.Index = -1;
				row.Model = default(TM);
				row.Selected = false;
			}
		}

		private void UpdateElements(bool reset = false)
		{
			if (reset)
			{
				ResetViews();
			}
			CreateTopSpring();
			CreateBottomSpring();
			UpdateViews();
			UpdateSprings();
			UpdateEmptyView();
		}

		public void UpdateViews()
		{
			_activeCount = 0f;
			for (int i = 0; i < _rows.Count; i++)
			{
				int num = _index + i;
				if (num < _content.Count)
				{
					TV view = _rows[i].View;
					view.gameObject.SetActive(value: true);
					if (_rows[i].Index != num)
					{
						_rows[i].Index = num;
						_rows[i].Model = _content[num];
						_rows[i].Selected = (num == SelectedIndex);
						UpdateRowView(_rows[i]);
					}
					_activeCount += 1f;
				}
				else
				{
					TV view2 = _rows[i].View;
					view2.gameObject.SetActive(value: false);
				}
			}
		}

		private void UpdateSprings()
		{
			float num = (float)_index * _totalViewHeight;
			_topSpring.sizeDelta = new Vector2(1f, num);
			_bottomSpring.sizeDelta = ((!(_activeCount < (float)_poolSize)) ? new Vector2(1f, _totalHeight - num - _activeCount * _totalViewHeight) : EmptySize());
		}

		protected float GetBottomSpringHeight()
		{
			Vector2 sizeDelta = _bottomSpring.sizeDelta;
			return sizeDelta.y;
		}
	}
}
