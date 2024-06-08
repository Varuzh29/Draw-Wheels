using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformRotator : MonoBehaviour
{
    [SerializeField] private Transform m_transform;
    [SerializeField] private Vector3 rotation;

	private void Update()
	{
		m_transform.Rotate(rotation * Time.deltaTime);
	}
}
