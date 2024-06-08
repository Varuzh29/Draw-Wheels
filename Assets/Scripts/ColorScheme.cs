using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ColorScheme : ScriptableObject
{
    [SerializeField] private Color lightBlockColor; public Color LightBlockColor => lightBlockColor;
	[SerializeField] private Color darkBlockColor; public Color DarkBlockColor => darkBlockColor;
}
