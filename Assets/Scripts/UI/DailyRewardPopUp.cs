using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DailyRewardPopUp : MonoBehaviour
{
	[SerializeField] private Transform windowTransform;
	[SerializeField] private Ease ease;
	[SerializeField] private float duration;
	[SerializeField] private UIButton closeButton;
	[SerializeField] private UIButton claimButton;
	[SerializeField] private GameObject waitTextParent;
	[SerializeField] private TextMeshProUGUI waitTime;
	[SerializeField] private RewardItemUI rewardItemPrefab;
	[SerializeField] private Transform gridLayout;
	[SerializeField] private CollectCoinsTween collectCoinsTween;
	private List<RewardItemUI> rewardItems;
	public event Action Claimed;
	private Tween tween;

	private void Awake()
	{
		rewardItems = new List<RewardItemUI>();
		for (int i = 0; i < GameManager.Config.MaxStreak; i++)
		{
			rewardItems.Add(Instantiate(rewardItemPrefab, gridLayout));
		}
	}

	private void UpdateItems()
	{
		for (int i = 0; i < rewardItems.Count; i++)
		{
			RewardItemUI item = rewardItems[i];
			item.SetClaimed(i < GameManager.DailyReward.Streak)
				.SetDayNumber(i + 1)
				.SetRewardAmount((i + 1) * GameManager.Config.DailyRewardAmountStep);
		}
		Tic();
	}

	private void OnEnable()
	{
		closeButton.OnClick.AddListener(OnCloseButtonClick);
		claimButton.OnClick.AddListener(OnClaimButtonClick);
		tween?.Kill();
		tween = windowTransform.DOScale(1, duration).From(0).SetEase(ease)
			.OnStart(() =>
			{
				closeButton.Interactable = false;
				claimButton.Interactable = false;
			})
			.OnComplete(() =>
			{
				closeButton.Interactable = true;
				claimButton.Interactable = true;
			});
		UpdateItems();
		InvokeRepeating(nameof(Tic), 0, 0.5f);
	}

	private void OnDisable()
	{
		closeButton.Interactable = false;
		closeButton.OnClick.RemoveListener(OnCloseButtonClick);
		claimButton.Interactable = false;
		claimButton.OnClick.RemoveListener(OnClaimButtonClick);
		CancelInvoke();
		tween?.Kill();
	}

	private void OnCloseButtonClick()
	{
		gameObject.SetActive(false);
	}

	private void OnClaimButtonClick()
	{
		if (GameManager.DailyReward.TryClaim())
		{
			GameManager.PlayerWallet.AddMoney(GameManager.DailyReward.Streak * GameManager.Config.DailyRewardAmountStep);
			GameManager.SaveDailyRewardState();
			UpdateItems();
			Transform startPoint = rewardItems[GameManager.DailyReward.Streak - 1].transform;
			collectCoinsTween.SetStartPoint(startPoint).Play();
			Claimed?.Invoke();
		}
	}

	private void Tic()
	{
		claimButton.gameObject.SetActive(GameManager.DailyReward.CanClaim);
		waitTextParent.SetActive(!GameManager.DailyReward.CanClaim);
		if (!GameManager.DailyReward.CanClaim)
		{
			waitTime.text = GameManager.DailyReward.RemainingTime.ToString(@"hh\:mm\:ss");
		}
	}
}
