using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{
    [SerializeField] private UnityEvent Collected;
    [SerializeField] private UnityEvent CollectedByPlayer;
	private bool collected;

	private void OnTriggerEnter(Collider other)
	{
		if (collected) return;

		if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
		{
			Collected?.Invoke();

			if(other.gameObject.CompareTag("Player"))
			{
				CollectedByPlayer?.Invoke();
			}

			collected = true;
		}
	}
}
