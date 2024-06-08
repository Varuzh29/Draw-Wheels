using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetter : MonoBehaviour
{
	[SerializeField] private GameObject enemyLevel;

	private void Start()
	{
		enemyLevel.SetActive(GameManager.StorageReader.Level >= GameManager.Config.EnemysMinimalLevel);
	}
}
