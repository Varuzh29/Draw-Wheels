using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class CapsuleColliderController : MonoBehaviour
{
	private CapsuleCollider capsuleCollider;

	private void Awake()
	{
		capsuleCollider = GetComponent<CapsuleCollider>();
	}

	public void Set(Vector3 startPoint, Vector3 endPoint, float radius)
    {
		transform.position = startPoint;
		capsuleCollider.direction = 2;
		Vector3 direction = endPoint - startPoint;
		transform.forward = direction.normalized;
		float distance = direction.magnitude;
		capsuleCollider.center = Vector3.forward * distance * 0.5f;
		capsuleCollider.height = distance;
		capsuleCollider.radius = radius;
    }
}
