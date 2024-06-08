using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMonetizeSDKBridge : ISDKBridge
{
    public string Language
    {
        get
        {
            string lang = "en";
            switch (Application.systemLanguage)
            {
                case SystemLanguage.Russian:
                    lang = "ru";
                    break;
                case SystemLanguage.Portuguese:
                    lang = "pt";
                    break;
                case SystemLanguage.Spanish:
                    lang = "es";
                    break;
                case SystemLanguage.German:
                    lang = "de";
                    break;
                case SystemLanguage.French:
                    lang = "fr";
                    break;
                case SystemLanguage.Italian:
                    lang = "it";
                    break;
            }
            return lang;
        }
    }

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
        GameMonetize.OnResumeGame += ResumeGame;
        GameMonetize.OnPauseGame += PauseGame;
        LoadData(GameManager.Config.StorageKey);
    }

    private void ResumeGame()
    {
        InterstitialStateChanged.Invoke(InterstitialState.Closed);
    }

    private void PauseGame()
    {
        InterstitialStateChanged.Invoke(InterstitialState.Opened);
    }

    public void LoadData(string key)
    {
        string loadedData = string.Empty;
        if (PlayerPrefs.HasKey(key))
        {
            loadedData = PlayerPrefs.GetString(key);
        }
        DataLoaded.Invoke(loadedData);
    }

    public void SaveData(string key, string data)
    {
        PlayerPrefs.SetString(key, data);
        PlayerPrefs.Save();
    }

    public void SetLeaderboradScore(string leaderboardName, int score)
    {
        //
    }

    public void ShowInterstitial()
    {
        GameMonetize.Instance.ShowAd();
    }

    public void ShowReview()
    {
        //
    }

    public void ShowRewarded()
    {
        RewardedStateChanged.Invoke(RewardedState.Loading);
        GameMonetize.Instance.ShowAd();
        RewardedStateChanged.Invoke(RewardedState.Rewarded);
    }
}
