using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

[Serializable]
public class MinigameDifficultyModifier {

    public Timer Timer;
    public Image[] Hearts = new Image[4];
    int DMG;

    public void OnStart() {
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
    public void OnUpdate(Minigame me) {
        int i = 0;
        foreach (Image image in Hearts)
            if (!image.enabled) i++;
        if (i == Hearts.Length) {
            me.Log.Win = false;
            me.Save();
            me.GameLoss.Message = "You Lost!";
            me.GameLoss.Show();
            me.CountDown.StopTimer();
        }
    }

    public void RegisterError() {
        DMG++;
        for (int i = 0; i < DMG; i++)
            Hearts[i].enabled = false;
    }
}
