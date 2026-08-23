using Axlebolt.Standoff.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Matchmaking
{
	public class RegionSelectDialog : View
	{
		[SerializeField]
		private RegionSelectRow _regionSelectRow;

		[SerializeField]
		private Color _greenColor;

		[SerializeField]
		private Color _yellowColor;

		[SerializeField]
		private Color _redColor;

		[SerializeField]
		private CloseButton _closeButton;

		private Dictionary<PhotonServer, RegionSelectRow> _items;

		private Transform _canvasTransform;

		private bool _result;

		public PhotonServer Server
		{
			get;
			private set;
		}

		public bool SaveSelection
		{
			get;
			set;
		}

		private void Awake()
		{
			_items = new Dictionary<PhotonServer, RegionSelectRow>();
			PhotonServer[] servers = Regions.Servers;
			foreach (PhotonServer server in servers)
			{
				RegionSelectRow regionSelectRow = UnityEngine.Object.Instantiate(_regionSelectRow, _regionSelectRow.transform.parent, worldPositionStays: false);
				regionSelectRow.RegionNameText.text = server.GetDisplayName();
				regionSelectRow.ActionHandler = delegate
				{
					SetResult(server);
				};
				_items[server] = regionSelectRow;
			}
			_regionSelectRow.Hide();
			_canvasTransform = ViewUtility.GetCanvas(base.transform).transform;
			_closeButton.CloseHandler = delegate
			{
				SetResult(null);
			};
		}

		public IEnumerator ShowAndWait()
		{
			base.Show();
			StartCoroutine(StartPingCoroutine());
			_result = false;
			Server = null;
			base.transform.SetParent(_canvasTransform, worldPositionStays: false);
			base.transform.SetAsLastSibling();
			while (!_result)
			{
				yield return null;
			}
			if (SaveSelection && Server != null)
			{
				yield return Regions.SetCurrentRegion(Server);
			}
			StartCoroutine(HideNextFrame());
		}

		public override void Show()
		{
			throw new NotImplementedException();
		}

		private IEnumerator StartPingCoroutine()
		{
			foreach (RegionSelectRow value in _items.Values)
			{
				value.PingIndicatorImage.color = _redColor;
			}
			Dictionary<PhotonServer, Ping> pings = new Dictionary<PhotonServer, Ping>();
			PhotonServer[] servers = Regions.Servers;
			foreach (PhotonServer photonServer in servers)
			{
				pings[photonServer] = new Ping(photonServer.Ip);
			}
			while (base.IsVisible)
			{
				PhotonServer[] servers2 = Regions.Servers;
				foreach (PhotonServer photonServer2 in servers2)
				{
					if (pings[photonServer2].isDone)
					{
						_items[photonServer2].PingIndicatorImage.color = GetPingColor(pings[photonServer2].time);
						pings[photonServer2] = new Ping(photonServer2.Ip);
					}
				}
				yield return null;
			}
		}

		private void SetResult(PhotonServer server)
		{
			_result = true;
			Server = server;
		}

		private Color GetPingColor(int ping)
		{
			if (ping < 130)
			{
				return _greenColor;
			}
			if (ping < 250)
			{
				return _yellowColor;
			}
			return _redColor;
		}

		private IEnumerator HideNextFrame()
		{
			yield return null;
			Hide();
		}

		public override void Hide()
		{
			StopAllCoroutines();
			base.Hide();
		}
	}
}
