using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Controls
{
	public class HitIndicatorView : HudComponentView
	{
		[SerializeField]
		private Image _front;

		[SerializeField]
		private Image _right;

		[SerializeField]
		private Image _back;

		[SerializeField]
		private Image _left;

		private PlayerController _playerController;

		private void OnEnable()
		{
			_front.gameObject.SetActive(value: false);
			_right.gameObject.SetActive(value: false);
			_back.gameObject.SetActive(value: false);
			_left.gameObject.SetActive(value: false);
		}

		public override void SetPlayerController(PlayerController playerController)
		{
			if (_playerController != null)
			{
				_playerController.HitController.OnHit = null;
			}
			_playerController = playerController;
			if (_playerController != null)
			{
				_playerController.HitController.OnHit = OnHit;
			}
		}

		public override void UpdateView(PlayerController playerController)
		{
		}

		private void OnHit(HitData hitData)
		{
			Image indicator = GetIndicator(hitData);
			indicator.gameObject.SetActive(value: false);
			indicator.gameObject.SetActive(value: true);
		}

		private Image GetIndicator(HitData hitData)
		{
			Vector3 forward = _playerController.transform.forward;
			forward = new Vector3(forward.x, 0f, forward.z);
			Vector3 vector = -hitData.Direction;
			vector = new Vector3(vector.x, 0f, vector.z);
			float num = VectorAngle.AngleDirected(vector, forward, Vector3.up);
			if (num > -45f && num <= 45f)
			{
				return _front;
			}
			if (num > 45f && num <= 135f)
			{
				return _right;
			}
			if (num > 135f || num < -135f)
			{
				return _back;
			}
			return _left;
		}
	}
}
