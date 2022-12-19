﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class SetController : MonoBehaviour {

    [Header("Engine Data")]
    public float RobbieDifficulty = .1f;
    public GameLoss Loss;
    public GameWin Win;
    public CountDown CountDown;
    GameLogging.Log Log;
    public MinigameDifficultyModifier MDM;

    [Header("User Input")]
    public InputField PlayerInput;
    public Text SetType;
    public Text PlayerSet;

    [Header("Robbie")]
    public Animator RobbieSpeak;
    public Text RobbieTalk;
    bool RobbieFinished = false;
    public Sprite Robbie_Inactive;

    [Header("Internal Data")]
    HashSet<string> Player = new HashSet<string>();
    HashSet<string> LegalOptions;
    List<string> IllegalOptions = new List<string>();



    // Start is called before the first frame update
    void Start() {
        Log = new GameLogging.Log() {
            IsPractice = Zombie.IsPractice,
            IsTeamGame = !Zombie.IsSolo,
            ErrorsMade = 0,
            TurnsUsed = 0,
        };

        RobbieSpeak.enabled = false;
        SetItems.SetArgs GameLoad = SetItems.SelectSet();
        LegalOptions = GameLoad.Set;
        SetType.text = $"Topic: {GameLoad.Name}";

        PlayerSet.text = "";

        PlayerInput.onEndEdit.AddListener(delegate {
            PlayerMove();
        });
        PlayerInput.enabled = true;
        Loss.Message = "That's Incorrect!";
    }

    void PlayerMove() {
        if(string.IsNullOrWhiteSpace(PlayerInput.text) || string.IsNullOrWhiteSpace(PlayerInput.text)) { PlayerInput.text = ""; PlayerInput.Select(); return; }
        
        Log.TurnsUsed++;
        int Location = LegalOptions.ContainsValue(PlayerInput.text);
        
        if (Location != -1) {
            if (Player.Add(LegalOptions.ElementAt(Location))) {
                IllegalOptions.Add(LegalOptions.ElementAt(Location));
                LegalOptions.Remove(LegalOptions.ElementAt(Location));

                string Last5 = "";
                for (int i = 1; i <= 5; i++) {
                    try {
                        Last5 += $"{i} {Player.ElementAt(Player.Count - i)}\n";
                    } catch {
                        Last5 += "\n";
                    }
                }
     
                PlayerSet.text = Last5;

                Debug.Log($"Legal {PlayerInput.text}");
                PlayerInput.enabled = true;
            }
        } else {
            Log.ErrorsMade++;
            Debug.Log($"Illegal {PlayerInput.text}");
            MDM.RegisterError();
            //Loss.Show();
            //CountDown.StopTimer();
            //
        }
        float RNG = Random.value;
        if (RNG >= RobbieDifficulty) {
            try {
                int RandomIndex = Random.Range(0, LegalOptions.Count - 1);
                StartCoroutine(RobbieSpeaking(LegalOptions.ElementAt(RandomIndex)));
                IllegalOptions.Add(LegalOptions.ElementAt(RandomIndex));
                LegalOptions.Remove(LegalOptions.ElementAt(RandomIndex));
                Debug.Log($"Legal {LegalOptions.ElementAt(RandomIndex)}");
            } catch {
                Zombie.CurrentProfileStats.Stats["Set"]["Robbie's Revenge"].GameLog.Add(Log);
                Win.Show();
                CountDown.StopTimer();
            }
        } else {
            try {
                int RandomIndex = Random.Range(0, IllegalOptions.Count-1);
                StartCoroutine(RobbieSpeaking(IllegalOptions.ElementAt(RandomIndex)));
                Debug.Log($"Illegal {IllegalOptions.ElementAt(RandomIndex)}");
                Win.Show();
                CountDown.StopTimer();
                Zombie.CurrentProfileStats.Stats["Set"]["Robbie's Revenge"].GameLog.Add(Log);
            } catch {
                int RandomIndex = Random.Range(0, LegalOptions.Count - 1);
                StartCoroutine(RobbieSpeaking(LegalOptions.ElementAt(RandomIndex)));
                IllegalOptions.Add(LegalOptions.ElementAt(RandomIndex));
                LegalOptions.Remove(LegalOptions.ElementAt(RandomIndex));
                Debug.Log($"Legal {LegalOptions.ElementAt(RandomIndex)}");
            }
        }
    }

    private void Update() {
        if(Loss.isActiveAndEnabled) Zombie.CurrentProfileStats.Stats["Set"]["Robbie's Revenge"].GameLog.Add(Log);
        if (Input.GetKey(KeyCode.Return) && RobbieFinished) {
            RobbieTalk.text = "";
            RobbieFinished = false;
            RobbieTalk.transform.parent.gameObject.SetActive(false);
            RobbieSpeak.enabled = false;
            RobbieSpeak.gameObject.GetComponent<Image>().sprite = Robbie_Inactive;
            
            PlayerInput.text = "";
            PlayerInput.enabled = true;
        }
    }

    IEnumerator RobbieSpeaking(string Word) {
        RobbieTalk.text = "";
        PlayerInput.enabled = false;
        RobbieFinished = false;
        RobbieSpeak.enabled = true;
        RobbieTalk.transform.parent.gameObject.SetActive(true);
        foreach (char C in Word.ToCharArray()) {
            RobbieTalk.text += C;
            yield return new WaitForSeconds(.1f);
        }
        RobbieFinished = true;
        PlayerInput.enabled = true;
    }

}
public static class SetItems {
    
    static List<string> Sets = new List<string>();
    static Regex RegEx = new Regex("[^a-zA-Z0-9,]");

    static SetItems() {
        TextAsset[] Assets = Resources.LoadAll<TextAsset>("Set\\");
        foreach (TextAsset asset in Assets) { Sets.Add(asset.name); }
    }
    public static SetArgs SelectSet() {
        string Item = Sets[Random.Range(0, Sets.Count)];
        string[] LegalItems = Resources.Load<TextAsset>($"Set\\{Item}").text.Split(',');
        HashSet<string> Items = new HashSet<string>();
        foreach(string Legal in LegalItems) {
            Items.Add(Legal);
        }
        return new SetArgs(Item, Items);
    }

    public struct SetArgs {
        public static SetArgs Empty { get; } = new SetArgs("Null", new HashSet<string>());
        public string Name { get; }
        public HashSet<string> Set { get; }
        public SetArgs(string Name, HashSet<string> Set) { this.Name = Name; this.Set = Set; }
    }

    public static int ContainsValue(this HashSet<string> Set, string valueCheck) {
        string CleanedCheck = RegEx.Replace(valueCheck.Normalize(System.Text.NormalizationForm.FormKD),"").ToLower();

        int counter = -1;
        foreach(string Item in Set) {
            counter++;
            string cleaned = RegEx.Replace(Item.Normalize(System.Text.NormalizationForm.FormKD),"").ToLower();
            if (cleaned == CleanedCheck)
                return counter;
        }
        return -1;
    }
}