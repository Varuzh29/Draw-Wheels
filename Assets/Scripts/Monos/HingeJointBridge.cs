using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VarCo;

public class HingeJointBridge : MonoBehaviour
{
    [SerializeField] private Rigidbody start;
    [SerializeField] private HingeJoint end;
    [SerializeField] private HingeJoint bridgeSegmentPrefab;
    [SerializeField] private Transform segmentsParent;
    [SerializeField] private float segmentsMass;
    [SerializeField] private float segmentsPerUnit;
	[SerializeField] private float spacing;
    private List<HingeJoint> bridgeJoints = new List<HingeJoint>();

	[Button]
    private void Generate()
    {
		Extensions.DestroyImmediateAllChildren(segmentsParent);
		bridgeJoints.Clear();

		float bridgeLength = Vector3.Distance(start.transform.position, end.transform.position);
		int jointsCount = Mathf.RoundToInt(bridgeLength * segmentsPerUnit);
		int countDifference = jointsCount - bridgeJoints.Count;

		if (countDifference == 0) return;

		if (countDifference > 0)
		{
			for (int i = 0; i < countDifference; i++)
			{
				AddJoint();
			}
		}
		else if (countDifference < 0)
		{
			for (int i = 0; i < Mathf.Abs(countDifference); i++)
			{
				RemoveJoint();
			}
		}

		ArrangeJoints();
	}

    private void AddJoint()
    {
		HingeJoint joint = Instantiate(bridgeSegmentPrefab, segmentsParent);
		bridgeJoints.Add(joint);
    }

    private void RemoveJoint()
    {
        int lastIndex = bridgeJoints.Count - 1;
        DestroyImmediate(bridgeJoints[lastIndex]);
        bridgeJoints.RemoveAt(lastIndex);
	}

	private void ArrangeJoints()
    {
		float bridgeLength = Vector3.Distance(start.transform.position, end.transform.position);
		HingeJoint startJoint = bridgeJoints[0];
		startJoint.connectedBody = start;
		startJoint.transform.position = start.transform.position + start.transform.TransformDirection(Vector3.forward) * 0.5f;

		for (int i = 1; i < bridgeJoints.Count; i++)
		{
			Rigidbody prevJoint = bridgeJoints[i - 1].GetComponent<Rigidbody>();
			HingeJoint currentJoint = bridgeJoints[i];
			currentJoint.connectedAnchor = new Vector3(0, 0.5f, 0.5f + spacing);
			currentJoint.connectedBody = prevJoint;
			currentJoint.transform.position = Vector3.Lerp(start.transform.position, end.transform.position, i / (float)bridgeJoints.Count) + currentJoint.transform.TransformDirection(Vector3.forward) * 0.5f;
			prevJoint.mass = segmentsMass;
		}

		HingeJoint lastJoint = bridgeJoints[bridgeJoints.Count - 1];
		Rigidbody lastJointRB = lastJoint.GetComponent<Rigidbody>();
		end.connectedBody = lastJointRB;
		lastJointRB.mass = segmentsMass;
    }
}
