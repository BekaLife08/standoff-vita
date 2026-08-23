using UnityEngine;
using UnityEngine.UI;

public class UI_LogData : MonoBehaviour
{
	public enum Type
	{
		Text,
		Slider
	}

	public Text header;

	public Text value;

	public Slider slider;

	private float range1 = -1f;

	private float range2 = 1f;

	private Type _type;

	[HideInInspector]
	public bool isDirty;

	public bool isEmpty = true;

	public void Log(string title, float value, Type type)
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
		}
		header.text = title;
		slider.gameObject.SetActive(value: false);
		this.value.gameObject.SetActive(value: false);
		isDirty = true;
		isEmpty = false;
		if (type == Type.Text)
		{
			this.value.text = string.Empty + value;
			this.value.gameObject.SetActive(value: true);
		}
		if (type == Type.Slider)
		{
			slider.value = (value - range1) / (range2 - range1);
			slider.gameObject.SetActive(value: true);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (base.gameObject.activeSelf)
		{
			isEmpty = true;
			base.gameObject.SetActive(value: false);
		}
	}
}
