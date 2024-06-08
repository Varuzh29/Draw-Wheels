using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinishTrigger : MonoBehaviour
{
	[SerializeField] private UnityEvent finished;
	private bool triggered;

	private void OnTriggerEnter(Collider other)
	{
		if (triggered) return;

		if (other.tag == "Player" || other.tag == "Enemy")
		{
			finished?.Invoke();
			GameManager.EventBus.SomeOneFinished?.Invoke();
			triggered = true;
		}
	}
}
