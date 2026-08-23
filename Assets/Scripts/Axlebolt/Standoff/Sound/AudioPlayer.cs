using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Sound
{
	public class AudioPlayer : MonoBehaviour
	{
		private List<AudioSource> _freeAudioSourcesList = new List<AudioSource>();

		private List<AudioSource> _involvedAudioSourcesList = new List<AudioSource>();

		private Transform _parent;

		private Vector3 _localPosition;

		public bool PlayDisabled
		{
			get;
			set;
		}

		public void Initialize(Transform parent, Vector3 localPosition, int audioSourcesCount)
		{
			for (int i = 0; i < audioSourcesCount; i++)
			{
				_freeAudioSourcesList.Add(base.gameObject.AddComponent<AudioSource>());
			}
			_localPosition = localPosition;
			_parent = parent;
		}

		public List<AudioSource> GetAudioSources(int count)
		{
			if (_freeAudioSourcesList.Count == 0)
			{
				UnityEngine.Debug.LogError("No free AudioSource. " + base.gameObject.name);
				return new List<AudioSource>();
			}
			if (_freeAudioSourcesList.Count < count)
			{
				UnityEngine.Debug.LogError("Not enough AudioSources. " + _freeAudioSourcesList.Count + " < " + count + ".  " + base.gameObject.name);
				count = _freeAudioSourcesList.Count;
			}
			List<AudioSource> list = new List<AudioSource>();
			for (int i = 0; i < count; i++)
			{
				AudioSource item = _freeAudioSourcesList[i];
				list.Add(_freeAudioSourcesList[i]);
				_involvedAudioSourcesList.Add(item);
			}
			_freeAudioSourcesList.RemoveRange(0, count);
			return list;
		}

		public void ReturnAudioSources(List<AudioSource> audioSources)
		{
			foreach (AudioSource audioSource in audioSources)
			{
				audioSource.spatialBlend = 0f;
				audioSource.pitch = 1f;
				audioSource.volume = 1f;
				audioSource.panStereo = 0f;
				_freeAudioSourcesList.Add(audioSource);
				_involvedAudioSourcesList.Remove(audioSource);
			}
		}

		private bool IsPlaying()
		{
			foreach (AudioSource involvedAudioSources in _involvedAudioSourcesList)
			{
				if (involvedAudioSources.isPlaying)
				{
					return true;
				}
			}
			return false;
		}

		public void Play()
		{
		}

		private void LateUpdate()
		{
			base.transform.position = _parent.TransformPoint(_localPosition);
		}
	}
}
