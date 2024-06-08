using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VarCo;

public class CollectNumberTween : MonoBehaviour
{
    [SerializeField] private Transform m_transform;
    [SerializeField] private TextMeshPro tmp;
	[SerializeField, Min(0)] private float duration;
	[SerializeField] private Ease ease;
	private Sequence sequence;

	private void OnEnable()
	{
		Vector3 direction = (m_transform.position - Mouse3D.MainCamera.transform.position).normalized;
		m_transform.forward = direction;
		sequence?.Kill();
		sequence = DOTween.Sequence();
		sequence
			.Append(m_transform.DOLocalMoveY(2, duration).From(1))
			.Join(tmp.DOFade(0, duration).From(1))
			.SetEase(ease);
	}

	private void OnDisable()
	{
		sequence.Kill();
	}
}
