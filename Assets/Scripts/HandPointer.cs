using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VarCo;

public class HandPointer : MonoBehaviour
{
	private void Update()
	{
		transform.position = Mouse3D.WorldPoint(2);
		transform.forward = (transform.position - Mouse3D.MainCamera.transform.position).normalized;
	}
}
