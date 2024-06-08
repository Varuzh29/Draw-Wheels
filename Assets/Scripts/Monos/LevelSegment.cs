using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelSegment : MonoBehaviour
{
	[SerializeField] private Vector3 nextSegmentPosition = new Vector3(0, 0, 5);
	public Vector3 NextSegmentPosition => transform.TransformPoint(nextSegmentPosition);
}

#if UNITY_EDITOR
[CustomEditor(typeof(LevelSegment))]
public class LevelSegmentEditor : Editor
{
	public void OnSceneGUI()
	{
		LevelSegment levelSegment = (LevelSegment)target;
		Handles.color = Color.cyan;
		EditorGUI.BeginChangeCheck();
		SerializedProperty endPointProperty = serializedObject.FindProperty("nextSegmentPosition");
		Vector3 newEndPoint = Handles.PositionHandle(levelSegment.NextSegmentPosition, levelSegment.transform.rotation);
		if (EditorGUI.EndChangeCheck())
		{
			endPointProperty.vector3Value = levelSegment.transform.InverseTransformPoint(newEndPoint);
			serializedObject.ApplyModifiedProperties();
		}
		Handles.Label(levelSegment.NextSegmentPosition, $"Next segment position: \n" +
			$" {endPointProperty.vector3Value}", "Box");
	}
}
#endif
