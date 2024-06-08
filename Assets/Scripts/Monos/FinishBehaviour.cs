using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishBehaviour : MonoBehaviour
{
    [SerializeField] private Behaviour[] behavioursToDisable;

    public void DisableBehaviours()
    {
        foreach (Behaviour behaviour in behavioursToDisable)
        {
            behaviour.enabled = false;
        }
    }
}
