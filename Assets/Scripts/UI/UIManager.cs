using DG.Tweening;
using UnityEngine;
using VarCo;

public class UIManager : MonoBehaviour
{
	[SerializeField] private float fadeDuration;
	[SerializeField] private UIPage startPage;
	[SerializeField] private UIPage racePage;
	[SerializeField] private UIPage winPage;
	[SerializeField] private UIPage losePage;
	[SerializeField] private UIPage shopPage;
	[SerializeField] private UIPage searchPage;
	[SerializeField] private float finishDelay;
	private UIPage currentPage;
	private Sequence timer;

	private void OnEnable()
	{
		OnGameStateChanged(GameManager.GameState);
		GameManager.GameStateChanged += OnGameStateChanged;
	}

	private void OnGameStateChanged(GameState gameState)
	{
		currentPage?.Hide(fadeDuration);
		currentPage = null;

		switch (gameState)
		{
			case GameState.Initialization:
				break;
			case GameState.Loading:
				break;
			case GameState.OpponentSearch:
				currentPage = searchPage;
				currentPage.Show(fadeDuration);
				break;
			case GameState.Start:
				currentPage = startPage;
				currentPage.Show(fadeDuration);
				break;
			case GameState.Race:
				currentPage = racePage;
				currentPage.Show(fadeDuration);
				break;
			case GameState.Win:
				currentPage = winPage;
				timer?.Kill();
				timer = DOTimer.Set(finishDelay, () => currentPage.Show(fadeDuration));
				break;
			case GameState.Lose:
				currentPage = losePage;
				timer?.Kill();
				timer = DOTimer.Set(finishDelay, () => currentPage.Show(fadeDuration));
				break;
			case GameState.Shop:
				currentPage = shopPage;
				currentPage.Show(fadeDuration);
				break;
			default:
				break;
		}
	}

	private void OnDisable()
	{
		GameManager.GameStateChanged -= OnGameStateChanged;
		timer.Kill();
	}
}
