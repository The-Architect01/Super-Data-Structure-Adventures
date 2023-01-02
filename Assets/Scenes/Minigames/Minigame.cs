using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using GameLogging;

public class Minigame : MonoBehaviour {

    [Header("Minigame Data")]
    public string DataStructure;
    public string Name;
    public MiniGameData Data;

    [Header("Engine Controllers")]
    public GameWin GameWin;
    public GameLoss GameLoss;
    public CountDown CountDown;
    public MinigameDifficultyModifier MDM;
    public Log Log;

    bool HasSavedLog = false;

    public void Awake() {
        Data.OnAwake(gameObject);
    }

    public void Start() {
        MDM.OnStart();
    }

    public void LateUpdate() {
        MDM.OnUpdate(this);
    }

    protected void Win() {
        Log.Win = true;
        GameWin.Show();
        Save();
    }
    protected void Lose() {
        Log.Win = false;
        GameLoss.Show();
        Save();
    }

    public void Save() {
        if (!HasSavedLog) {
            Log.Date = System.DateTime.Now;
            Log.IsPractice = true;
            Log.TimeTaken = CountDown.TimeElapsed;
            Zombie.CurrentProfileStats.Stats[DataStructure][Name].GameLog.Add(Log);
        }
    }
}

public static class MinigameExtensions {
    
    //Allows an array to be shuffled.
    public static void Shuffle<T>(this T[] list) {
        int n = list.Length;
        while (n > 1) {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

}