using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceSliderUI : MonoBehaviour
{
    [SerializeField] private LevelBuilder levelBuilder;
    [SerializeField] private Transform player;
    [SerializeField] private Transform enemy;
	[SerializeField] private RectTransform sliderArea;
	[SerializeField] private RectTransform playerDot;
	[SerializeField] private RectTransform enemyDot;
	private float startX;
	private float finishX;

	private void OnEnable()
	{
		startX = player.transform.position.x;
		finishX = levelBuilder.transform.GetChild(levelBuilder.transform.childCount - 1).position.x;
	}

	private void Update()
	{
		playerDot.anchoredPosition = new Vector2(CalculateDotPosition(player), playerDot.anchoredPosition.y);
		enemyDot.anchoredPosition = new Vector2(CalculateDotPosition(enemy), enemyDot.anchoredPosition.y);
	}

	private float CalculateDotPosition(Transform dotTarget)
	{
		float range = Mathf.Abs(finishX - startX);
		float distanceFromStart = Mathf.Abs(dotTarget.position.x - startX);
		float position = distanceFromStart / range;
		float sliderWith = sliderArea.rect.width - playerDot.rect.width;
		float result = Mathf.Clamp(position * sliderWith, 0, sliderWith);
		return result;
	}
}
