using Axlebolt.Standoff.UI;
using UnityEngine.EventSystems;

namespace Axlebolt.Standoff.Main.Messages
{
	public class ChatScrollView : ScrollView<ChatRowView, ChatModel>
	{
		protected override void UpdateRowView(RowData rowData)
		{
			rowData.View.SetChat(rowData.Model);
			rowData.View.Selected = rowData.Selected;
		}

		protected override void OnRowSelected(RowData rowData, PointerEventData eventData)
		{
			rowData.View.Selected = true;
		}

		protected override void OnRowUnselected(RowData rowData)
		{
			rowData.View.Selected = false;
		}
	}
}
