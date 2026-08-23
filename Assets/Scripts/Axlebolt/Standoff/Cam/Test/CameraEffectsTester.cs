using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.Player.Ragdoll;
using Axlebolt.Standoff.Player.Ragdoll.Test;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Cam.Test
{
	public class CameraEffectsTester : Singleton<CameraEffectsTester>
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CPlay_003Ec__async0 : IAsyncStateMachine
		{
			internal CameraEffectsTester _0024this;

			internal AsyncVoidMethodBuilder _0024builder;

			internal int _0024PC;

			private CoroutineAsyncExtensions.CoroutineAsyncBridge _0024awaiter0;

			public void MoveNext()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				try
				{
					switch (num)
					{
						default:
							return;
						case 0u:
							RagdollTestController.Instance.Hit();
							_0024awaiter0 = Singleton<KillCamera>.Instance.PlayerDieEffect.PlayEffect(Singleton<RagdollManager>.Instance.GetActiveRagdollByPlayerId(_0024this._playerController.PlayerId).Character, _0024this._kllerTransform).GetAwaiter();
							if (!_0024awaiter0.IsCompleted)
							{
								_0024PC = 1;
								_0024builder.AwaitOnCompleted(ref _0024awaiter0, ref this);
								return;
							}
							goto case 1u;
						case 1u:
							_0024awaiter0.GetResult();
							_0024awaiter0 = Singleton<KillCamera>.Instance.PlayerFocusEffect.PlayEffect(_0024this._kllerTransform, _0024this._screenshotImage).GetAwaiter();
							if (!_0024awaiter0.IsCompleted)
							{
								_0024PC = 2;
								_0024builder.AwaitOnCompleted(ref _0024awaiter0, ref this);
								return;
							}
							break;
						case 2u:
							break;
					}
					_0024awaiter0.GetResult();
					Singleton<KillCamera>.Instance.PlayerFocusEffect.FinishFocusing();
				}
				catch (Exception exception)
				{
					_0024PC = -1;
					_0024builder.SetException(exception);
					return;
				}
				_0024PC = -1;
				_0024builder.SetResult();
			}

			[DebuggerHidden]
			public void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_0024builder.SetStateMachine(stateMachine);
			}
		}

		[SerializeField]
		private Transform _kllerTransform;

		[SerializeField]
		private Camera _camera;

		[SerializeField]
		private RawImage _screenshotImage;

		private PlayerController _playerController;

		private void Start()
		{
			_camera.gameObject.AddComponent<KillCamera>();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.I))
			{
				_playerController = RagdollTestController.Instance.InitPlayer();
			}
			if (_playerController != null && Input.GetKeyDown(KeyCode.E))
			{
				Play();
			}
		}

		[AsyncStateMachine(typeof(_003CPlay_003Ec__async0))]
		[DebuggerStepThrough]
		private void Play()
		{
			_003CPlay_003Ec__async0 stateMachine = default(_003CPlay_003Ec__async0);
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}
	}
}
