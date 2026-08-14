using System;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.UI
{
	public abstract class PagingScrollView<TV, TM> : ScrollView<TV, TM> where TV : View
	{
		[SerializeField]
		private Text _countText;

		[SerializeField]
		private GameObject _progressIndicator;

		[SerializeField]
		private GameObject _errorView;

		[SerializeField]
		private Button _retryButton;

		protected int Page;

		protected int PageSize;

		private Vector2 _value;

		private bool _isLoading;

		private bool _fullyLoaded;

		private bool _requestFailed;

		protected IAsyncDataProvider<TM> AsyncDataProvider
		{
			get;
			private set;
		}

		public bool IsLoadEnabled
		{
			get;
			set;
		} = true;


		protected override void Init()
		{
			base.Init();
			HideProgress();
			_retryButton.onClick.AddListener(LoadPage);
		}

		public virtual void Init(IAsyncDataProvider<TM> dataProvider, int pageSize)
		{
			AsyncDataProvider = dataProvider;
			PageSize = pageSize;
			Init();
		}

		public virtual void Reload()
		{
			Page = 0;
			_fullyLoaded = false;
			HideError();
			HideProgress();
			SetContent(new TM[0]);
			LoadPage();
		}

		protected override void OnValueChanged(int diff)
		{
			base.OnValueChanged(diff);
			if (!_isLoading && !_requestFailed && Mathf.Approximately(GetBottomSpringHeight(), 0f))
			{
				LoadPage();
			}
		}

		private void LoadPage()
		{
			if (IsLoadEnabled && !_fullyLoaded)
			{
				HideError();
				ShowProgress();
				_isLoading = true;
				_requestFailed = false;
				AsyncDataProvider.LoadData(Page, PageSize, LoadSuccess, LoadFailed);
			}
		}

		private void LoadSuccess(TM[] data)
		{
			_isLoading = false;
			HideProgress();
			ReplaceContent(data, Page * PageSize, PageSize);
			if (data.Length == PageSize)
			{
				Page++;
			}
			else
			{
				_fullyLoaded = true;
			}
		}

		private void LoadFailed(Exception ex)
		{
			_isLoading = false;
			_requestFailed = true;
			HideProgress();
			if (!(ex is OperationCanceledException))
			{
				ShowError();
			}
		}

		private void ShowProgress()
		{
			base.EmptyView.gameObject.SetActive(value: false);
			_progressIndicator.transform.SetAsLastSibling();
			_progressIndicator.SetActive(value: true);
		}

		private void HideProgress()
		{
			_progressIndicator.transform.SetAsFirstSibling();
			_progressIndicator.SetActive(value: false);
		}

		private void ShowError()
		{
			_errorView.gameObject.SetActive(value: true);
			_errorView.transform.SetAsLastSibling();
		}

		private void HideError()
		{
			_errorView.gameObject.SetActive(value: false);
			_errorView.transform.SetAsFirstSibling();
		}
	}
}
