using UnityEngine;

public class Accelerator : MonoBehaviour
{
	[SerializeField] private Rigidbody m_rigidbody;

	private void Awake()
	{
		GameManager.GameStateChanged += OnGameStateChanged;
	}

	private void OnGameStateChanged(GameState gameState)
	{
		enabled = gameState == GameState.Race;
	}

	private void FixedUpdate()
	{
		m_rigidbody.AddForce(Vector3.right * 0.5f * GameManager.StorageReader.SpeedMultiplier * Time.fixedDeltaTime, ForceMode.VelocityChange);
	}

	private void OnDestroy()
	{
		GameManager.GameStateChanged -= OnGameStateChanged;
	}
}
