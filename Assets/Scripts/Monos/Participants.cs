using DG.Tweening;
using UnityEngine;

public class Participants : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform enemy;
    [SerializeField] private Transform crown;
    [SerializeField] private float yOffset;
    [SerializeField] private float duration;
    private Vector3 crownTargetPos;
    private bool playerLeading = true;
    private float lerp = 1;
    private Tween tween;

	private void Awake()
	{
		GameManager.GameStateChanged += OnGameStateChanged;
	}

	private void OnGameStateChanged(GameState gameState)
	{
		enabled = gameState == GameState.Race;
	}

	private void OnEnable()
	{
		GameManager.EventBus.SomeOneFinished.AddListener(OnSomeOneFinished);
		crown.DOKill();
		crown.gameObject.SetActive(true);
		crown.DOScale(1, 0.5f).From(0);
	}

	private void OnSomeOneFinished()
	{
		if (IsPlayerLeading())
		{
			GameManager.Win();
		}
		else
		{
			GameManager.Lose();
		}
		GameManager.EventBus.SomeOneFinished.RemoveListener(OnSomeOneFinished);
		enabled = false;
	}

	private void Start()
	{
		crownTargetPos = player.position;
        InvokeRepeating(nameof(Tic), 0, 0.5f);
	}

	public bool IsPlayerLeading()
    {
        return player.position.x >= enemy.position.x;
    }

    private void Tic()
    {
		if (IsPlayerLeading() != playerLeading)
		{
			playerLeading = IsPlayerLeading();
			LeaderChanged();
		}
	}

	private void Update()
	{
		crownTargetPos = playerLeading ? player.position : enemy.position;
		crown.position = Vector3.Lerp(crown.position, crownTargetPos + Vector3.up * yOffset, lerp);
	}

    private void LeaderChanged()
    {
        tween?.Kill();
        tween = DOTween.To(() => lerp, x => lerp = x, 1, duration).From(0);
	}

	private void OnDisable()
	{
		CancelInvoke();
		crown?.DOKill();
		crown?.DOScale(0, 0.5f).From(1).OnComplete(() =>
		{
			crown?.gameObject.SetActive(false);
		});
	}

	private void OnDestroy()
	{
		GameManager.GameStateChanged -= OnGameStateChanged;
	}
}
