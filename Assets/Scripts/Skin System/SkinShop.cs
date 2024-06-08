using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VarCo;

public class SkinShop : MonoBehaviour
{
    [SerializeField] private Transform grid;
    [SerializeField] private SkinCard cardPrefab;
	[SerializeField] private UIButton closeButton;
	[SerializeField] private Image currentSkinIcon;
	[SerializeField] private UIButton unlockRandomButton;
	[SerializeField] private TextMeshProUGUI priceText;
	[SerializeField] private UIButton addCoinsButton;
	[SerializeField] private TextMeshProUGUI giftText;
	[SerializeField] private CollectCoinsTween collectCoinsTween;
	[SerializeField] private NumberTweener moneyCounter;
	[SerializeField] private float selectDuration;
	[SerializeField] private Ease ease;
	private SkinCard[] skinCards;
	private int skinsCount;
	private List<int> availableSkins = new List<int>();
	private bool rewarded = false;
	private int UnlockPrice
	{
		get
		{
			return GameManager.Config.UnlockPriceStep * (GameManager.StorageReader.AvailableSkins.Length);
		}
	}
	private Sequence sequence;

	private void Awake()
	{
		GameManager.GameStateChanged += OnGameStateChanged;
		skinsCount = GameManager.Config.Skins.Collection.Length;
		skinCards = new SkinCard[skinsCount];
		for (int i = 0; i < skinsCount; i++)
		{
			SkinCard skinCard = Instantiate(cardPrefab, grid);
			skinCard.SetInteractable(false).SetSkinId(i).SetState(SkinCardState.Hidden);
			skinCards[i] = skinCard;
		}
	}

	private void OnGameStateChanged(GameState gameState)
	{
		enabled = gameState == GameState.Shop;
	}

	private void OnEnable()
	{
		closeButton.OnClick.AddListener(OnCloseButtonClick);
		unlockRandomButton.OnClick.AddListener(OnUnlockRandomButtonClick);
		addCoinsButton.OnClick.AddListener(OnAddCoinsButtonClick);
		GameManager.CurrentSkinChanged += OnCurrentSkinChanged;
		GameManager.SDK.RewardedStateChanged += OnRewardedStateChanged;
		UpdateUI();
	}

	private void OnRewardedStateChanged(RewardedState rewardedState)
	{
		switch (rewardedState)
		{
			case RewardedState.Rewarded:
				GameManager.PlayerWallet.AddMoney(GameManager.Config.GiftCoinsAmount);
				rewarded = true;
				break;
			case RewardedState.Closed:
				if (rewarded)
				{
					collectCoinsTween.Play(() =>
					{
						UpdateUI();
					});
					rewarded = false;
				}
				break;
		}
	}

	private void OnAddCoinsButtonClick()
	{
		GameManager.SDK.ShowRewarded();
	}

	private void OnUnlockRandomButtonClick()
	{
		if (GameManager.PlayerWallet.TryPurchase(UnlockPrice))
		{
			moneyCounter.SetNumber(GameManager.StorageReader.Money);
			addCoinsButton.Interactable = false;
			closeButton.Interactable = false;
			unlockRandomButton.Interactable = false;
			for (int i = 0; i < skinsCount; i++)
			{
				SkinCard skinCard = skinCards[i];
				skinCard.SetInteractable(false);
			}
			List<int> lockedSkins = new List<int>();
			List<int> availableSkins = new List<int>();
			availableSkins.AddRange(GameManager.StorageReader.AvailableSkins);
			for (int i = 0; i < skinsCount; i++)
			{
				bool locked = !availableSkins.Contains(i);
				if (locked)
				{
					lockedSkins.Add(i);
				}
			}
			lockedSkins.Shuffle();
			int randomSkinId = lockedSkins[lockedSkins.Count - 1];
			GameManager.UnlockSkin(randomSkinId);
			sequence?.Kill();
			sequence = DOTween.Sequence();
			for (int i = 0; i < lockedSkins.Count; i++)
			{
				int id = lockedSkins[i];
				sequence
					.AppendCallback(() =>
					{
						RandomSelect(id);
					})
					.AppendInterval(selectDuration);
			}
			sequence.SetEase(ease);
			sequence.OnComplete(() =>
			{
				addCoinsButton.Interactable = true;
				closeButton.Interactable = true;
				unlockRandomButton.Interactable = true;
				GameManager.SetSkin(randomSkinId);
			});
		}
	}

	private void RandomSelect(int id)
	{
		for (int i = 0; i < skinsCount; i++)
		{
			SkinCard skinCard = skinCards[i];
			if (i == id)
			{
				skinCard.SetState(SkinCardState.SelectedByRandom);
			}
			else
			{
				if (skinCard.State == SkinCardState.SelectedByRandom)
				{
					skinCard.SetState(SkinCardState.Hidden);
				}
			}
		}
	}

	private void OnCurrentSkinChanged(int skinId)
	{
		UpdateUI();
	}

	private void OnCloseButtonClick()
	{
		GameManager.CloseShop();
	}

	private void UpdateUI()
	{
		availableSkins.Clear();
		availableSkins.AddRange(GameManager.StorageReader.AvailableSkins);
		currentSkinIcon.sprite = GameManager.Config.Skins.Collection[GameManager.StorageReader.CurrentSkin].icon;
		for (int i = 0; i < skinsCount; i++)
		{
			SkinCard skinCard = skinCards[i];

			if (GameManager.StorageReader.CurrentSkin == i)
			{
				skinCard.SetState(SkinCardState.Selected).SetInteractable(false);
			}
			else
			{
				if (availableSkins.Contains(i))
				{
					skinCard.SetState(SkinCardState.NotSelected).SetInteractable(true);
				}
				else
				{
					skinCard.SetState(SkinCardState.Hidden).SetInteractable(false);
				}
			}
		}

		moneyCounter.SetNumber(GameManager.StorageReader.Money);

		if (GameManager.StorageReader.AvailableSkins.Length >= skinsCount)
		{
			unlockRandomButton.gameObject.SetActive(false);
			addCoinsButton.gameObject.SetActive(false);
		}
		else
		{
			priceText.text = UnlockPrice.ToString();
			unlockRandomButton.Interactable = GameManager.PlayerWallet.MoneyCount >= UnlockPrice;
			giftText.text = $"+{GameManager.Config.GiftCoinsAmount}";
		}
	}

	private void OnDisable()
	{
		closeButton.OnClick.RemoveListener(OnCloseButtonClick);
		unlockRandomButton.OnClick.RemoveListener(OnUnlockRandomButtonClick);
		addCoinsButton.OnClick.RemoveListener(OnAddCoinsButtonClick);
		GameManager.CurrentSkinChanged -= OnCurrentSkinChanged;
		GameManager.SDK.RewardedStateChanged -= OnRewardedStateChanged;
		sequence?.Kill();
	}

	private void OnDestroy()
	{
		GameManager.GameStateChanged -= OnGameStateChanged;
	}
}
