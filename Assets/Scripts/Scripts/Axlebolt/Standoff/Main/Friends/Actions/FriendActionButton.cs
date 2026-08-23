using System.Linq;
using Axlebolt.Bolt.Friends;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Friends.Actions
{
	[RequireComponent(typeof(Button))]
	public class FriendActionButton : MonoBehaviour
	{
		private BoltFriend _friend;

		private IFriendAction _action;

		private Text _text;

		private IFriendActionListener _listener;

		[SerializeField]
		private FriendActionId[] _ids;

		[SerializeField]
		private bool _isConditionTrue;

		[SerializeField]
		private RelationshipStatus _conditionStatus;

		[SerializeField]
		private GameObject _conditionGameObject;

		protected virtual void Awake()
		{
			this.GetRequireComponent<Button>().onClick.AddListener(delegate
			{
				FriendActionExecutor.ExecuteAction(_action.Id, _friend, _listener);
			});
			_text = base.gameObject.GetComponentInChildren<Text>();
		}

		public void SetListener(IFriendActionListener listener)
		{
			_listener = listener;
		}

		public virtual void SetFriend(BoltFriend friend)
		{
			_friend = friend;
			IFriendAction action = _ids.Select(FriendActionExecutor.GetAction).FirstOrDefault((IFriendAction a) => a.IsSupported(_friend));
			SetAction(action);
			if (!(_conditionGameObject == null))
			{
				bool flag = friend.Relationship == _conditionStatus;
				if (!_isConditionTrue)
				{
					flag = !flag;
				}
				_conditionGameObject.SetActive(flag);
			}
		}

		protected virtual void SetAction(IFriendAction action)
		{
			_action = action;
			if (_action == null)
			{
				base.gameObject.SetActive(false);
				return;
			}
			base.gameObject.SetActive(true);
			_text.text = _action.LocalizedText;
		}
	}
}
