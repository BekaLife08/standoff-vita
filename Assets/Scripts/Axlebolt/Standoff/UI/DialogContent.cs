namespace Axlebolt.Standoff.UI
{
	public class DialogContent : View
	{
		private Dialog _dialog;

		internal void SetDialog(Dialog dialog)
		{
			_dialog = dialog;
		}

		protected void HideDialog()
		{
			_dialog.Hide();
		}
	}
}
