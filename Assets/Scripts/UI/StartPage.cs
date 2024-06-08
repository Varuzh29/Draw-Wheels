using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VarCo;

public class StartPage : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI levelNumberTMP;
	[Space]
	[SerializeField] private UIButton speedButton;
	[SerializeField] private TextMeshProUGUI speedResultTMP;
	[SerializeField] private TextMeshProUGUI speedPriceTMP;
	[SerializeField] private GameObject speedPriceContainer;
	[SerializeField] private GameObject speedADContainer;
	[Space]
	[SerializeField] private UIButton incomeButton;
	[SerializeField] private TextMeshProUGUI incomeResultTMP;
	[SerializeField] private TextMeshProUGUI incomePriceTMP;
	[SerializeField] private GameObject incomePriceContainer;
	[SerializeField] private GameObject incomeADContainer;
	[Space]
	[SerializeField] private NumberTweener moneyCount;
	[Space]
	[SerializeField] private UIButton developerButton;
	[SerializeField] private UIButton shopButton;
	[Space]
	[SerializeField] private UIButton audioButton;
	[SerializeField] private Image audioIcon;
	[SerializeField] private Sprite audioOnIcon;
	[SerializeField] private Sprite audioOffIcon;
	[SerializeField] private UIButton dailyRewardButton;
	[SerializeField] private DailyRewardPopUp dailyRewardPopUp;
	[SerializeField] private GameObject dailyRewardBadge;

	private void OnEnable()
	{
		GameManager.PlayerBoosters.ValuesChanged += OnValuesChanged;
		dailyRewardPopUp.Claimed += OnDailyRewardPopUpClaimed;
		dailyRewardButton.OnClick.AddListener(OnDailyRewardButtonClick);
		speedButton.OnClick.AddListener(SpeedBoost);
		incomeButton.OnClick.AddListener(IncomeBoost);
		shopButton.OnClick.AddListener(OnShopButtonClick);
		audioButton.OnClick.AddListener(OnAudioButtonClick);
		if (GameManager.SDK is PluginYGBridge)
		{
			developerButton.OnClick.AddListener(OnDeveloperButtonClick);
		}
		else
		{
			developerButton.gameObject.SetActive(false);
		}
		UpdateUI();
	}

	private void OnDailyRewardPopUpClaimed()
	{
		UpdateUI();
	}

	private void OnDailyRewardButtonClick()
	{
		dailyRewardPopUp.gameObject.SetActive(true);
	}

	private void OnAudioButtonClick()
	{
		GameManager.ToggleAudio();
		UpdateUI();
	}

	private void OnShopButtonClick()
	{
		GameManager.OpenShop();
	}

	private void OnValuesChanged()
	{
		UpdateUI();
	}

	private void OnDeveloperButtonClick()
	{
		PluginYGBridge pluginYGBridge = (PluginYGBridge)GameManager.SDK;
		pluginYGBridge.OpenYandexDeveloperPage();
	}

	private void UpdateUI()
	{
		//Level
		levelNumberTMP.text = GameManager.StorageReader.Level.ToString();
		//Money
		moneyCount.SetNumber(GameManager.StorageReader.Money);
		//Speed Button
		string currentSpeed = GameManager.StorageReader.SpeedMultiplier.ToString();
		string nextSpeed = (GameManager.StorageReader.SpeedMultiplier + 1).ToString();
		speedResultTMP.text = $"{currentSpeed} => {nextSpeed}";
		speedPriceTMP.text = GameManager.PlayerBoosters.SpeedPrice.ToString();
		speedPriceContainer.SetActive(GameManager.PlayerBoosters.CanBuySpeed);
		speedADContainer.SetActive(!speedPriceContainer.activeInHierarchy);
		//Income Button
		string currentIncome = GameManager.StorageReader.IncomeMultiplier.ToString();
		string nextIncome = (GameManager.StorageReader.IncomeMultiplier + 1).ToString();
		incomeResultTMP.text = $"x{currentIncome} => x{nextIncome}";
		incomePriceTMP.text = GameManager.PlayerBoosters.IncomePrice.ToString();
		incomePriceContainer.SetActive(GameManager.PlayerBoosters.CanBuyIncome);
		incomeADContainer.SetActive(!incomePriceContainer.activeInHierarchy);
		//Audio Button
		audioIcon.sprite = GameManager.StorageReader.Audio ? audioOnIcon : audioOffIcon;
		//Daily Reward
		dailyRewardBadge.SetActive(GameManager.DailyReward.CanClaim);
	}

	private void SpeedBoost()
	{
		GameManager.PlayerBoosters.IncreaseSpeed();
		UpdateUI();
	}

	private void IncomeBoost()
	{
		GameManager.PlayerBoosters.IncreaseIncome();
		UpdateUI();
	}

	private void OnDisable()
	{
		speedButton.OnClick.RemoveListener(SpeedBoost);
		incomeButton.OnClick.RemoveListener(IncomeBoost);
		shopButton.OnClick.RemoveListener(OnShopButtonClick);
		audioButton.OnClick.RemoveListener(OnAudioButtonClick);
		dailyRewardButton.OnClick.RemoveListener(OnDailyRewardButtonClick);
		dailyRewardPopUp.Claimed -= OnDailyRewardPopUpClaimed;
		if (GameManager.SDK is PluginYGBridge)
		{
			developerButton.OnClick.RemoveListener(OnDeveloperButtonClick);
		}
		GameManager.PlayerBoosters.ValuesChanged -= OnValuesChanged;
	}
}
