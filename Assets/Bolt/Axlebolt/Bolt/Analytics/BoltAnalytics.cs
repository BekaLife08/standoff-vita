using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Analytics
{
	public class BoltAnalytics : BoltService<BoltAnalytics>
	{
		private const string EventUrl = "http://{0}:{1}/{2}/{3}/{4}/{5}";

		private const string DefaultKey = "7773a71ce246d34a3009ecf1f6748b3b";

		private readonly ConcurrentQueue<AnalyticEvent> _queue;

		private readonly BoltApi _boltApi;

		private readonly Thread _worker;

		private bool _alive;

		public string Key { get; set; } = "7773a71ce246d34a3009ecf1f6748b3b";


		public BoltAnalytics()
		{
			_queue = new ConcurrentQueue<AnalyticEvent>();
			_boltApi = BoltApi.Instance;
			_alive = true;
			AnalyticEvent ae;
			_worker = new Thread((ThreadStart)delegate
			{
				while (_alive)
				{
					Thread.Sleep(200);
					while (_queue.TryDequeue(out ae))
					{
						//WorkerSend(ae);
					}
				}
			});
		}

		internal override void Load()
		{
			Enable();
		}

		internal override void Destroy()
		{
			base.Destroy();
			Disable();
		}

		public void Enable()
		{
			_worker.Start();
		}

		public void Disable()
		{
			_alive = false;
			_worker.Join();
		}

		public void SendEvent(string category, string @event)
		{
			SendEvent(category, @event, 1);
		}

		public void SendEvent(string category, string @event, int count)
		{
			if (_alive)
			{
				_queue.Enqueue(new AnalyticEvent(category, @event, count));
			}
		}

		public int QueuedEvents()
		{
			return _queue.Count;
		}

		private void WorkerSend(AnalyticEvent ae)
		{
			try
			{
				string requestUriString = string.Format("http://{0}:{1}/{2}/{3}/{4}/{5}", _boltApi.Ip, _boltApi.HttpPort, Key, ae.Category, ae.Event, ae.Count);
				WebRequest webRequest = WebRequest.Create(requestUriString);
				webRequest.Timeout = 1000;
				webRequest.GetResponse().Close();
				if (Logger.LogDebug)
				{
					Logger.Debug(string.Format("Event '{0}.{1}' ({2}) successfully sent!", ae.Category, ae.Event, ae.Count));
				}
			}
			catch (WebException ex)
			{
				if (Logger.LogDebug)
				{
					Logger.Debug(ex.Message);
				}
			}
			catch (Exception ex2)
			{
				Logger.Error(ex2.Message);
			}
		}
	}
}
