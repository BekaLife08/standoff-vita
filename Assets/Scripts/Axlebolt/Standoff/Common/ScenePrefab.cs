using Axlebolt.Standoff.Core;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Axlebolt.Standoff.Common
{
	[RequireComponent(typeof(RectTransform))]
	public class ScenePrefab : Singleton<ScenePrefab>
	{
		private Canvas _canvas;

		private void Awake()
		{
			_canvas = GameObject.Find("Canvas").GetRequireComponent<Canvas>();
			base.transform.SetParent(_canvas.transform, worldPositionStays: false);
			base.transform.SetAsFirstSibling();
		}

		public IEnumerator LoadPrefab<T>(string sceneName) where T : MonoBehaviour
		{
			if (!SceneManager.GetSceneByName(sceneName).isLoaded)
			{
				SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
			}
			yield return null;
			GameObject[] rootGameObjects = SceneManager.GetSceneByName(sceneName).GetRootGameObjects();
			foreach (GameObject gameObject in rootGameObjects)
			{
				T componentInChildren = gameObject.GetComponentInChildren<T>();
				if (!((UnityEngine.Object)componentInChildren == (UnityEngine.Object)null))
				{
					componentInChildren.gameObject.SetActive(value: false);
					componentInChildren.transform.SetParent(base.transform, worldPositionStays: false);
				}
			}
			yield return SceneManager.UnloadSceneAsync(sceneName);
		}

		public T Singleton<T>(Transform parent) where T : MonoBehaviour
		{
			T componentInChildren = GetComponentInChildren<T>(includeInactive: true);
			if ((UnityEngine.Object)componentInChildren == (UnityEngine.Object)null)
			{
				throw new InvalidOperationException($"Prefab with type {typeof(T)} not found");
			}
			componentInChildren.transform.SetParent(parent, worldPositionStays: false);
			componentInChildren.gameObject.SetActive(value: true);
			return componentInChildren;
		}

		public T Instantiate<T>(Transform parent) where T : MonoBehaviour
		{
			T componentInChildren = GetComponentInChildren<T>(includeInactive: true);
			if ((UnityEngine.Object)componentInChildren == (UnityEngine.Object)null)
			{
				throw new InvalidOperationException($"Prefab with type {typeof(T)} not found");
			}
			T result = UnityEngine.Object.Instantiate(componentInChildren, parent, worldPositionStays: false);
			result.gameObject.SetActive(value: true);
			return result;
		}
	}
}
