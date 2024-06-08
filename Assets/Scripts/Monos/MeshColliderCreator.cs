using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class MeshColliderCreator : MonoBehaviour
{
	private MeshCollider meshCollider;
	public MeshCollider MeshCollider
	{
		get
		{
			if (meshCollider == null)
			{
				meshCollider = GetComponent<MeshCollider>();
			}
			return meshCollider;
		}
	}
	private MeshFilter meshFilter;
	public MeshFilter MeshFilter
	{
		get
		{
			if (meshFilter == null)
			{
				meshFilter = GetComponent<MeshFilter>();
			}
			return meshFilter;
		}
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		SetMesh();
	}
#endif

	private void Start()
	{
		SetMesh();
	}

	private void SetMesh()
	{
		MeshCollider.sharedMesh = MeshFilter.sharedMesh;
		MeshCollider.convex = true;
		MeshCollider.convex = false;
	}
}
