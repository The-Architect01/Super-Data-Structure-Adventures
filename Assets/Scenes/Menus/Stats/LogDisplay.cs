using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogDisplay : MonoBehaviour {

    public Text Win;
    public Text Turns;
    public Text Practice;
    public Text Multiplayer;
    public Text Errors;
    public Text Time;

    public void Populate(GameLogging.Log Stat) {
        try {
            Win.text = Stat.Win.ToString();
            Turns.text = Stat.TurnsUsed.ToString();
            Practice.text = Stat.IsPractice.ToString();
            Multiplayer.text = Stat.IsTeamGame.ToString();
            Errors.text = Stat.ErrorsMade.ToString();
            Time.text = Stat.TimeTaken.ToString("00:00.000");
        } catch {
            Win.text = Turns.text = Practice.text = Multiplayer.text = Errors.text = Time.text = "";
        }
    }
    public void Populate() {
        Win.text = Turns.text = Practice.text = Multiplayer.text = Errors.text = Time.text = "";
    }

}
