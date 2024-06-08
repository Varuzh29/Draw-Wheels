using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SkinCardState
{
	Hidden,
	NotSelected,
	Selected,
	SelectedByRandom
}

public class SkinCard : MonoBehaviour
{
	[SerializeField] private Image icon;
    [SerializeField] private GameObject selected;
    [SerializeField] private GameObject selectedByRandom;
    [SerializeField] private GameObject hiddenIcon;
	[SerializeField] private UIButton uIButton;
	private int skinId;
    public SkinCardState State { get; private set; }

	private void Awake()
	{
		SetState(SkinCardState.Hidden);
	}

	private void OnEnable()
	{
		uIButton.OnClick.AddListener(OnUIButtonClick);
	}

	private void OnUIButtonClick()
	{
		GameManager.SetSkin(skinId);
	}

	private void OnDisable()
	{
		uIButton.OnClick.RemoveListener(OnUIButtonClick);
	}

	public SkinCard SetState(SkinCardState state)
	{
		if(this.State == state) return this;

		this.State = state;

		switch (this.State)
		{
			case SkinCardState.Hidden:
				hiddenIcon.SetActive(true);
				selected.SetActive(false);
				selectedByRandom.SetActive(false);
				icon.gameObject.SetActive(false);
				break;
			case SkinCardState.NotSelected:
				hiddenIcon.SetActive(false);
				selected.SetActive(false);
				selectedByRandom.SetActive(false);
				icon.gameObject.SetActive(true);
				break;
			case SkinCardState.Selected:
				hiddenIcon.SetActive(false);
				selected.SetActive(true);
				selectedByRandom.SetActive(false);
				icon.gameObject.SetActive(true);
				break;
			case SkinCardState.SelectedByRandom:
				hiddenIcon.SetActive(true);
				selected.SetActive(false);
				selectedByRandom.SetActive(true);
				icon.gameObject.SetActive(false);
				break;
		}

		return this;
	}

	public SkinCard SetSkinId(int skinId)
	{
		this.skinId = skinId;
		icon.sprite = GameManager.Config.Skins.Collection[skinId].icon;
		return this;
	}

	public SkinCard SetInteractable(bool interactable)
	{
		uIButton.Interactable = interactable;
		return this;
	}
}
