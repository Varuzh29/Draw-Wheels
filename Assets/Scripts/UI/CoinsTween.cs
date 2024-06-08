using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsTween : MonoBehaviour
{
	[SerializeField] private Transform finalPoint;
	[SerializeField] private float randomBounds;
	[SerializeField] private float minDelay;
	[SerializeField] private float maxDelay;
	[SerializeField] private float duration;
	[SerializeField] private AudioYB audioYB;
	[SerializeField] private string collectSound;
	private Transform[] coins;
	private Vector3 finalScale;

	private void Awake()
	{
		coins = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++)
		{
			coins[i] = transform.GetChild(i);
		}
		finalScale = coins[0].localScale;
	}

	private void OnEnable()
	{
		foreach (RectTransform coin in coins)
		{
			coin.anchoredPosition = new Vector2(coin.anchoredPosition.x + Random.Range(-randomBounds, randomBounds), coin.anchoredPosition.y + Random.Range(-randomBounds, randomBounds));
			coin.SetParent(finalPoint, true);

			DOTween.Sequence()
				.Append(coin.DOScale(finalScale, 0.5f).From(Vector3.zero))
				.Append(coin.DOLocalMove(Vector3.zero, duration).OnComplete(() =>
				{
					//coinIconTween.Play();
					audioYB?.PlayOneShot(collectSound);
				})
				.SetDelay(Random.Range(minDelay, maxDelay)));
		}
	}
}
