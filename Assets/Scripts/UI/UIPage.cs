using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIPage : MonoBehaviour
{
	private CanvasGroup canvasGroup;
	private CanvasGroup CanvasGroup
	{
		get
		{
			if (canvasGroup == null)
			{
				canvasGroup = GetComponent<CanvasGroup>();
			}
			return canvasGroup;
		}
	}
	private Tween tween;

	public void Show(float fadeDuration)
	{
		CanvasGroup.gameObject.SetActive(true);
		CanvasGroup.interactable = false;
		CanvasGroup.blocksRaycasts = true;
		CanvasGroup.alpha = 0;
		tween?.Kill();
		tween = CanvasGroup.DOFade(1, fadeDuration).OnComplete(() =>
		{
			CanvasGroup.interactable = true;
		});
	}

	public void Hide(float fadeDuration)
	{
		CanvasGroup.interactable = false;
		CanvasGroup.blocksRaycasts = true;
		CanvasGroup.alpha = 1;
		tween?.Kill();
		tween = CanvasGroup.DOFade(0, fadeDuration).OnComplete(() =>
		{
			CanvasGroup.blocksRaycasts = false;
			CanvasGroup.gameObject.SetActive(false);
		});
	}
}
