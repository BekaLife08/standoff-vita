using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Mecanim
{
	public class MecanimSync
	{
		public class LayerTimeInfo
		{
			public float transitionStartTime;

			public int lastTransitionHash;

			public float stateEnterTime;

			public int lastStateHash;
		}

		public List<MecanimTransitionInfo> transitionList;

		private PlayerMecanimConfig config;

		public Animator animator;

		private List<int> nonSyncParamsList = new List<int>();

		private List<AnimatorControllerParameter> _animatorParameters;

		private MecanimSyncData _lastReceivedSnapshot;

		private Dictionary<int, List<int>> _layerBufferedParameters = new Dictionary<int, List<int>>();

		private List<LayerTimeInfo> layerTimeInfoList = new List<LayerTimeInfo>();

		public List<List<MecanimLayerInfo>> layerDataBuffer = new List<List<MecanimLayerInfo>>();

		private float _localTime;

		private bool _isNetworkSync;

		public void PreInitialize(Animator animator, PlayerMecanimConfig config)
		{
			this.animator = animator;
			this.config = config;
			transitionList = config.transitionList;
			_animatorParameters = animator.parameters.ToList();
			foreach (string nonSyncParatetre in config.nonSyncParatetres)
			{
				nonSyncParamsList.Add(Animator.StringToHash(nonSyncParatetre));
			}
			for (int i = 0; i < animator.layerCount; i++)
			{
				layerTimeInfoList.Add(new LayerTimeInfo());
			}
		}

		public void Initialize(Animator animator)
		{
			this.animator = animator;
			_isNetworkSync = true;
			foreach (PlayerMecanimConfig.BufferedFloatParameters bufferedFloatParam in config.BufferedFloatParams)
			{
				if (!_layerBufferedParameters.ContainsKey(bufferedFloatParam.LayerId))
				{
					_layerBufferedParameters.Add(bufferedFloatParam.LayerId, new List<int>());
				}
				_layerBufferedParameters[bufferedFloatParam.LayerId].Add(bufferedFloatParam.Hash);
			}
		}

		public void EvaluateTimePoints(float time, bool isNetworkSync)
		{
			_localTime = time;
			_isNetworkSync = isNetworkSync;
			List<MecanimLayerInfo> list = new List<MecanimLayerInfo>();
			for (int i = 0; i < animator.layerCount; i++)
			{
				MecanimLayerInfo layerInfo = GetLayerInfo(i);
				list.Add(layerInfo);
			}
			layerDataBuffer.Add(list);
			if (layerDataBuffer.Count > 10)
			{
				layerDataBuffer.RemoveRange(0, layerDataBuffer.Count - 10);
			}
		}

		private void SortByTransitionLength(List<MecanimLayerInfo> transitioningLayers)
		{
			for (int i = 0; i < transitioningLayers.Count - 1; i++)
			{
				for (int j = i + 1; j < transitioningLayers.Count; j++)
				{
					float pastTransTime = transitioningLayers[i].GetPastTransTime();
					float pastTransTime2 = transitioningLayers[j].GetPastTransTime();
					if (pastTransTime < pastTransTime2)
					{
						MecanimLayerInfo value = transitioningLayers[i];
						transitioningLayers[i] = transitioningLayers[j];
						transitioningLayers[j] = value;
					}
				}
			}
		}

		private MecanimTransitionInfo GetTransitionInfo(int nameHash)
		{
			foreach (MecanimTransitionInfo transition in transitionList)
			{
				if (nameHash == transition.GetNameHash())
				{
					return transition;
				}
			}
			return null;
		}

		private bool IsTransitionNTMatch(MecanimSyncData snapshot)
		{
			for (int i = 0; i < animator.layerCount; i++)
			{
				if (snapshot.layers[i].isInTransition != animator.IsInTransition(i))
				{
					return false;
				}
				AnimatorTransitionInfo animatorTransitionInfo = animator.GetAnimatorTransitionInfo(i);
				if (snapshot.layers[i].isInTransition && Mathf.Abs(snapshot.layers[i].transitionNormalizedTime - animatorTransitionInfo.normalizedTime) > config.maxTransitionNormalizedTimeDeviation)
				{
					return false;
				}
			}
			return true;
		}

		private void RemapStateSync(MecanimSyncData snapshot)
		{
			MecanimLayerInfo[] layers = snapshot.layers;
			foreach (MecanimLayerInfo mecanimLayerInfo in layers)
			{
				PlayerMecanimConfig.StatePair targetState = GetTargetState(mecanimLayerInfo.stateNameHash);
				if (targetState != null)
				{
					mecanimLayerInfo.stateNameHash = targetState.TargetStateHash;
					if (mecanimLayerInfo.isInTransition)
					{
						mecanimLayerInfo.isInTransition = false;
					}
				}
				else if (mecanimLayerInfo.isInTransition)
				{
					PlayerMecanimConfig.StatePair targetState2 = GetTargetState(mecanimLayerInfo.nextStateNameHash);
					if (targetState2 != null)
					{
						mecanimLayerInfo.isInTransition = false;
						mecanimLayerInfo.stateNameHash = targetState2.TargetStateHash;
					}
				}
			}
		}

		public MecanimLayerInfo GetSynchronizedLayerInfo(int layerInd)
		{
			if (_lastReceivedSnapshot != null)
			{
				_lastReceivedSnapshot.layers[layerInd].IsSynchronized = true;
				return _lastReceivedSnapshot.layers[layerInd];
			}
			return null;
		}

		public void SetAnimatorSnapshot(MecanimSyncData snapshot, bool isNetworkSync, bool evaluateAnimator)
		{
			if (snapshot.layers == null || snapshot.layers.Length == 0)
			{
				return;
			}
			_lastReceivedSnapshot = snapshot;
			_isNetworkSync = isNetworkSync;
			if (_isNetworkSync)
			{
				RemapStateSync(snapshot);
			}
			if (!evaluateAnimator)
			{
				return;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (AnimatorControllerParameter animatorParameter in _animatorParameters)
			{
				if (!nonSyncParamsList.Contains(animatorParameter.nameHash))
				{
					if (!isNetworkSync)
					{
						if (animatorParameter.type == AnimatorControllerParameterType.Bool && snapshot.boolParams != null && snapshot.boolParams.Length > 0)
						{
							animator.SetBool(animatorParameter.nameHash, snapshot.boolParams[num]);
							num++;
						}
						if (animatorParameter.type == AnimatorControllerParameterType.Int && snapshot.intParams != null && snapshot.intParams.Length > 0)
						{
							animator.SetInteger(animatorParameter.nameHash, snapshot.intParams[num3]);
							num3++;
						}
					}
					if (animatorParameter.type == AnimatorControllerParameterType.Float && snapshot.floatParams != null && snapshot.floatParams.Length > 0)
					{
						animator.SetFloat(animatorParameter.nameHash, snapshot.floatParams[num2]);
						num2++;
					}
				}
			}
			if (!IsTransitionNTMatch(snapshot))
			{
				List<MecanimLayerInfo> list = new List<MecanimLayerInfo>();
				MecanimLayerInfo[] layers = snapshot.layers;
				foreach (MecanimLayerInfo mecanimLayerInfo in layers)
				{
					if (mecanimLayerInfo.isInTransition)
					{
						list.Add(mecanimLayerInfo);
					}
				}
				if (list.Count > 0)
				{
					SortByTransitionLength(list);
					MecanimLayerInfo mecanimLayerInfo2 = null;
					foreach (MecanimLayerInfo item in list)
					{
						MecanimLayerInfo mecanimLayerInfo3 = item;
						if (mecanimLayerInfo2 != null)
						{
							float deltaTime = mecanimLayerInfo2.GetPastTransTime() - mecanimLayerInfo3.GetPastTransTime();
							animator.Update(deltaTime);
						}
						MecanimTransitionInfo transitionInfo = GetTransitionInfo(mecanimLayerInfo3.transitionNameHash);
						if (transitionInfo == null)
						{
							UnityEngine.Debug.LogError("No Transition Found");
							return;
						}
						animator.CrossFade(mecanimLayerInfo3.nextStateNameHash, transitionInfo.duration, mecanimLayerInfo3.layerInd, transitionInfo.offset);
						animator.Update(0f);
						mecanimLayerInfo2 = mecanimLayerInfo3;
					}
					MecanimLayerInfo mecanimLayerInfo4 = list[list.Count - 1];
					animator.Update(mecanimLayerInfo4.GetPastTransTime());
				}
			}
			MecanimLayerInfo[] layers2 = snapshot.layers;
			foreach (MecanimLayerInfo mecanimLayerInfo5 in layers2)
			{
				if (!mecanimLayerInfo5.isInTransition)
				{
					animator.Play(mecanimLayerInfo5.stateNameHash, mecanimLayerInfo5.layerInd, mecanimLayerInfo5.stateNormalizedTime);
				}
				foreach (MecanimLayerInfo.ParameterPair bufferedParameters in mecanimLayerInfo5.BufferedParametersList)
				{
					animator.SetFloat(bufferedParameters.Hash, bufferedParameters.Value);
				}
			}
		}

		private PlayerMecanimConfig.StatePair GetTargetState(int sourceNameHash)
		{
			foreach (PlayerMecanimConfig.StatePair item in config.StateRemaper)
			{
				if (item.SourceStateHash == sourceNameHash)
				{
					return item;
				}
			}
			return null;
		}

		private MecanimLayerInfo GetLayerInfo(int i)
		{
			MecanimLayerInfo mecanimLayerInfo = new MecanimLayerInfo();
			mecanimLayerInfo.layerInd = i;
			mecanimLayerInfo.time = _localTime;
			AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(i);
			mecanimLayerInfo.stateNameHash = currentAnimatorStateInfo.shortNameHash;
			mecanimLayerInfo.stateNormalizedTime = currentAnimatorStateInfo.normalizedTime;
			mecanimLayerInfo.stateLength = currentAnimatorStateInfo.length / (currentAnimatorStateInfo.speed * currentAnimatorStateInfo.speedMultiplier);
			bool flag = animator.IsInTransition(i);
			if (flag)
			{
				mecanimLayerInfo.isInTransition = flag;
				currentAnimatorStateInfo = animator.GetNextAnimatorStateInfo(i);
				AnimatorTransitionInfo animatorTransitionInfo = animator.GetAnimatorTransitionInfo(i);
				mecanimLayerInfo.nextStateNameHash = currentAnimatorStateInfo.shortNameHash;
				MecanimTransitionInfo transitionInfo = GetTransitionInfo(animatorTransitionInfo.userNameHash);
				if (transitionInfo != null)
				{
					mecanimLayerInfo.transitionDuration = GetTransitionInfo(animatorTransitionInfo.userNameHash).duration;
					mecanimLayerInfo.transitionNameHash = animatorTransitionInfo.userNameHash;
					mecanimLayerInfo.transitionNormalizedTime = animatorTransitionInfo.normalizedTime;
				}
			}
			if (_layerBufferedParameters.ContainsKey(i))
			{
				List<int> list = _layerBufferedParameters[i];
				{
					foreach (int item in list)
					{
						mecanimLayerInfo.BufferedParametersList.Add(new MecanimLayerInfo.ParameterPair
						{
							Hash = item,
							Value = animator.GetFloat(item)
						});
					}
					return mecanimLayerInfo;
				}
			}
			return mecanimLayerInfo;
		}

		public MecanimSyncData GetAnimatorSnapshot(bool isNetworkSync)
		{
			_isNetworkSync = isNetworkSync;
			MecanimSyncData mecanimSyncData = new MecanimSyncData();
			List<bool> list = new List<bool>();
			List<int> list2 = new List<int>();
			List<float> list3 = new List<float>();
			AnimatorControllerParameter[] parameters = animator.parameters;
			foreach (AnimatorControllerParameter animatorControllerParameter in parameters)
			{
				if (nonSyncParamsList.Contains(animatorControllerParameter.nameHash))
				{
					continue;
				}
				if (!isNetworkSync)
				{
					if (animatorControllerParameter.type == AnimatorControllerParameterType.Bool)
					{
						list.Add(animator.GetBool(animatorControllerParameter.nameHash));
					}
					if (animatorControllerParameter.type == AnimatorControllerParameterType.Int)
					{
						list2.Add(animator.GetInteger(animatorControllerParameter.nameHash));
					}
				}
				if (animatorControllerParameter.type == AnimatorControllerParameterType.Float)
				{
					list3.Add(animator.GetFloat(animatorControllerParameter.nameHash));
				}
			}
			mecanimSyncData.boolParams = list.ToArray();
			mecanimSyncData.intParams = list2.ToArray();
			mecanimSyncData.floatParams = list3.ToArray();
			if (!isNetworkSync)
			{
				List<MecanimLayerInfo> list4 = new List<MecanimLayerInfo>();
				for (int j = 0; j < animator.layerCount; j++)
				{
					list4.Add(GetLayerInfo(j));
				}
				mecanimSyncData.layers = list4.ToArray();
			}
			mecanimSyncData.dTime = Time.deltaTime;
			if (layerDataBuffer.Count == 0)
			{
				EvaluateTimePoints(_localTime, isNetworkSync);
			}
			for (int k = 0; k < layerDataBuffer.Count; k++)
			{
				List<MecanimLayerInfo> list5 = layerDataBuffer[k];
				List<MecanimLayerInfo> list6 = new List<MecanimLayerInfo>();
				if (list5 != null)
				{
					foreach (MecanimLayerInfo item in list5)
					{
						list6.Add(item);
					}
				}
				mecanimSyncData.layerBuffer.layerDataBuffer.Add(list6);
			}
			mecanimSyncData.layerCount = animator.layerCount;
			if (layerDataBuffer.Count > 0)
			{
				layerDataBuffer.RemoveRange(0, layerDataBuffer.Count - 1);
			}
			return mecanimSyncData;
		}
	}
}
