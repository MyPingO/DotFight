using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    // Events for the timer
    [NonSerialized]
    public UnityEvent OnTimerStart = new UnityEvent();
    [NonSerialized]
    public UnityEvent OnTimerStop = new UnityEvent();
    [NonSerialized]
    public UnityEvent<float> OnTimerUpdate = new UnityEvent<float>();
    [NonSerialized]
    public UnityEvent<float> OnTimerSet = new UnityEvent<float>();

    // Events to keep track of the score
    [NonSerialized]
    public UnityEvent<int> OnScoreUpdate = new UnityEvent<int>();
    [NonSerialized]
    public UnityEvent<int> OnScoreSet = new UnityEvent<int>();
    [NonSerialized]
    public UnityEvent<int> OnScoreReset = new UnityEvent<int>();

}
