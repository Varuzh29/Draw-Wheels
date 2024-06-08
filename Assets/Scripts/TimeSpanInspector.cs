using System;
using UnityEngine;

[Serializable]
public class TimeSpanInspector
{
    [SerializeField] private int days, hours, minutes, seconds, milliseconds;

    public TimeSpan ToTimeSpan()
    {
        return new TimeSpan(days, hours, minutes, seconds, milliseconds);
    }
}
