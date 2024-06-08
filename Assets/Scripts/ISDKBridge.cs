using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RewardedState
{
    Loading,
    Opened,
	Rewarded,
	Closed,
	Failed
}

public enum InterstitialState
{
	Loading,
	Opened,
	Closed,
	Failed
}

public interface ISDKBridge
{
    public string Language { get; }
	public event Action<bool> FocusChanged;
    public event Action<RewardedState> RewardedStateChanged;
    public event Action<InterstitialState> InterstitialStateChanged;
	public event Action<string> DataLoaded;

	public void Initialize();
	public void ShowRewarded();
	public void ShowInterstitial();
	public void GameReady();
	public void LoadData(string key);
	public void SaveData(string key, string data);
	public void ShowReview();
	public void SetLeaderboradScore(string leaderboardName, int score);
}
