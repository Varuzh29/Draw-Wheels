using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using NaughtyAttributes;

public class CollectCoinsTween : MonoBehaviour
{
    [SerializeField] private Transform[] coins;
    [SerializeField] private Transform startPoint;
	[SerializeField] private float randomBounds;
    [SerializeField] private float scaleEffect;
    [SerializeField] private float delay;
    [SerializeField] private string collectSound;
    [SerializeField] private AudioYB audioYB;
    private Sequence sequence;

    [Button]
    public void Play(Action onComplete = null)
    {
        sequence?.Kill();
        sequence = DOTween.Sequence();
        sequence
            .AppendCallback(SetStartPositions)
            .AppendInterval(0.5f)
            .AppendCallback(MoveCoinsToFinalPoint)
            .AppendInterval(coins.Length * delay + 0.25f)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
            });
    }

    public CollectCoinsTween SetStartPoint(Transform startPoint)
    {
        this.startPoint = startPoint;
        return this;
    }

    private void SetStartPositions()
    {
        foreach (RectTransform coin in coins)
        {
            coin.SetParent(startPoint, true);
            coin.localPosition = Vector3.zero;
            coin.anchoredPosition = new Vector2(coin.anchoredPosition.x + UnityEngine.Random.Range(-randomBounds, randomBounds), coin.anchoredPosition.y + UnityEngine.Random.Range(-randomBounds, randomBounds));
            coin.SetParent(transform, true);
			coin.gameObject.SetActive(true);
            coin.DOKill();
            coin.DOScale(1, 0.5f).From(0);
        }
    }

    private void MoveCoinsToFinalPoint()
    {
        for (int i = 0; i < coins.Length; i++)
        {
            Transform coin = coins[i];
            coin.DOKill();
			coin.DOLocalMove(Vector3.zero, 0.5f).SetDelay(delay * (i + 1)).OnComplete(() =>
            {
                coin.DOScale(scaleEffect, 0.1f).OnComplete(() =>
                {
                    audioYB.PlayOneShot(collectSound);
                    coin.DOScale(1, 0.15f).OnComplete(() =>
                    {
                        coin.gameObject.SetActive(false);
                    });
                });
            });
		}
    }
}
