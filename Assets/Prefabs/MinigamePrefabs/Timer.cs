using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public Text TimerLabel;
    public bool AutoRun;

    void Start() {
        TimerLabel.text = "00:00.000";
        if (AutoRun) { running = true; }
    }

    public float Time { get; set; }
    public bool running { get; set; }

    public void StopTimer() { running = false; }
    public void StartTimer() { running = true; }

    protected virtual void Update() {
        if (running) {
            try {
                Time += UnityEngine.Time.deltaTime;
                float minutes = Mathf.FloorToInt(Time / 60);
                float seconds = Mathf.FloorToInt(Time % 60);
                float milliseconds = (Time % 1) * 1000;
                string timedisplay = $"{minutes,2}:{seconds,2}.{milliseconds,3:F0}";
                TimerLabel.text = timedisplay.Replace(" ", "0");
            } catch {
                running = false;
            }
        }
    }
}