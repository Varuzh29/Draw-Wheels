using UnityEngine;
using VarCo;

public class LosePage : MonoBehaviour
{
	[SerializeField] private UIButton retryButton;
	[SerializeField] private UIButton skipButton;

	private void OnEnable()
	{
		retryButton.OnClick.AddListener(OnRetryButtonClick);
		skipButton.OnClick.AddListener(OnSkipButtonClick);
		GameManager.SDK.RewardedStateChanged += OnRewardedStateChanged;
	}

	private void OnRewardedStateChanged(RewardedState rewardedState)
	{
		switch (rewardedState)
		{
			case RewardedState.Loading:
				break;
			case RewardedState.Opened:
				break;
			case RewardedState.Rewarded:
				GameManager.SkipLevel();
				break;
			case RewardedState.Closed:
				SceneFader.ReloadScene();
				break;
			case RewardedState.Failed:
				break;
			default:
				break;
		}
	}

	private void DisableButtons()
	{
		retryButton.Interactable = false;
		skipButton.Interactable = false;
	}

	private void OnRetryButtonClick()
	{
		DisableButtons();
		SceneFader.ReloadScene();
	}

	private void OnSkipButtonClick()
	{
		DisableButtons();
		GameManager.SDK.ShowRewarded();
	}

	private void OnDisable()
	{
		retryButton.OnClick.RemoveListener(OnRetryButtonClick);
		skipButton.OnClick.RemoveListener(OnSkipButtonClick);
		GameManager.SDK.RewardedStateChanged -= OnRewardedStateChanged;
	}
}
