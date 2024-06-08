using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
	[SerializeField] private Rigidbody refRigidbody;
    [SerializeField] private Vector3 rotationDirection;

	private void Update()
	{
		transform.Rotate(refRigidbody.velocity.x * rotationDirection);
	}
}
