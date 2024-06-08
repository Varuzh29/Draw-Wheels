using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using VarCo;
using System.Collections.Generic;
public enum DebugMessageType
{
	Normal,
	Warning,
	Error
}

public enum GameState
{
	Initialization,
	Loading,
	Start,
	OpponentSearch,
	Race,
	Win,
	Lose,
	Shop,
	Restart,
	Pause,
}

public static class GameManager
{
	public static ISDKBridge SDK { get; private set; }
	private static GameConfig config;
	public static GameConfig Config
	{
		get
		{
			if (config != null)
			{
				return config;
			}
			else
			{
				config = Resources.Load<GameConfig>("Game Config");
				if (config == null)
				{
					config = ScriptableObject.CreateInstance<GameConfig>();
					DebugMessage("Can't find Game Config asset in Resources folder! New Game Config instance was created with default properties", DebugMessageType.Warning);
				}
				return config;
			}
		}
	}
	public static string Language
	{
		get
		{
			if (Application.isEditor)
			{
				return Config.InEditorLanguage;
			}
			else
			{
				return SDK.Language;
			}
		}
	}
	public static bool Ready { get; private set; }
	private static LoadingUI loadingUI;
	public static LoadingUI LoadingUI
	{
		get
		{
			if (loadingUI == null)
			{
				loadingUI = Resources.Load<LoadingUI>("[LoadingUI]");
				UnityEngine.Object.DontDestroyOnLoad(loadingUI.gameObject);
			}
			return loadingUI;
		}
	}
	private static Storage Storage;
	public static StorageReader StorageReader { get; private set; }
	public static PlayerWallet PlayerWallet { get; private set; }
	public static LevelReward LevelReward { get; private set; }
	public static DailyReward DailyReward { get; private set; }
	private static Action rewardAction;
	public static PlayerBoosters PlayerBoosters { get; private set; }
	public static EventBus EventBus { get; private set; }
	private static Dictionary<string, Dictionary<string, string>> localizedStrings;
	public static Dictionary<string, Dictionary<string, string>> LocalizedStrings
	{
		get
		{
			if (localizedStrings == null)
			{
				localizedStrings = CSVParser.Parse(',', '\n', Config.CSVString);
			}
			return localizedStrings;
		}
	}
	public static ColorScheme ColorScheme { get; set; }
	public static GameState GameState { get; private set; }
	public static event Action<GameState> GameStateChanged;
	public static event Action<int> CurrentSkinChanged;
	//
	public static void DebugMessage(object message, DebugMessageType messageType = DebugMessageType.Normal)
	{
		if (Config.ShowDebugMessages == false) return;

		switch (messageType)
		{
			case DebugMessageType.Normal:
				Debug.Log(message);
				break;
			case DebugMessageType.Warning:
				Debug.LogWarning(message);
				break;
			case DebugMessageType.Error:
				Debug.LogError(message);
				break;
		}
	}

	public static void GetReward(Action rewardAction)
	{
		GameManager.rewardAction = rewardAction;
		SDK.ShowRewarded();
	}

	public static void Init()
	{
		SDK = new GameDistributionSDKBridge();
		SDK.DataLoaded += OnDataLoaded;
		SDK.FocusChanged += OnFocusChanged;
		SDK.RewardedStateChanged += OnRewardedStateChanged;
		SDK.InterstitialStateChanged += OnInterstitialStateChanged;
		SDK.Initialize();

		SceneManager.sceneLoaded += OnSceneLoaded;
		SceneManager.sceneUnloaded += OnSceneUnloaded;
		Application.quitting += OnQuitting;
		DebugMessage("Game Manager Initialized \n" +
					$"Language: {Language} \n" +
					$"Game state is: {GameState}");
	}

	public static void ToggleAudio()
	{
		Storage.Audio = !Storage.Audio;
		AudioListener.volume = Storage.Audio ? 1 : 0;
	}

	public static void RestartRace()
	{
		SetGameState(GameState.Restart);
		SceneFader.ReloadScene();
	}

	public static void SaveDailyRewardState()
	{
		Storage.LastClaimTimeTicks = DailyReward.LastClaimTime.Ticks;
		Storage.LastStreak = DailyReward.Streak >= Config.MaxStreak ? 0 : DailyReward.Streak;
		DailyReward = new DailyReward(new DateTime(Storage.LastClaimTimeTicks), Storage.LastStreak, Config.Cooldown.ToTimeSpan(), Config.ExpiresIn.ToTimeSpan());
	}

	private static void OnSceneUnloaded(Scene scene)
	{
		SetGameState(GameState.Loading);
	}

	private static void OnFocusChanged(bool focus)
	{
		if (focus)
		{
			AudioListener.volume = Storage.Audio ? 1 : 0;
		}
		else
		{
			AudioListener.volume = 0;
		}
		DebugMessage($"Game focus changed. Current state: {focus}");
	}

	private static void OnDataLoaded(string data)
	{
		Storage = new Storage(data);
		EventBus = new EventBus();
		StorageReader = new StorageReader(Storage);
		PlayerWallet = new PlayerWallet(Storage);
		PlayerBoosters = new PlayerBoosters(PlayerWallet, Storage);
		LevelReward = new LevelReward(PlayerWallet);
		DailyReward = new DailyReward(new DateTime(Storage.LastClaimTimeTicks), Storage.LastStreak, Config.Cooldown.ToTimeSpan(), Config.ExpiresIn.ToTimeSpan());
		SceneFader.LoadScene(1);
	}

	public static void Win()
	{
		SetGameState(GameState.Win);
		Storage.Level++;
	}

	public static void Lose()
	{
		SetGameState(GameState.Lose);
	}

	public static void StartOpponentSearch()
	{
		SetGameState(GameState.OpponentSearch);
	}

	public static void OpenShop()
	{
		if (GameState == GameState.Start)
		{
			SetGameState(GameState.Shop);
		}
	}

	public static void CloseShop()
	{
		if (GameState == GameState.Shop)
		{
			SetGameState(GameState.Start);
		}
	}

	public static void StartRace()
	{
		SetGameState(GameState.Race);
	}

	public static void SetSkin(int skinId)
	{
		if (Storage.CurrentSkin == skinId) return;

		Storage.CurrentSkin = skinId;
		CurrentSkinChanged?.Invoke(Storage.CurrentSkin);
	}

	public static void UnlockSkin(int skinId)
	{
		List<int> availableSkins = new List<int>();
		availableSkins.AddRange(Storage.AvailableSkins);
		availableSkins.Add(skinId);
		Storage.AvailableSkins = availableSkins.ToArray();
	}

	private static void OnRewardedStateChanged(RewardedState rewardedState)
	{
		switch (rewardedState)
		{
			case RewardedState.Loading:
				LoadingUI.Show();
				break;
			case RewardedState.Opened:
				AudioListener.volume = 0;
				break;
			case RewardedState.Rewarded:
				rewardAction?.Invoke();
				break;
			case RewardedState.Closed:
				AudioListener.volume = Storage.Audio ? 1 : 0;
				LoadingUI.Hide();
				break;
			case RewardedState.Failed:
				AudioListener.volume = Storage.Audio ? 1 : 0;
				LoadingUI.Hide();
				break;
		}

		DebugMessage($"Rewarded state changed. Current state: {rewardedState}");
	}

	private static void OnInterstitialStateChanged(InterstitialState interstitialState)
	{
		switch (interstitialState)
		{
			case InterstitialState.Loading:
				LoadingUI.Show();
				break;
			case InterstitialState.Opened:
				AudioListener.volume = 0;
				break;
			case InterstitialState.Closed:
				AudioListener.volume = Storage.Audio ? 1 : 0;
				LoadingUI.Hide();
				break;
			case InterstitialState.Failed:
				AudioListener.volume = Storage.Audio ? 1 : 0;
				LoadingUI.Hide();
				break;
		}

		DebugMessage($"Interstitial state changed. Current state: {interstitialState}");
	}

	private static void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
	{
		DebugMessage($"Scene loaded: {scene.name}");

		if (Storage.Level % Config.ReviewPromptLevel == 0)
		{
			DebugMessage($"REVIEW PROMPT");
		}

		if (!Ready && scene.buildIndex != 0)
		{
			Ready = true;
			OnGameIsReady();
		}

		if (Config.ShowInterstitialOnSceneLoaded && scene.buildIndex == 1)
		{
			DebugMessage("Show Interstitial");
			SDK.ShowInterstitial();
		}

		if (scene.buildIndex > 0)
		{
			LevelReward?.Reset();
			SetGameState(GameState.Start);
			SDK.SetLeaderboradScore("LB", Storage.Level);
		}
	}

	private static void OnGameIsReady()
	{
		DebugMessage("Game is ready");
		SDK.GameReady();
	}

	private static void OnQuitting()
	{
		DebugMessage($"Application quitting");

		SceneManager.sceneLoaded -= OnSceneLoaded;
		SceneManager.sceneUnloaded -= OnSceneUnloaded;
		SDK.FocusChanged -= OnFocusChanged;
		SDK.RewardedStateChanged -= OnRewardedStateChanged;
		SDK.InterstitialStateChanged -= OnInterstitialStateChanged;
		Application.quitting -= OnQuitting;
	}

	public static void SkipLevel()
	{
		Storage.Level++;
	}

	private static void SetGameState(GameState gameState)
	{
		if (GameState == gameState) return;
		if (GameState == GameState.Lose && gameState == GameState.Win) return;
		if (GameState == GameState.Win && gameState == GameState.Lose) return;
		if (GameState == GameState.Restart && gameState == GameState.Win) return;
		if (GameState == GameState.Restart && gameState == GameState.Lose) return;

		GameState = gameState;

		GameStateChanged?.Invoke(GameState);
		DebugMessage($"Game state set to: {GameState}");
	}
}