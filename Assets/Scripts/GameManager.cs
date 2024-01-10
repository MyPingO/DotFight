using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private EventManager eventManager;


    private void Awake()
    {
        eventManager = GetComponent<EventManager>();
    }

    private void Start()
    {
        eventManager.OnTimerSet.Invoke(90);
        eventManager.OnTimerStart.Invoke();
    }
}
