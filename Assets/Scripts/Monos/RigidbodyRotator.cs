using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyRotator : MonoBehaviour
{
	[SerializeField] private Rigidbody m_rigidbody;
	[SerializeField] private float multiplier = 1;
	private float zPos = 0f;

	private void Awake()
	{
		GameManager.GameStateChanged += OnGameStateChanged;
	}

	private void OnGameStateChanged(GameState gameState)
	{
		enabled = gameState == GameState.Race;
	}

	private void Start()
	{
		zPos = transform.position.z;
	}

	private void FixedUpdate()
	{
		Vector3 torque = new Vector3(0, 0, -GameManager.Config.TorqueForce * multiplier);
		m_rigidbody.AddTorque(torque * Time.fixedDeltaTime, ForceMode.VelocityChange);
		transform.position = new Vector3(
			x: transform.position.x,
			y: transform.position.y,
			z: zPos);
		transform.eulerAngles = new Vector3(
			x: 0,
			y: 0,
			z: transform.eulerAngles.z);
	}

	private void OnDestroy()
	{
		GameManager.GameStateChanged -= OnGameStateChanged;
	}
}
