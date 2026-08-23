using Axlebolt.Standoff.Common;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Settings.Controls
{
	public class ControlsSettingsManager
	{
		private readonly string _key = "ControlSettings";

		private static ControlsSettingsManager _instance;

		private readonly IFileStorage _storage;

		public static ControlsSettingsManager Instance
		{
			get
			{
				if (_instance == null)
				{
					Init();
				}
				return _instance;
			}
		}

		public ControlsSettings Model
		{
			get;
			private set;
		}

		public event Action<ControlsSettings> SettingsChangedEvent;

		private ControlsSettingsManager(IFileStorage settingsStorage)
		{
			_storage = settingsStorage;
			_key = _key + "_" + SystemInfo.deviceName;
			Model = _storage.Load(_key, DefaultValue());
			FixNull();
		}

		private void FixNull()
		{
			if (Model.CustomSettingsNull)
			{
				Model.CustomSettings = null;
			}
		}

		public static void Init()
		{
			Init(new PrefsStorage());
		}

		public static void Init(IFileStorage storage)
		{
			_instance = new ControlsSettingsManager(storage);
		}

		public static ControlsSettings DefaultValue()
		{
			return new ControlsSettings();
		}

		public IEnumerator Save(ControlsSettings model)
		{
			Model = model;
			Model.CustomSettingsNull = (Model.CustomSettings == null);
			OnSettingsChangedEvent(Model);
			yield return _storage.Save(_key, Model);
			FixNull();
		}

		public static void SetControlSettings(RectTransform control, ControlsCustomSettings.Element settings)
		{
			if (settings != null)
			{
				control.position = new Vector3(settings.PosX, settings.PosY, 0f);
				Graphic[] componentsInChildren = control.GetComponentsInChildren<Graphic>();
				foreach (Graphic graphic in componentsInChildren)
				{
					Graphic graphic2 = graphic;
					Color color = graphic.color;
					float r = color.r;
					Color color2 = graphic.color;
					float g = color2.g;
					Color color3 = graphic.color;
					graphic2.color = new Color(r, g, color3.b, settings.Alpha);
				}
				Vector2 sizeDelta = control.sizeDelta;
				float y = sizeDelta.y;
				Vector2 sizeDelta2 = control.sizeDelta;
				float num = y / sizeDelta2.x;
				control.sizeDelta = new Vector2(settings.SizeX, settings.SizeX * num);
			}
		}

		protected virtual void OnSettingsChangedEvent(ControlsSettings obj)
		{
			if (this.SettingsChangedEvent != null)
			{
				this.SettingsChangedEvent(obj);
			}
		}
	}
}
