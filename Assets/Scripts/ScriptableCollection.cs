using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableCollection<T> : ScriptableObject
{
    [SerializeField] private T[] collection;
    public T[] Collection => collection;

    public T GetRandom()
    {
        return collection[Random.Range(0, collection.Length)];
    }
}
