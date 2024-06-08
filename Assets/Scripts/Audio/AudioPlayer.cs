using UnityEngine;

[RequireComponent(typeof(AudioYB))]
public class AudioPlayer : MonoBehaviour
{
	[SerializeField] private string winSound;
	[SerializeField] private string loseSound;
	private AudioYB audioYB;

	private void Awake()
	{
		audioYB = GetComponent<AudioYB>();
		GameManager.GameStateChanged += OnGameStateChanged;
	}

	private void OnGameStateChanged(GameState gameState)
	{
		if (gameState == GameState.Win)
		{
			audioYB.PlayOneShot(winSound);
		}
		if (gameState == GameState.Lose)
		{
			audioYB.PlayOneShot(loseSound);
		}
	}

	private void OnDestroy()
	{
		GameManager.GameStateChanged -= OnGameStateChanged;
	}
}
