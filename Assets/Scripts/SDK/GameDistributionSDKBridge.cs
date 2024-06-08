using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDistributionSDKBridge : ISDKBridge
{
    private UnityLanguageProvider unityLanguageProvider = new UnityLanguageProvider("en");
    public string Language => unityLanguageProvider.GetLanguage();

    public event Action<bool> FocusChanged;
    public event Action<RewardedState> RewardedStateChanged;
    public event Action<InterstitialState> InterstitialStateChanged;
    public event Action<string> DataLoaded;

    public void GameReady()
    {
        //
    }

    public void Initialize()
    {
        GameDistribution.OnPauseGame += OnPauseGame;
        GameDistribution.OnRewardGame += OnRewardGame;
        GameDistribution.OnResumeGame += OnResumeGame;
        GameDistribution.OnRewardedVideoSuccess += OnRewardedVideoSuccess;
        GameDistribution.OnRewardedVideoFailure += OnRewardedVideoFailure;
        LoadData(GameManager.Config.StorageKey);
        GameDistribution.Instance.PreloadRewardedAd();
    }

    private void OnRewardedVideoFailure()
    {
        RewardedStateChanged?.Invoke(RewardedState.Failed);
    }

    private void OnRewardedVideoSuccess()
    {
        RewardedStateChanged?.Invoke(RewardedState.Opened);
    }

    private void OnResumeGame()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    private void OnRewardGame()
    {
        RewardedStateChanged?.Invoke(RewardedState.Rewarded);
        RewardedStateChanged?.Invoke(RewardedState.Closed);
        GameDistribution.Instance.PreloadRewardedAd();
    }

    private void OnPauseGame()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
    }

    public void LoadData(string key)
    {
        string data = PlayerPrefs.GetString(key);
        DataLoaded?.Invoke(data);
    }

    public void SaveData(string key, string data)
    {
        PlayerPrefs.SetString(key, data);
    }

    public void SetLeaderboradScore(string leaderboardName, int score)
    {
        //
    }

    public void ShowInterstitial()
    {
        GameDistribution.Instance.ShowAd();
    }

    public void ShowReview()
    {
        //
    }

    public void ShowRewarded()
    {
        GameDistribution.Instance.ShowRewardedAd();
    }
}
