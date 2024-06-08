using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VarCo;

public class EnemyNickname : MonoBehaviour
{
	[SerializeField] private SpriteRenderer flagRenderer;
	[SerializeField] private TextMeshPro nicknameTMP;

	private void Awake()
	{
		GameManager.GameStateChanged += OnGameStateChanged;
	}

	private void OnGameStateChanged(GameState gameState)
	{
		gameObject.SetActive(gameState == GameState.Race || gameState == GameState.Win || gameState == GameState.Lose);
	}

	private void Start()
	{
		flagRenderer.sprite = GameManager.Config.Flags.GetRandom();
		nicknameTMP.text = GameManager.Config.Nicknames.GetRandom();
	}

	private void Update()
	{
		transform.LookAt(Mouse3D.MainCamera.transform.position);
	}

	private void OnDestroy()
	{
		GameManager.GameStateChanged -= OnGameStateChanged;
	}
}
