using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Pattern : ScriptableObject
{
    [SerializeField] private Vector3[] points;
    public Vector3[] Points => points;

    public void SetPositions(Vector3[] points)
    {
        this.points = points;
    }
}
