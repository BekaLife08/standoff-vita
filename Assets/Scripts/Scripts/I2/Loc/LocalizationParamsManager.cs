using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizationParamsManager : MonoBehaviour, ILocalizationParamsManager
	{
		[Serializable]
		public struct ParamValue
		{
			public string Name;

			public string Value;
		}

		[SerializeField]
		public List<ParamValue> _Params = new List<ParamValue>();

		public string GetParameterValue(string ParamName)
		{
			if (_Params != null)
			{
				int i = 0;
				for (int count = _Params.Count; i < count; i++)
				{
					ParamValue paramValue = _Params[i];
					if (paramValue.Name == ParamName)
					{
						ParamValue paramValue2 = _Params[i];
						return paramValue2.Value;
					}
				}
			}
			return null;
		}

		public void SetParameterValue(string ParamName, string ParamValue, bool localize = true)
		{
			bool flag = false;
			int i = 0;
			for (int count = _Params.Count; i < count; i++)
			{
				ParamValue paramValue = _Params[i];
				if (paramValue.Name == ParamName)
				{
					ParamValue value = _Params[i];
					value.Value = ParamValue;
					_Params[i] = value;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				_Params.Add(new ParamValue
				{
					Name = ParamName,
					Value = ParamValue
				});
			}
			if (localize)
			{
				OnLocalize();
			}
		}

		public void OnLocalize()
		{
			Localize component = GetComponent<Localize>();
			if (component != null)
			{
				component.OnLocalize(Force: true);
			}
		}
	}
}
