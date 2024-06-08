using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternSaver : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Pattern pattern;

    [Button]
    private void SavePattern()
    {
        Vector3[] ponts = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(ponts);
        for (int i = 0; i < ponts.Length; i++)
        {
            ponts[i].z = 0;
        }
        pattern.SetPositions(ponts);
    }
}
