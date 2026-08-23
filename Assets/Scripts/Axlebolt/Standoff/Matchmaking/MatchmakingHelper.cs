using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Axlebolt.Standoff.Analytics;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Matchmaking
{
	public class MatchmakingHelper : MonoBehaviour
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CFind_003Ec__async0 : IAsyncStateMachine
		{
			private sealed class _003CFind_003Ec__AnonStorey3
			{
				internal CancellationTokenSource cancellationTokenSource;

				internal _003CFind_003Ec__async0 _003C_003Ef__ref_00240;

				internal void _003C_003Em__0()
				{
					cancellationTokenSource.Cancel();
				}

				internal void _003C_003Em__1(PhotonServer location, int online, int ping)
				{
					_003C_003Ef__ref_00240._0024this._searchGameView.Online = 2 * online;
					_003C_003Ef__ref_00240._0024this._searchGameView.Ping = ping;
					_003C_003Ef__ref_00240._0024this._searchGameView.Region = location.GetDisplayName();
				}
			}

			internal MatchmakingFilter filter;

			internal PhotonServer _003Cregion_003E__0;

			internal CancellationTokenSource cancellationTokenSource;

			internal MatchmakingHelper _0024this;

			internal AsyncTaskMethodBuilder<MatchmakingResult> _0024builder;

			internal int _0024PC;

			private _003CFind_003Ec__AnonStorey3 _0024locvar0;

			private TaskAwaiter<PhotonServer> _0024awaiter0;

			private TaskAwaiter<MatchmakingResult> _0024awaiter1;

			public void MoveNext()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				bool flag = false;
				MatchmakingResult result;
				try
				{
					switch (num)
					{
						default:
							return;
						case 0u:
							_0024locvar0 = new _003CFind_003Ec__AnonStorey3();
							_0024locvar0._003C_003Ef__ref_00240 = this;
							_0024locvar0.cancellationTokenSource = cancellationTokenSource;
							if (filter == null)
							{
								throw new ArgumentNullException("filter");
							}
							_0024awaiter0 = _0024this.GetRegion().GetAwaiter();
							if (!_0024awaiter0.IsCompleted)
							{
								_0024PC = 1;
								flag = true;
								_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
								return;
							}
							goto case 1u;
						case 1u:
							_003Cregion_003E__0 = _0024awaiter0.GetResult();
							if (_0024locvar0.cancellationTokenSource == null)
							{
								_0024locvar0.cancellationTokenSource = new CancellationTokenSource();
							}
							_003CFind_003Ec__AnonStorey3 _024locvar0 = _0024locvar0;
							_0024this._searchGameView.CancelHandler = delegate
							{
								_024locvar0.cancellationTokenSource.Cancel();
							};
							num = 4294967293u;
							break;
						case 2u:
							break;
					}
					try
					{
						switch (num)
						{
							default:
								_0024this._searchGameView.Show();
								_003CFind_003Ec__AnonStorey3 _024locvar0 = _0024locvar0;
								_0024awaiter1 = MatchmakingManager.Find(_003Cregion_003E__0.Location, filter, delegate (PhotonServer location, int online, int ping)
								{
									_024locvar0._003C_003Ef__ref_00240._0024this._searchGameView.Online = 2 * online;
									_024locvar0._003C_003Ef__ref_00240._0024this._searchGameView.Ping = ping;
									_024locvar0._003C_003Ef__ref_00240._0024this._searchGameView.Region = location.GetDisplayName();
								}, _0024locvar0.cancellationTokenSource.Token).GetAwaiter();
								if (!_0024awaiter1.IsCompleted)
								{
									_0024PC = 2;
									flag = true;
									_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter1, ref this);
									return;
								}
								break;
							case 2u:
								break;
						}
						result = _0024awaiter1.GetResult();
					}
					catch (Exception exception)
					{
						_0024this._searchGameView.CancelHandler = null;
						_0024this._searchGameView.Hide();
						AnalyticsManager.PhotonMatchmakingError(exception);
						throw;
					}
				}
				catch (Exception exception2)
				{
					_0024PC = -1;
					_0024builder.SetException(exception2);
					return;
				}
				_0024PC = -1;
				_0024builder.SetResult(result);
			}

			[DebuggerHidden]
			public void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_0024builder.SetStateMachine(stateMachine);
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CCreate_003Ec__async1 : IAsyncStateMachine
		{
			private sealed class _003CCreate_003Ec__AnonStorey4
			{
				internal CancellationTokenSource cancellationTokenSource;

				internal _003CCreate_003Ec__async1 _003C_003Ef__ref_00241;

				internal void _003C_003Em__0()
				{
					cancellationTokenSource.Cancel();
				}

				internal void _003C_003Em__1(PhotonServer location, int online, int ping)
				{
					_003C_003Ef__ref_00241._0024this._searchGameView.Online = 2 * online;
					_003C_003Ef__ref_00241._0024this._searchGameView.Ping = ping;
					_003C_003Ef__ref_00241._0024this._searchGameView.Region = location.GetDisplayName();
				}
			}

			internal MatchmakingCreateOptions options;

			internal PhotonServer _003Cregion_003E__0;

			internal CancellationTokenSource cancellationTokenSource;

			internal MatchmakingHelper _0024this;

			internal AsyncTaskMethodBuilder<MatchmakingResult> _0024builder;

			internal int _0024PC;

			private _003CCreate_003Ec__AnonStorey4 _0024locvar0;

			private TaskAwaiter<PhotonServer> _0024awaiter0;

			private TaskAwaiter<MatchmakingResult> _0024awaiter1;

			public void MoveNext()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				bool flag = false;
				MatchmakingResult result;
				try
				{
					switch (num)
					{
						default:
							return;
						case 0u:
							_0024locvar0 = new _003CCreate_003Ec__AnonStorey4();
							_0024locvar0._003C_003Ef__ref_00241 = this;
							_0024locvar0.cancellationTokenSource = cancellationTokenSource;
							if (options == null)
							{
								throw new ArgumentNullException("options");
							}
							_0024awaiter0 = _0024this.GetRegion().GetAwaiter();
							if (!_0024awaiter0.IsCompleted)
							{
								_0024PC = 1;
								flag = true;
								_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
								return;
							}
							goto case 1u;
						case 1u:
							_003Cregion_003E__0 = _0024awaiter0.GetResult();
							if (_0024locvar0.cancellationTokenSource == null)
							{
								_0024locvar0.cancellationTokenSource = new CancellationTokenSource();
							}

							_003CCreate_003Ec__AnonStorey4 _024locvar0 = _0024locvar0;
							_0024this._searchGameView.CancelHandler = delegate
							{
								_024locvar0.cancellationTokenSource.Cancel();
							};
							num = 4294967293u;
							break;
						case 2u:
							break;
					}
					try
					{
						switch (num)
						{
							default:
								_0024this._searchGameView.Show();
								_003CCreate_003Ec__AnonStorey4 _024locvar0 = _0024locvar0;
								_0024awaiter1 = MatchmakingManager.Create(_003Cregion_003E__0.Location, options, delegate (PhotonServer location, int online, int ping)
								{
									_024locvar0._003C_003Ef__ref_00241._0024this._searchGameView.Online = 2 * online;
									_024locvar0._003C_003Ef__ref_00241._0024this._searchGameView.Ping = ping;
									_024locvar0._003C_003Ef__ref_00241._0024this._searchGameView.Region = location.GetDisplayName();
								}, _0024locvar0.cancellationTokenSource.Token).GetAwaiter();
								if (!_0024awaiter1.IsCompleted)
								{
									_0024PC = 2;
									flag = true;
									_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter1, ref this);
									return;
								}
								break;
							case 2u:
								break;
						}
						result = _0024awaiter1.GetResult();
					}
					catch (Exception exception)
					{
						_0024this._searchGameView.CancelHandler = null;
						_0024this._searchGameView.Hide();
						AnalyticsManager.PhotonMatchmakingError(exception);
						throw;
					}
				}
				catch (Exception exception2)
				{
					_0024PC = -1;
					_0024builder.SetException(exception2);
					return;
				}
				_0024PC = -1;
				_0024builder.SetResult(result);
			}

			[DebuggerHidden]
			public void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_0024builder.SetStateMachine(stateMachine);
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CGetRegion_003Ec__async2 : IAsyncStateMachine
		{
			internal PhotonServer _003Cregion_003E__0;

			internal MatchmakingHelper _0024this;

			internal AsyncTaskMethodBuilder<PhotonServer> _0024builder;

			internal int _0024PC;

			private CoroutineAsyncExtensions.CoroutineAsyncBridge _0024awaiter0;

			public void MoveNext()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				PhotonServer server;
				try
				{
					switch (num)
					{
						default:
							return;
						case 0u:
							_003Cregion_003E__0 = Regions.GetCurrentRegion();
							if (_003Cregion_003E__0 == null)
							{
								_0024awaiter0 = _0024this._regionSelectDialog.ShowAndWait().GetAwaiter();
								if (!_0024awaiter0.IsCompleted)
								{
									_0024PC = 1;
									_0024builder.AwaitOnCompleted(ref _0024awaiter0, ref this);
									return;
								}
								break;
							}
							server = _003Cregion_003E__0;
							goto end_IL_000e;
						case 1u:
							break;
					}
					_0024awaiter0.GetResult();
					if (_0024this._regionSelectDialog.Server == null)
					{
						throw new OperationCanceledException();
					}
					server = _0024this._regionSelectDialog.Server;
				end_IL_000e:;
				}
				catch (Exception exception)
				{
					_0024PC = -1;
					_0024builder.SetException(exception);
					return;
				}
				_0024PC = -1;
				_0024builder.SetResult(server);
			}

			[DebuggerHidden]
			public void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_0024builder.SetStateMachine(stateMachine);
			}
		}

		[SerializeField]
		private RegionSelectDialog _regionSelectDialog;

		[SerializeField]
		private SearchGameView _searchGameView;

		private void Awake()
		{
			_regionSelectDialog.SaveSelection = true;
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CFind_003Ec__async0))]
		public Task<MatchmakingResult> Find([NotNull] MatchmakingFilter filter, CancellationTokenSource cancellationTokenSource = null)
		{
			_003CFind_003Ec__async0 stateMachine = default(_003CFind_003Ec__async0);
			stateMachine.filter = filter;
			stateMachine.cancellationTokenSource = cancellationTokenSource;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncTaskMethodBuilder<MatchmakingResult>.Create();
			AsyncTaskMethodBuilder<MatchmakingResult> _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CCreate_003Ec__async1))]
		public Task<MatchmakingResult> Create([NotNull] MatchmakingCreateOptions options, CancellationTokenSource cancellationTokenSource = null)
		{
			_003CCreate_003Ec__async1 stateMachine = default(_003CCreate_003Ec__async1);
			stateMachine.options = options;
			stateMachine.cancellationTokenSource = cancellationTokenSource;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncTaskMethodBuilder<MatchmakingResult>.Create();
			AsyncTaskMethodBuilder<MatchmakingResult> _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		[AsyncStateMachine(typeof(_003CGetRegion_003Ec__async2))]
		[DebuggerStepThrough]
		private Task<PhotonServer> GetRegion()
		{
			_003CGetRegion_003Ec__async2 stateMachine = default(_003CGetRegion_003Ec__async2);
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncTaskMethodBuilder<PhotonServer>.Create();
			AsyncTaskMethodBuilder<PhotonServer> _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}
	}
}
