using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventBus
{
    public UnityEvent PlayerStopPainting { get; private set; } = new UnityEvent();
    public UnityEvent SomeOneFinished { get; private set; } = new UnityEvent();
}
