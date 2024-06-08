using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageSwitcher : MonoBehaviour
{
	[SerializeField] private float fadeDuration;
	private Tween tween;

	public void Show(CanvasGroup canvasGroup)
	{
		canvasGroup.gameObject.SetActive(true);
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = true;
		canvasGroup.alpha = 0;
		tween = canvasGroup.DOFade(1, fadeDuration).OnComplete(() =>
		{
			canvasGroup.interactable = true;
		});
	}

	public void Hide(CanvasGroup canvasGroup)
	{
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = true;
		canvasGroup.alpha = 1;
		tween = canvasGroup.DOFade(0, fadeDuration).OnComplete(() =>
		{
			canvasGroup.blocksRaycasts = false;
			canvasGroup.gameObject.SetActive(false);
		});
	}

	private void OnDisable()
	{
		tween.Kill();
	}
}
