using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Axlebolt.Standoff.Game.UI
{
	public class GameView : View
	{
		private static readonly Log Log = Log.Create(typeof(GameView));

		private readonly Dictionary<Type, View> _views = new Dictionary<Type, View>();

		private IPlayerPropSensitiveView[] _refreshViews = new IPlayerPropSensitiveView[0];

		private bool _initialized;

		public static GameView Instance
		{
			get;
			private set;
		}

		private void Awake()
		{
			Instance = this;
		}

		public IEnumerator Init()
		{
			_initialized = true;
			for (int i = 0; i < base.transform.childCount; i++)
			{
				View component = base.transform.GetChild(i).GetComponent<View>();
				if (component != null)
				{
					_views[component.GetType()] = component;
					component.gameObject.SetActive(value: true);
				}
			}
			yield return null;
			for (int j = 0; j < base.transform.childCount; j++)
			{
				View component2 = base.transform.GetChild(j).GetComponent<View>();
				if (component2 != null)
				{
					_views[component2.GetType()] = component2;
					component2.gameObject.SetActive(value: false);
				}
			}
			_refreshViews = GetComponentsInChildren<IPlayerPropSensitiveView>(includeInactive: true);
		}

		public T GetView<T>() where T : View
		{
			CheckInitialized();
			if (_views.TryGetValue(typeof(T), out View value))
			{
				return (T)value;
			}
			throw new KeyNotFoundException($"View {typeof(T).Name} not found");
		}

		public void Refresh()
		{
			Refresh(null);
		}

		public void Refresh(IEnumerable<object> changedProps)
		{
			CheckInitialized();
			IPlayerPropSensitiveView[] refreshViews = _refreshViews;
			foreach (IPlayerPropSensitiveView playerPropSensitiveView in refreshViews)
			{
				if (playerPropSensitiveView.IsVisible)
				{
					string[] sensitivePlayerProperties = playerPropSensitiveView.SensitivePlayerProperties;
					if (changedProps == null || sensitivePlayerProperties.Any((string property) => changedProps.Any((object changedProp) => object.Equals(changedProp, property))))
					{
						playerPropSensitiveView.Refresh();
						Log.Debug($"{playerPropSensitiveView.GetType().Name} updated");
					}
				}
			}
		}

		private void CheckInitialized()
		{
			if (!_initialized)
			{
				throw new InvalidOperationException("GameView not initialzied");
			}
		}

		private void OnDestroy()
		{
			Instance = null;
		}
	}
}
