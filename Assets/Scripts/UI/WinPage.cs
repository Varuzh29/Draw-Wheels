using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VarCo;

public class WinPage : MonoBehaviour
{
	[SerializeField] private NumberTweener moneyCounter;
	[SerializeField] private NumberTweener coinsCollectedNumber;
	[SerializeField] private RewardLine rewardLine;
	[SerializeField] private TextMeshProUGUI getRewardButtonText;
	[SerializeField] private UIButton getRewardButton;
	[SerializeField] private UIButton noThxButton;
	[SerializeField] private CollectCoinsTween collectCoinsTween;
	private bool rewarded = false;
	private int coinsToGive = 0;
	private Sequence timer1;
	private Sequence timer2;

	private void OnEnable()
	{
		rewarded = false;
		UpdateUI();
		rewardLine.RewardChanged += OnRewardChanged;
		getRewardButton.OnClick.AddListener(OnGetRewardButtonClick);
		GameManager.SDK.RewardedStateChanged += OnRewardedStateChanged;
		noThxButton.OnClick.AddListener(OnNoThxButtonClick);
	}

	private void DeactivateButtons()
	{
		noThxButton.Interactable = false;
		getRewardButton.Interactable = false;
	}

	private void OnNoThxButtonClick()
	{
		DeactivateButtons();
		coinsToGive = GameManager.LevelReward.CollectedMoneyAmount;
		rewardLine.Stop();
		rewardLine.gameObject.SetActive(false);
		GameManager.PlayerWallet.AddMoney(coinsToGive);
		ShowRewardAndReload();
	}

	private void OnGetRewardButtonClick()
	{
		DeactivateButtons();
		rewardLine.Stop();
		rewardLine.gameObject.SetActive(false);
		GameManager.SDK.ShowRewarded();
	}

	private void GiveReward()
	{
		coinsToGive = GameManager.LevelReward.CollectedMoneyAmount * rewardLine.CurrentMultiplier;
		GameManager.PlayerWallet.AddMoney(coinsToGive);
	}

	private void ShowRewardAndReload()
	{
		collectCoinsTween.Play();
		timer1?.Kill();
		timer1 = DOTimer.Set(1.5f, () => moneyCounter.SetNumber(GameManager.StorageReader.Money));
		timer2?.Kill();
		timer2 = DOTimer.Set(2f, () =>
		{
			SceneFader.ReloadScene();
		});
	}

	private void Reload()
	{
		SceneFader.ReloadScene();
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
				rewarded = true;
				GiveReward();
				break;
			case RewardedState.Closed:
				if (rewarded)
				{
					ShowRewardAndReload();
					rewarded = false;
				}
				else
				{
					Reload();
				}
				break;
			case RewardedState.Failed:
				break;
			default:
				break;
		}
	}

	private void OnRewardChanged()
	{
		getRewardButtonText.text = $"+ {(GameManager.LevelReward.CollectedMoneyAmount * rewardLine.CurrentMultiplier)}";
	}

	private void UpdateUI()
	{
		moneyCounter.SetNumber(GameManager.StorageReader.Money);
		coinsCollectedNumber.SetNumber(GameManager.LevelReward.CollectedMoneyAmount);
	}

	private void OnDisable()
	{
		rewardLine.RewardChanged += OnRewardChanged;
		getRewardButton.OnClick.RemoveListener(OnGetRewardButtonClick);
		GameManager.SDK.RewardedStateChanged -= OnRewardedStateChanged;
		noThxButton.OnClick.RemoveListener(OnNoThxButtonClick);
		timer1?.Kill();
		timer2?.Kill();
	}
}
