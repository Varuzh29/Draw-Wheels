using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VarCo;

public class SearchPage : MonoBehaviour
{
    [SerializeField] private float minDelay;
    [SerializeField] private float maxDelay;
    [SerializeField] private Transform text;
	[SerializeField] private float scaleEffect;
	[SerializeField] private float scaleDuration;
	[SerializeField] private Ease ease;
	private Tween tween;
	private Sequence timer;

	private void OnEnable()
	{
		tween?.Kill();
		tween = text.transform.DOScale(scaleEffect, scaleDuration).From(1).SetLoops(-1, LoopType.Yoyo).SetEase(ease);
		float delay = Random.Range(minDelay, maxDelay);
		timer?.Kill();
		timer = DOTimer.Set(delay, () =>
		{
			GameManager.StartRace();
		});
	}

	private void OnDisable()
	{
		tween?.Kill();
		timer?.Kill();
	}
}
