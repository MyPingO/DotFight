using System;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    // Events for the timer
    [NonSerialized]
    public UnityEvent OnTimerStart = new();
    [NonSerialized]
    public UnityEvent OnTimerStop = new();
    [NonSerialized]
    public UnityEvent<float> OnTimerUpdate = new();
    [NonSerialized]
    public UnityEvent<float> OnTimerSet = new();

    // Events to keep track of the score
    [NonSerialized]
    public UnityEvent OnPlayerScore = new();
    [NonSerialized]
    public UnityEvent OnAiScore = new();
}
