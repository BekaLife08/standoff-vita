using Axlebolt.Standoff.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI
{
	public class TutorialView : View
	{
		[SerializeField]
		private Button _closeButton;

		private void Awake()
		{
			_closeButton.onClick.AddListener(delegate
			{
				UnityEngine.Object.Destroy(base.gameObject);
			});
			_closeButton.gameObject.SetActive(value: false);
			StartCoroutine(ShowAfter());
			base.transform.SetAsLastSibling();
		}

		private IEnumerator ShowAfter()
		{
			yield return new WaitForSeconds(12.5f);
			_closeButton.gameObject.SetActive(value: true);
		}
	}
}
