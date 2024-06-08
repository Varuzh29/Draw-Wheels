using System;
using UnityEngine;

public class DebugSDKBridge : ISDKBridge
{
	public string Language => GameManager.Config.InEditorLanguage;

	public event Action<bool> FocusChanged;
	public event Action<RewardedState> RewardedStateChanged;
	public event Action<InterstitialState> InterstitialStateChanged;
	public event Action<string> DataLoaded;

	public void GameReady()
	{
		//throw new NotImplementedException();
	}

	public void Initialize()
	{
		//throw new NotImplementedException();
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
		//throw new NotImplementedException();
	}

	public void ShowInterstitial()
	{
		//throw new NotImplementedException();
	}

	public void ShowReview()
	{
		//throw new NotImplementedException();
	}

	public void ShowRewarded()
	{
		//throw new NotImplementedException();
	}
}
