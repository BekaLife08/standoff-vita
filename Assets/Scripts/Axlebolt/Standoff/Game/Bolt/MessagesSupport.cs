using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game.Event;
using Axlebolt.Standoff.Game.UI.Pause;
using Axlebolt.Standoff.Main.Messages;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.Bolt
{
	public class MessagesSupport : MonoBehaviour
	{
		private void Awake()
		{
			GameController.Instance.GameInitEvent.AddListener(OnGameInit);
		}

		public void OnGameInit(GameInitEventArgs initEventArgs)
		{
			StartCoroutine(CreateUi());
		}

		private IEnumerator CreateUi()
		{
			Canvas canvas = UnityEngine.Object.Instantiate(ResourcesUtility.Load<GameObject>("UI/Canvas")).GetRequireComponent<Canvas>();
			canvas.gameObject.name = "MessagesCanvas";
			canvas.sortingOrder = 10;
			canvas.GetRequireComponent<CanvasScaler>().matchWidthOrHeight = 0f;
			yield return Singleton<ScenePrefab>.Instance.LoadPrefab<MessagesController>("MessagesController");
			MessagesController controller = Singleton<ScenePrefab>.Instance.Singleton<MessagesController>(canvas.transform);
			controller.Init();
			PauseButton pauseButton = GameController.Instance.GameView.GetView<PauseButton>();
			PauseView pauseView = GameController.Instance.GameView.GetView<PauseView>();
			MessagesPauseButton messagesPauseButton = pauseButton.GetRequireComponent<MessagesPauseButton>();
			MessagesButton messagesButton = pauseView.GetComponentInChildren<MessagesButton>(includeInactive: true);
			messagesButton.Show();
			pauseView.VisibleChanged += delegate(bool visible)
			{
				if (visible)
				{
					messagesButton.MessageCount = controller.GetNotificationsCount();
				}
			};
			messagesButton.OnClick = delegate
			{
				pauseView.Hide();
				controller.Open();
			};
			controller.OpenStateChangedEvent += GameController.Instance.DisableControlsHandler(controller);
			controller.OpenStateChangedEvent += delegate(bool isOpen)
			{
				pauseButton.IsVisible = !isOpen;
			};
			controller.NewNotificationEvent += delegate
			{
				messagesPauseButton.MessageCount = controller.GetNotificationsCount();
				if (pauseView.IsVisible)
				{
					messagesButton.MessageCount = controller.GetNotificationsCount();
				}
			};
			messagesPauseButton.MessageCount = controller.GetNotificationsCount();
			messagesButton.MessageCount = controller.GetNotificationsCount();
		}
	}
}
