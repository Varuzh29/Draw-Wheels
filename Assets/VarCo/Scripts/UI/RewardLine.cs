using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace VarCo
{
	public class RewardLine : MonoBehaviour
	{
		public event Action RewardChanged;
		[SerializeField] private RectTransform arrow;
		[SerializeField] private float lineWith;
		[SerializeField] private Image[] selectors;
		[SerializeField] private int[] multipliers;
		[SerializeField] private float loopDuration;
		[SerializeField] private Ease ease;
		//
		private int selectedMultiplier;
		private int lastSelectedMultiplier;
		private float percentage;
		private int currentMultiplier;
		public int CurrentMultiplier => currentMultiplier;
		private Tween tween;

		private void Start()
		{
			selectors[selectedMultiplier].gameObject.SetActive(true);
			currentMultiplier = multipliers[selectedMultiplier];
			StartLoop();
		}

		private void StartLoop()
		{
			tween = DOTween.To(() => percentage, x => percentage = x, 1, loopDuration).SetEase(ease).SetLoops(-1, LoopType.Yoyo)
				.OnUpdate(() =>
				{
					arrow.anchoredPosition = new Vector2(Mathf.Lerp(-lineWith, lineWith, percentage), arrow.anchoredPosition.y);
					selectedMultiplier = Mathf.RoundToInt(Mathf.Lerp(0, selectors.Length - 1, percentage));
					if (lastSelectedMultiplier != selectedMultiplier)
					{
						selectors[lastSelectedMultiplier].gameObject.SetActive(false);
						selectors[selectedMultiplier].gameObject.SetActive(true);
						lastSelectedMultiplier = selectedMultiplier;
						currentMultiplier = multipliers[selectedMultiplier];
						RewardChanged?.Invoke();
					}
				});
		}

		private void OnDestroy()
		{
			tween.Kill();
		}

		public void Stop()
		{
			tween.Pause();
		}
	}
}
