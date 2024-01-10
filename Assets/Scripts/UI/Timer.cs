using JetBrains.Annotations;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Timer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gameManager;

    private EventManager eventManager;
    private TMP_Text timerText;
    private float time = 30f;
    private bool isRunning = false;
    [SerializeField]
    private bool showMilliSeconds = true;

    void Awake()
    {
        timerText = GetComponent<TMP_Text>();
        eventManager = gameManager.GetComponent<EventManager>();
    }

    private void OnEnable()
    {
        eventManager.OnTimerStart.AddListener(StartTimer);
        eventManager.OnTimerStop.AddListener(StopTimer);
        eventManager.OnTimerUpdate.AddListener(UpdateTimer);
        eventManager.OnTimerSet.AddListener(SetTimer);

    }

    private void OnDisable()
    {
        eventManager.OnTimerStart.RemoveListener(StartTimer);
        eventManager.OnTimerStop.RemoveListener(StopTimer);
        eventManager.OnTimerUpdate.RemoveListener(UpdateTimer);
        eventManager.OnTimerSet.RemoveListener(SetTimer);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning && time < 0.0f)
        {
            eventManager.OnTimerStop.Invoke();
            return;
        }

        time -= Time.deltaTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        string format = showMilliSeconds ? @"mm\:ss\:ff" : @"mm\:ss";
        timerText.text = timeSpan.ToString(format);

    }

    private void UpdateTimer(float s)
    {
        time += s;
    }

    private void SetTimer(float s)
    {
        time = s;
    }

    private void StartTimer()
    {
        isRunning = true;
    }

    private void StopTimer()
    {
        isRunning = false;
    }
}
