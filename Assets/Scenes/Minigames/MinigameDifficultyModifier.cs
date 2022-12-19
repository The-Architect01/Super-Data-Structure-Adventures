using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MinigameDifficultyModifier : MonoBehaviour {

    public Timer Timer;
    public GameLoss GameOver;
    public Image[] Hearts = new Image[4];
    int DMG;

    public void Start() {
        Timer.Time = 120 - (30 * ((int)Zombie.Difficulty));
        float minutes = Mathf.FloorToInt(Timer.Time / 60);
        float seconds = Mathf.FloorToInt(Timer.Time % 60);
        float milliseconds = (Timer.Time % 1) * 1000;
        Timer.TimerLabel.text = $"{minutes,2}:{seconds,2}.{milliseconds,3:F0}".Replace(" ", "0");
        Hearts = Hearts.Reverse().ToArray();
        for(int i = 0; i<(int)Zombie.Difficulty; i++)
            Hearts[i].enabled = false;
        DMG = (int)Zombie.Difficulty;
    }
    public void Update() {
        int i = 0;
        foreach (Image image in Hearts)
            if (!image.enabled) i++;
        if (i == Hearts.Length) {
            GameOver.Message = "You Lost!";
            GameOver.Show();
            Timer.StopTimer();
        }
    }

    public void RegisterError() {
        DMG++;
        for (int i = 0; i < DMG; i++)
            Hearts[i].enabled = false;
    }
}
