using UnityEngine;
using TMPro;
using System;

public class FPSDisplay : MonoBehaviour
{

    public TMP_Text FpsText, timerText;

    private readonly float pollingTime = 3f;
    private float time;
    private int frameCount;

    private bool startPause;
    DateTime startTime;
    public DateTime dateStart;
    private double totalSeconds;
    public TimeSpan TimeElapsed { get; private set; }

    private void Start()
    {
        startTime = DateTime.Now;
        startPause = false;
        totalSeconds = 0;
    }

    void Update()
    {
        if (!PauseMenu.isPaused) {

            if (startPause)
            {
                totalSeconds += (DateTime.Now - dateStart).TotalSeconds;
                startPause = false;
            }

            time += Time.deltaTime;
            frameCount++;

            TimeElapsed = (DateTime.Now - startTime).Subtract(TimeSpan.FromSeconds(totalSeconds));

            
            timerText.text = TimeElapsed.Hours.ToString("00") + ":" + TimeElapsed.Minutes.ToString("00") + ":" + TimeElapsed.Seconds.ToString("00");

            if (time >= pollingTime)
            {
                int frameRate = Mathf.RoundToInt(frameCount / time);
                FpsText.text = frameRate.ToString() + " FPS";

                time -= pollingTime;
                frameCount = 0;
            }
        }
        else
        {
            if (!startPause)
            {
                dateStart = DateTime.Now;
                startPause = true;
            }
        }
    }
}
