using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyCenterOfMass : MonoBehaviour
{
	[SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] private Vector3 centerOfMass;

	private void Awake()
	{
		m_rigidbody.centerOfMass = centerOfMass;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.TransformPoint(centerOfMass), 0.25f);
	}
}
