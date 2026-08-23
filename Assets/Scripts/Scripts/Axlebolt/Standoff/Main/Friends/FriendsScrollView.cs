using Axlebolt.Bolt.Friends;
using Axlebolt.Standoff.Main.Friends.Actions;
using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Axlebolt.Standoff.Main.Friends
{
	public class FriendsScrollView : ScrollView<FriendRowView, BoltFriend>
	{
		[SerializeField]
		private FriendActionPopupMenu _popupMenu;

		protected override void Awake()
		{
			base.Awake();
			_popupMenu.HideHandler = delegate
			{
				base.SelectedRow.View.Selected = false;
			};
		}

		public void SetActionListener(IFriendActionListener listener)
		{
			_popupMenu.SetListener(listener);
			RowData[] views = GetViews();
			foreach (RowData rowData in views)
			{
				rowData.View.SetListener(listener);
			}
		}

		protected override void UpdateRowView(RowData rowData)
		{
			rowData.View.SetFriend(rowData.Model);
		}

		protected override void OnRowSelected(RowData rowData, PointerEventData eventData)
		{
			rowData.View.Selected = true;
			_popupMenu.Show(rowData.Model, eventData.position);
		}

		protected override void OnRowUnselected(RowData rowData)
		{
			rowData.View.Selected = false;
			_popupMenu.Hide();
		}
	}
}
