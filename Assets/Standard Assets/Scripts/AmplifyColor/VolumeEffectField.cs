using System;
using System.Reflection;
using UnityEngine;

namespace AmplifyColor
{
	[Serializable]
	public class VolumeEffectField
	{
		public string fieldName;

		public string fieldType;

		public float valueSingle;

		public Color valueColor;

		public bool valueBoolean;

		public Vector2 valueVector2;

		public Vector3 valueVector3;

		public Vector4 valueVector4;

		public VolumeEffectField(string fieldName, string fieldType)
		{
			this.fieldName = fieldName;
			this.fieldType = fieldType;
		}

		public VolumeEffectField(FieldInfo pi, Component c)
			: this(pi.Name, pi.FieldType.FullName)
		{
			object value = pi.GetValue(c);
			UpdateValue(value);
		}

		public static bool IsValidType(string type)
		{
			if (type != null && (type == "System.Single" || type == "System.Boolean" || type == "UnityEngine.Color" || type == "UnityEngine.Vector2" || type == "UnityEngine.Vector3" || type == "UnityEngine.Vector4"))
			{
				return true;
			}
			return false;
		}

		public void UpdateValue(object val)
		{
			string text = fieldType;
			if (text == null)
			{
				return;
			}
			if (!(text == "System.Single"))
			{
				if (!(text == "System.Boolean"))
				{
					if (!(text == "UnityEngine.Color"))
					{
						if (!(text == "UnityEngine.Vector2"))
						{
							if (!(text == "UnityEngine.Vector3"))
							{
								if (text == "UnityEngine.Vector4")
								{
									valueVector4 = (Vector4)val;
								}
							}
							else
							{
								valueVector3 = (Vector3)val;
							}
						}
						else
						{
							valueVector2 = (Vector2)val;
						}
					}
					else
					{
						valueColor = (Color)val;
					}
				}
				else
				{
					valueBoolean = (bool)val;
				}
			}
			else
			{
				valueSingle = (float)val;
			}
		}
	}
}
