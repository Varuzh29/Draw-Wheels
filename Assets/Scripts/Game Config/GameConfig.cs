using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu]
public class GameConfig : ScriptableObject
{
    [SerializeField] private bool showDebugMessages = true; public bool ShowDebugMessages => showDebugMessages;
    [Header("Line Settings:")]
    [SerializeField, Tooltip("Maximum points count in line renderers")] 
    private int maxPointsCount = 1000; public int MaxPointsCount => maxPointsCount;
    [SerializeField, Min(0.1f), Tooltip("Minimal distance between two line segments")] 
    private float lineStep = 0.1f; public float LineStep => lineStep;
    [Header("Time Settings:")]
    [SerializeField] private float slowMotionTimeScale = 0.1f; public float SlowMotionTimeScale => slowMotionTimeScale;
    [Header("Level Settings:")]
    [SerializeField, Min(1)] private int enemysMinimalLevel = 2; public int EnemysMinimalLevel => enemysMinimalLevel;
    [SerializeField, Min(1)] private int segmentsCount = 10; public int SegmentsCount => segmentsCount;
    [SerializeField] private LevelSegmentsSet levelSegmentsSet; public LevelSegmentsSet LevelSegmentsSet => levelSegmentsSet;
	[SerializeField] private LevelSegment startSegment; public LevelSegment StartSegment => startSegment;
	[SerializeField] private LevelSegment endSegment; public LevelSegment EndSegment => endSegment;
	[Header("Base Numbers:")]
    [SerializeField] private float torqueForce; public float TorqueForce => torqueForce;
    [Space]
	//
	[Header("Price list:")]
	[SerializeField, Min(0)] private int speedPriceStep = 200; public int SpeedPriceStep => speedPriceStep;
	[SerializeField, Min(0)] private int incomePriceStep = 200; public int IncomePriceStep => incomePriceStep;
	[SerializeField, Min(0)] private int unlockPriceStep = 200; public int UnlockPriceStep => unlockPriceStep;
	[SerializeField, Min(0)] private int giftCoinsAmount = 100; public int GiftCoinsAmount => giftCoinsAmount;
	//
	[Header("Player Data:")]
    [SerializeField] private bool loadLevelFromPlayerData; public bool LoadLevelFromPlayerData => loadLevelFromPlayerData;
	[SerializeField, Min(0)] private int initialMoneyAmount = 100; public int InitialMoneyAmount => initialMoneyAmount;
	//
	[Header("SDK Settings:")]
    //
    [SerializeField, Tooltip("When TRUE shows interstitial ad on every scene loaded event")] 
    private bool showInterstitialOnSceneLoaded = true; 
    public bool ShowInterstitialOnSceneLoaded => showInterstitialOnSceneLoaded;
	//
	[SerializeField] private string storageKey = "key0"; 
    public string StorageKey => storageKey;
    //
    [SerializeField, Tooltip("Show review prompt on every X levels")] 
    private int reviewPromptLevel = 10;
    public int ReviewPromptLevel => reviewPromptLevel;
    //
    [Header("Collections:")]
    [SerializeField] private StringCollection nicknames; public StringCollection Nicknames => nicknames;
    [SerializeField] private SpriteCollection flags; public SpriteCollection Flags => flags;
    [SerializeField] private ColorSchemeCollection colorSchemes; public ColorSchemeCollection ColorSchemes => colorSchemes;
    [SerializeField] private SkinsCollection skins; public SkinsCollection Skins => skins;
    [SerializeField] private int[] initialSkins; public int[] InitialSkins => initialSkins;
    //
	[Header("Daily Reward System:")]
	[SerializeField] private TimeSpanInspector cooldown; public TimeSpanInspector Cooldown => cooldown;
    [SerializeField] private TimeSpanInspector expiresIn; public TimeSpanInspector ExpiresIn => expiresIn;
    [SerializeField] private int maxStreak = 6; public int MaxStreak => maxStreak;
    [SerializeField] private int dailyRewardAmountStep = 200; public int DailyRewardAmountStep => dailyRewardAmountStep;
    [Header("Localization:")]
    [SerializeField] private TextAsset CSVFile;
    public string CSVString => CSVFile.text;
    [SerializeField, Tooltip("Whitch language shown when game loaded in editor (ISO 639-1)")]
    private string inEditorLanguage = "en";  
    public string InEditorLanguage => inEditorLanguage;

	[Button]
    public void SetTexts()
    {
        TMPLocString[] TMPLocalizations = FindObjectsOfType<TMPLocString>();
        foreach (var TMPLocalization in TMPLocalizations)
        {
            TMPLocalization.SetText();
        }
    }

    [Button]
    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs Cleared");
    }
}
