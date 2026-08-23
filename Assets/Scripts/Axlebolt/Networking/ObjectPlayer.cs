using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Networking
{
	public abstract class ObjectPlayer : MonoBehaviour
	{
		private readonly List<ObjectSnapshot> _snapshotBuffer = new List<ObjectSnapshot>();

		private float _snapshotReceivedTime = -1f;

		private float _receivedServerTime = -1f;

		private float _clientTime = -1f;

		private readonly ObjectPlayerConfig _config = new ObjectPlayerConfig();

		public float InterpolationDelay
		{
			get;
			set;
		} = 0.2f;


		public float MaxExtrapolationDuration
		{
			get;
			set;
		} = 0.25f;


		public int BufferLength
		{
			get;
			set;
		} = 100;


		public virtual void PreInitialize()
		{
		}

		public virtual void Initialize()
		{
		}

		public virtual void Clear()
		{
			_clientTime = 0f;
			_receivedServerTime = 0f;
			_snapshotBuffer.Clear();
		}

		public void AddSnapshot(ObjectSnapshot snapshot)
		{
			_receivedServerTime = snapshot.time;
			_snapshotReceivedTime = Time.time;
			_snapshotBuffer.Add(snapshot);
			if (_snapshotBuffer.Count > BufferLength)
			{
				_snapshotBuffer.RemoveRange(0, _snapshotBuffer.Count - BufferLength);
			}
		}

		private void UpdateClientTime()
		{
			float num = _receivedServerTime + (Time.time - _snapshotReceivedTime);
			_clientTime += Time.deltaTime;
			if (Mathf.Abs(num - _clientTime) > _config.timeMaxDeviation)
			{
				_clientTime = num;
				return;
			}
			float f = num - _clientTime;
			if (Mathf.Abs(f) > _config.timeCorrectorGap)
			{
				float f2 = Mathf.Sign(f) * _config.timeCorrectorSpeed * Time.deltaTime;
				f2 = Mathf.Clamp(Mathf.Abs(f2), 0f, Mathf.Abs(Mathf.Abs(f) - _config.timeCorrectorGap)) * Mathf.Sign(f);
				_clientTime += f2;
			}
		}

		private int FindFromSnapshot(float time)
		{
			if (_snapshotBuffer.Count == 0 || time < _snapshotBuffer[0].time)
			{
				return -1;
			}
			if (_snapshotBuffer[_snapshotBuffer.Count - 1].time < time)
			{
				return _snapshotBuffer.Count - 1;
			}
			for (int i = 0; i < _snapshotBuffer.Count - 1; i++)
			{
				if (_snapshotBuffer[i].time <= time && time < _snapshotBuffer[i + 1].time)
				{
					return i;
				}
			}
			return -1;
		}

		public bool Play(float time)
		{
			float num = time - InterpolationDelay;
			int num2 = FindFromSnapshot(num);
			if (num2 == -1)
			{
				return false;
			}
			Interpolator interpolator = GetInterpolator();
			if (num2 == _snapshotBuffer.Count - 1)
			{
				ObjectSnapshot snapshot = interpolator.Interpolate(_snapshotBuffer[_snapshotBuffer.Count - 1], _snapshotBuffer[_snapshotBuffer.Count - 1], 1f);
				SetSnapshot(snapshot);
				return true;
			}
			int index = num2 + 1;
			float progress = (num - _snapshotBuffer[num2].time) / (_snapshotBuffer[index].time - _snapshotBuffer[num2].time);
			ObjectSnapshot snapshot2 = interpolator.Interpolate(_snapshotBuffer[num2], _snapshotBuffer[index], progress);
			SetSnapshot(snapshot2);
			return true;
		}

		public abstract Interpolator GetInterpolator();

		protected abstract void SetSnapshot(ObjectSnapshot snapshot);

		public void Evaluate()
		{
			Update();
		}

		private void Update()
		{
			UpdateClientTime();
			Play(_clientTime);
		}
	}
}
