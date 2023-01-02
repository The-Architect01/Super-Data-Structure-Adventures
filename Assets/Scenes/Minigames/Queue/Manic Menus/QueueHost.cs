
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueueHost : Minigame {

    [Header("Queue")]
    public List<QueueItem> Queue = new List<QueueItem>();
    public static int TurnsTaken = 0;
    public Text Correct;

    private void Start() {
        base.Start();
        Log = new GameLogging.Log() {
            IsPractice = Zombie.IsPractice,
            IsTeamGame = !Zombie.IsSolo,
            TurnsUsed = 0,
        };

        TurnsTaken = 0;
        Correct.text = Food.generateItems(8);
        string[] Foods = Correct.text.Split('\n');
        Foods.Shuffle();
        try {
            int i = 0;
            foreach(QueueItem Item in Queue) {
                Item.GetComponent<Text>().text = Foods[i];
                i++;
            }
        } catch { }
    }

    public void OnClick() {
        if (QueueItem.Selected == null) { return; }
        if (Queue.Contains(QueueItem.Selected)) { return; }
        TurnsTaken++;
        Log.TurnsUsed++;
        Queue.Add(QueueItem.Selected);
        int LocationAdded = 0;

        foreach (QueueItem TP in Queue) {
            LocationAdded++;
        }

        if (QueueItem.Selected.Host != null) {
            QueueItem.Selected.Host.Queue.Remove(QueueItem.Selected);
            QueueItem.Selected.Host.UpdateTree();
        }

        QueueItem.Selected.Host = this;

        try {
            for (int i = 0; i < QueueItem.Selected.Host.Queue.Count; i++) {
                QueueItem.Selected.Host.Queue[i].IsSelectable = i == 0;
            }
        } catch { Debug.Log("Empty Tower"); MDM.RegisterError(); }

        QueueItem.Selected.LastLegalLocation = QueueItem.Selected.GetComponent<RectTransform>().anchoredPosition = new Vector3(
            GetComponent<RectTransform>().anchoredPosition.x + 25, 
            (10) - 60 * (Queue.Count-1), 
            0
        );
        QueueItem.Selected.IsSelectable = (Queue.IndexOf(QueueItem.Selected) == 0);
        
        QueueItem.Selected = null;
        Check();
    }

    public void UpdateTree() {
        try {
            for (int i = 0; i < Queue.Count; i++) {
                Queue[i].IsSelectable = i == 0;
                Queue[i].LastLegalLocation =
Queue[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(
                    GetComponent<RectTransform>().anchoredPosition.x + 25,
                    (10) - 60 * i,
                    0
                );
            }
        } catch { Debug.Log("Empty Tower"); MDM.RegisterError(); }
    }


    void Check() {
        string Answer = "";
        foreach(QueueItem item in Queue) {
            Answer += item.GetComponent<Text>().text + "\n";
        }
        if(Answer.Replace("\n",string.Empty) == Correct.text.Replace("\n",string.Empty)) {
            Win();
            //WinGame.Show();
            //Zombie.CurrentProfileStats.Stats["Queue"]["Manic Menus"].GameLog.Add(Log);
        }
    }

}

public static class Food {

    static string[] FoodItems { get; } = {
        "Sandwich",
        "Steak",
        "Pudding",
        "Pizza",
        "Pie",
        "Fish",
        "Hamburger",
        "Ice Cream"
    };

    //Makes the food readable
    public static string generateItems(int items) {
        FoodItems.Shuffle();
        string end = "";
        bool start = true;
        foreach (string item in FoodItems) {
            if (start) { end += item; start = false; } else { end += "\n" + item; }
        }

        return end;
    }

}