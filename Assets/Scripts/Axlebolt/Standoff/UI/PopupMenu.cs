using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Axlebolt.Standoff.UI
{
	[RequireComponent(typeof(RectTransform))]
	public class PopupMenu : View
	{
		private class BlockerElement : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
		{
			public Action<PointerEventData> ClickHandler
			{
				private get;
				set;
			}

			public void OnPointerClick(PointerEventData eventData)
			{
				ClickHandler?.Invoke(eventData);
			}
		}

		private RectTransform _rectTransform;

		[SerializeField]
		[NotNull]
		private RectTransform _leftArrow;

		[NotNull]
		[SerializeField]
		private RectTransform _rightArrow;

		[SerializeField]
		[NotNull]
		private PopupItemView _itemView;

		private RectTransform _canvasTransform;

		private Transform _parent;

		private bool _hasChanges;

		private readonly List<PopupItemView> _views = new List<PopupItemView>();

		private PopupItemView _lastItem;

		private GameObject _blocker;

		public Action ShowHandler
		{
			get;
			set;
		}

		public Action<PopupItem> ActionHandler
		{
			get;
			set;
		}

		public Action HideHandler
		{
			get;
			set;
		}

		public int ItemsCount
		{
			[CompilerGenerated]
			get
			{
				return base.transform.childCount - 2;
			}
		}

		public List<PopupItem> Items
		{
			get;
		} = new List<PopupItem>();


		protected virtual void Awake()
		{
			_parent = base.transform.parent;
			_rectTransform = this.GetRequireComponent<RectTransform>();
			_itemView.gameObject.SetActive(value: false);
			Canvas canvas = ViewUtility.GetCanvas(base.transform);
			_canvasTransform = canvas.GetRequireComponent<RectTransform>();
		}

		public void AddItem(PopupItem item)
		{
			if ((object)_lastItem != null)
			{
				_lastItem.SplitLine.SetActive(value: true);
			}
			PopupItemView popupItemView = UnityEngine.Object.Instantiate(_itemView, _itemView.transform.parent);
			popupItemView.ClickHandler = delegate
			{
				ActionHandler?.Invoke(item);
				Hide();
			};
			popupItemView.gameObject.SetActive(value: true);
			popupItemView.Text.text = item.Text;
			popupItemView.SplitLine.SetActive(value: false);
			Items.Add(item);
			_views.Add(popupItemView);
			_lastItem = popupItemView;
			_hasChanges = true;
		}

		public void SetItemVisible(PopupItem item, bool visible)
		{
			_views[Items.IndexOf(item)].IsVisible = visible;
		}

		protected virtual GameObject CreateBlocker(RectTransform canvasTransform)
		{
			GameObject gameObject = new GameObject("Blocker");
			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			rectTransform.SetParent(canvasTransform, worldPositionStays: false);
			rectTransform.anchorMin = Vector3.zero;
			rectTransform.anchorMax = Vector3.one;
			rectTransform.sizeDelta = Vector2.zero;
			gameObject.AddComponent<Image>().color = Color.clear;
			gameObject.AddComponent<BlockerElement>().ClickHandler = delegate(PointerEventData data)
			{
				Hide();
				Raycast(data);
			};
			return gameObject;
		}

		private void Raycast(PointerEventData data)
		{
			List<RaycastResult> list = new List<RaycastResult>();
			ViewUtility.GetCanvas(base.transform).GetRequireComponent<GraphicRaycaster>().Raycast(data, list);
			foreach (RaycastResult item in list)
			{
				if (ExecuteEvents.Execute(item.gameObject, data, ExecuteEvents.pointerClickHandler) || (item.gameObject.transform.parent != null && ExecuteEvents.Execute(item.gameObject.transform.parent.gameObject, data, ExecuteEvents.pointerClickHandler)))
				{
					break;
				}
			}
		}

		public virtual void Show(Vector2 position)
		{
			ShowHandler?.Invoke();
			base.Show();
			if (_blocker == null)
			{
				_blocker = CreateBlocker(_canvasTransform);
			}
			base.transform.SetParent(_canvasTransform, worldPositionStays: false);
			if (_hasChanges)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
			}
			_hasChanges = false;
			float num = _canvasTransform.rect.width / (float)Screen.width;
			float num2 = _canvasTransform.rect.height / (float)Screen.height;
			Vector2 vector = new Vector2(position.x * num, position.y * num2);
			Vector2 pivot = new Vector2(0f, 0.5f);
			float height = _rectTransform.rect.height;
			float width = _leftArrow.rect.width;
			Vector2 pivot2 = _leftArrow.pivot;
			float num3 = width * pivot2.x / num;
			float width2 = _rectTransform.rect.width;
			float x = vector.x;
			float y = vector.y;
			float num4 = position.x + num3;
			float y2 = position.y;
			Rect rect = _canvasTransform.rect;
			RectTransform rectTransform = _leftArrow;
			_leftArrow.gameObject.SetActive(value: false);
			_rightArrow.gameObject.SetActive(value: false);
			if (y + pivot.y * height > rect.height)
			{
				pivot = new Vector2(pivot.x, 1f - (rect.height - y) / height);
			}
			else if (y - pivot.y * height < 0f)
			{
				pivot = new Vector2(pivot.x, y / height);
			}
			if (x + num3 + width2 > rect.width)
			{
				num4 -= num3 * 2f;
				pivot = new Vector2(1f, pivot.y);
				rectTransform = _rightArrow;
			}
			rectTransform.gameObject.SetActive(value: true);
			rectTransform.anchorMin = new Vector2(0f, pivot.y);
			rectTransform.anchorMax = new Vector2(0f, pivot.y);
			_rectTransform.pivot = pivot;
			_rectTransform.position = new Vector3(num4, y2);
		}

		public override void Hide()
		{
			base.Hide();
			if (!(_blocker == null))
			{
				base.transform.SetParent(_parent, worldPositionStays: false);
				UnityEngine.Object.Destroy(_blocker);
				_blocker = null;
				HideHandler?.Invoke();
			}
		}
	}
}
