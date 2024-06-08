using System;
using YG;

public class PluginYGBridge : ISDKBridge
{
	public string Language => YandexGame.EnvironmentData.language;

	public event Action<bool> FocusChanged;
	public event Action<RewardedState> RewardedStateChanged;
	public event Action<InterstitialState> InterstitialStateChanged;
	public event Action<string> DataLoaded;
	private RewardedState rewardedState;
	private InterstitialState interstitialState;
	private bool ready = false;

	public void GameReady()
	{
		if (ready) return;

		YandexGame.YSDK_features_LoadingAPI_ready();
		ready = true;
	}

	public void Initialize()
	{
		if (YandexGame.SDKEnabled)
		{
			InvokeDataLoaded();
		}
		else
		{
			YandexGame.GetDataEvent += InvokeDataLoaded;
		}
	}

	private void InvokeDataLoaded()
	{
		DataLoaded?.Invoke(YandexGame.savesData.data);
		SubscribeForEvents();
	}

	private void SubscribeForEvents()
	{
		YandexGame.OpenVideoEvent += OnOpenVideoEvent;
		YandexGame.CloseVideoEvent += OnCloseVideoEvent;
		YandexGame.ErrorVideoEvent += OnErrorVideoEvent;
		YandexGame.RewardVideoEvent += OnRewardVideoEvent;
		YandexGame.CloseFullAdEvent += OnCloseFullAdEvent;
		YandexGame.ErrorFullAdEvent += OnErrorFullAdEvent;
		YandexGame.OpenFullAdEvent += OnOpenFullAdEvent;
 	}

	private void OnOpenFullAdEvent()
	{
		SetInterstitialState(InterstitialState.Opened);
	}

	private void OnErrorFullAdEvent()
	{
		SetInterstitialState(InterstitialState.Failed);
	}

	private void OnCloseFullAdEvent()
	{
		SetInterstitialState(InterstitialState.Closed);
	}

	private void OnRewardVideoEvent(int id)
	{
		SetRewardedState(RewardedState.Rewarded);
	}

	private void OnErrorVideoEvent()
	{
		SetRewardedState(RewardedState.Failed);
	}

	private void OnCloseVideoEvent()
	{
		SetRewardedState(RewardedState.Closed);
	}

	private void OnOpenVideoEvent()
	{
		SetRewardedState(RewardedState.Opened);
	}

	private void SetRewardedState(RewardedState rewardedState)
	{
		if (this.rewardedState == rewardedState) return;

		this.rewardedState = rewardedState;

		RewardedStateChanged?.Invoke(this.rewardedState);
	}

	private void SetInterstitialState(InterstitialState interstitialState)
	{
		if (this.interstitialState == interstitialState) return;

		this.interstitialState = interstitialState;

		InterstitialStateChanged?.Invoke(this.interstitialState);
	}

	public void LoadData(string key)
	{
		if (YandexGame.savesData.data != null)
		{
			DataLoaded?.Invoke(YandexGame.savesData.data);
		}
		else
		{
			YandexGame.GetDataEvent += () => DataLoaded?.Invoke(YandexGame.savesData.data);
		}
	}

	public void SaveData(string key, string data)
	{
		YandexGame.savesData.data = data;
		YandexGame.SaveProgress();
	}

	public void ShowInterstitial()
	{
		//YandexGame.FullscreenShow();
	}

	public void ShowRewarded()
	{
		YandexGame.RewVideoShow(0);
	}

	public void ShowReview()
	{
		YandexGame.ReviewShow(false);
	}

	public void SetLeaderboradScore(string leaderboardName,  int score)
	{
		YandexGame.NewLeaderboardScores(leaderboardName, score);
	}

	public void OpenYandexDeveloperPage()
	{
		string url = $"https://yandex.{YandexGame.EnvironmentData.domain}/games/developer?name=VarCo";
		YandexGame.OnURL(url);
	}
}
