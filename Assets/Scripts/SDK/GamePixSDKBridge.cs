using System;
using GamePix;
using UnityEngine;

public class GamePixSDKBridge : ISDKBridge
{
    public string Language
    {
        get
        {
            string lang = "en";
            switch (Gpx.CurrentLanguage)
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
    private static event Action RewardAdSuccess;
    private static event Action RewardAdFail;
    private static event Action InterstitialAdSuccess;
    private static event Action InterstitialAdFail;

    public void GameReady()
    {
        //
    }

    public void Initialize()
    {
        InterstitialAdSuccess += () =>
        {
            InterstitialStateChanged.Invoke(InterstitialState.Opened);
            InterstitialStateChanged.Invoke(InterstitialState.Closed);
        };
        InterstitialAdFail += () =>
        {
            InterstitialStateChanged.Invoke(InterstitialState.Failed);
            InterstitialStateChanged.Invoke(InterstitialState.Closed);
        };
        RewardAdSuccess += () =>
        {
            RewardedStateChanged.Invoke(RewardedState.Rewarded);
            RewardedStateChanged.Invoke(RewardedState.Closed);
        };
        RewardAdFail += () =>
        {
            RewardedStateChanged.Invoke(RewardedState.Failed);
            RewardedStateChanged.Invoke(RewardedState.Closed);
        };
        LoadData(GameManager.Config.StorageKey);
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
        InterstitialStateChanged.Invoke(InterstitialState.Loading);
        Gpx.Ads.InterstitialAd(OnInterstitialAdSuccess, OnInterstitialAdFail);
    }

    public void ShowReview()
    {
        //
    }

    public void ShowRewarded()
    {
        RewardedStateChanged.Invoke(RewardedState.Loading);
        RewardedStateChanged.Invoke(RewardedState.Opened);
        Gpx.Ads.RewardAd(OnRewardAdSuccess, OnRewardAdFail);
    }

    [AOT.MonoPInvokeCallback(typeof(Gpx.gpxCallback))]
    public static void OnRewardAdSuccess()
    {
        RewardAdSuccess.Invoke();
    }

    [AOT.MonoPInvokeCallback(typeof(Gpx.gpxCallback))]
    public static void OnRewardAdFail()
    {
        RewardAdFail.Invoke();
    }

    [AOT.MonoPInvokeCallback(typeof(Gpx.gpxCallback))]
    public static void OnInterstitialAdSuccess()
    {
        InterstitialAdSuccess.Invoke();
    }

    [AOT.MonoPInvokeCallback(typeof(Gpx.gpxCallback))]
    public static void OnInterstitialAdFail()
    {
        InterstitialAdFail.Invoke();
    }

}
