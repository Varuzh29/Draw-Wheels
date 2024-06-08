using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VarCo;

public class Drawing : MonoBehaviour
{
    [SerializeField] private LineRenderer m_lineRenderer;
	[SerializeField] private CapsuleColliderController m_capsuleColliderPrefab;
	public LineRenderer LineRenderer => m_lineRenderer;
	private List<Vector3> points = new List<Vector3>();

	private void Start()
	{
		m_lineRenderer.positionCount = 0;
	}

	public void AddPoint(Vector3 position)
	{
		if (points.Count + 1 > GameManager.Config.MaxPointsCount) return;

		Vector3 localPosition = transform.InverseTransformPoint(position);
		localPosition.z = 0;
		points.Add(localPosition);
		SetLinePositions(points.ToArray());
	}

	public void SetLinePositions(Vector3[] positions)
	{
		m_lineRenderer.positionCount = positions.Length;
		m_lineRenderer.SetPositions(positions);
	}

	public void GenerateMesh()
	{
		Vector3[] points = new Vector3[m_lineRenderer.positionCount];
		m_lineRenderer.GetPositions(points);
		Extensions.DestroyImmediateAllChildren(transform);
		for (int i = 0; i + 1 < points.Length; i++)
		{
			Vector3 currentPoint = points[i];
			Vector3 nextPoint = points[i + 1];
			CapsuleColliderController capsuleCollider = Instantiate(m_capsuleColliderPrefab);
			capsuleCollider.transform.SetParent(transform);
			capsuleCollider.Set(transform.TransformPoint(currentPoint), transform.TransformPoint(nextPoint), m_lineRenderer.endWidth * 0.5f);
		}
	}

	[Button]
	public void Clear()
	{
		points.Clear();
		m_lineRenderer.positionCount = 0;
	}
}
