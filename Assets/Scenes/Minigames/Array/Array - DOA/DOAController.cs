using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLogging;
using UnityEngine.UI;

public class DOAController : Minigame {

    [Header("Game Input")]
    public InputField Row;
    public InputField Col;
    public Button SubmitButton;

    [Header("Text Displays")]
    public Text ErrorMessage;
    public Text[] People;
    public Text TVDisplay;

    [Header("Internal Data")]
    HashSet<int> _numbers = new HashSet<int>();
    int[] Numbers;
    Queue<int> NumberQueue = new Queue<int>();

    private void Awake() {
        base.Awake();
        Row.Select();
        Log = new Log() {
            IsPractice = Zombie.IsPractice,
            IsTeamGame = !Zombie.IsSolo,
            ErrorsMade=0,
            TurnsUsed=0,
            TimeTaken=0,
        };

        Numbers = new int[16];

        int i = 0;
        while(_numbers.Count < Numbers.Length) {
            int R = Random.Range(10, 99);
            if (_numbers.Add(R)) {
                Numbers[i] = R;
                i++;
            }
        }
        Numbers.Shuffle();

        for(int j = 0; j < People.Length; j++) {
            People[j].text = Numbers[j].ToString();
            People[j].gameObject.transform.parent.GetComponent<Image>().sprite = Resources.Load<Sprite>($"DOA/Person{(Numbers[j]%8)+1}");
        }

        Numbers.Shuffle();
        for(int j = 0; j < Numbers.Length; j++) { NumberQueue.Enqueue(Numbers[j]); }
        TVDisplay.text = NumberQueue.Dequeue().ToString();

        ErrorMessage.gameObject.SetActive(false);
        SubmitButton.onClick.AddListener(Submit);
        Row.onEndEdit.AddListener(delegate { Col.Select(); });
        Col.onEndEdit.AddListener(delegate { SubmitButton.Select(); });
    }

    private void Update() {
        if (GameLoss.gameObject.activeInHierarchy) { 
            Log.Win = false;
            Log.TimeTaken = CountDown.TimeElapsed;
            Zombie.CurrentProfileStats.Stats["Array"]["Arrays Attack"].GameLog.Add(Log);
        } else if (GameWin.gameObject.activeInHierarchy) { 
            Log.Win = true;
            Log.TimeTaken = CountDown.TimeElapsed;
            Zombie.CurrentProfileStats.Stats["Array"]["Arrays Attack"].GameLog.Add(Log);
        }
    }


    void Submit() {
        if (!string.IsNullOrEmpty(Row.text) && !string.IsNullOrEmpty(Col.text)) {
            string Coord = $"R{int.Parse(Row.text)}C{int.Parse(Col.text)}";
            Debug.Log(Coord);
            Col.text = "";
            Row.text = "";
            Row.Select();
            Log.TurnsUsed++;
            foreach (Text person in People) {
                if (person.name == Coord) {
                    if (person.text == TVDisplay.text) {
                        person.gameObject.transform.parent.gameObject.SetActive(false);
                        UpdateError(Errors.None);
                        try {
                            TVDisplay.text = NumberQueue.Dequeue().ToString();
                        } catch {
                            GameWin.Show();
                            Debug.Log("Win");
                        }
                        return;
                    } else {
                        Debug.Log(Errors.Not_Correct);
                        UpdateError(Errors.Not_Correct);
                        return;
                    }
                }
            }
            Log.ErrorsMade++;
            Debug.Log(Errors.Not_Correct);
            UpdateError(Errors.Not_Correct);
        } else {
            Log.ErrorsMade++;
            UpdateError(Errors.No_Input);
            Debug.Log(Errors.No_Input);
        }
        Col.text = "";
        Row.text = "";
        Row.Select();
    }

    void UpdateError(Errors error) {
        ErrorMessage.gameObject.SetActive(error != Errors.None);
        ErrorMessage.text = "Error: " + error.ToString().Replace('_', ' ');
        if(error != Errors.No_Input && error != Errors.None) MDM.RegisterError();
    }
 
    enum Errors {
        None,
        No_Input,
        Not_Found,
        Not_Correct,
        Invalid_Input,
        Other
    }

}
