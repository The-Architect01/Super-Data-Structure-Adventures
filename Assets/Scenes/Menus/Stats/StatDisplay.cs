using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class StatDisplay : MonoBehaviour {

    public Dropdown Filter;
    public Text PageDisplay;
    public Text Overview;
    public Button Export;
    int CurPage = 1;
    int MaxPage = 1;
    public LogDisplay[] Stats = new LogDisplay[7];

    public void Start() {
        Filter.ClearOptions();
        string[] minigames = Zombie.MiniGameList.GetCurrentScenes();
        Filter.AddOptions(minigames.ToList());
        Filter.value = 0;
        CurPage = 1;
        OnUpdate();
        Export.onClick.AddListener(delegate { PlayerIO.ExportData(Zombie.CurrentProfileStats); });
        for (int i = 0; i < Stats.Length; i++) {
            try {
                Stats[i].Populate(Zombie.CurrentProfileStats.Stats[Zombie.MiniGameList.GetCategoryFromSceneName(Filter.options[Filter.value].text)][Filter.options[Filter.value].text].GameLog[(i + ((CurPage - 1) * 7))]);
            } catch {
                Stats[i].Populate();
            }
        }
#if UNITY_WEBGL	
        Export.gameObject.SetActive(false);
#endif 
    }

    public void OnUpdate() {
        string c = Zombie.MiniGameList.GetCategoryFromSceneName(Filter.options[Filter.value].text);
        GameLogging.Log[] logs = Zombie.CurrentProfileStats.Stats[c][Filter.options[Filter.value].text].GameLog.ToArray();
        MaxPage = (logs.Length / 7) + 1;
        Debug.Log(MaxPage);
        Stat s = Zombie.CurrentProfileStats.Stats[c][Filter.options[Filter.value].text];
        float min = float.MaxValue;
        float avg = 0;
        foreach(GameLogging.Log log in logs) {
            if (log.TurnsUsed < min) min = log.TurnsUsed;
            avg += log.TurnsUsed;
        }
        if (avg == float.NaN) avg = -1;
        if (min == float.MaxValue) min = -1;
        try {
            Overview.text = $"Best Time: {s.BestTime}\n" +
                            $"Games Played: {s.GamesPlayed}\n" +
                            $"Games Won: {s.GamesWon}\n" +
                            $"Games Lost: {s.GamesLost}\n" +
                            $"Online Games: {0/*s.TimesPlayed - s.TimesPracticed*/}\n" +
                            $"Practice Games: {s.TimesPracticed}\n" +
                            $"Avg. # of Moves: {(avg / logs.Length).ToString("###.##")}\n" +
                            $"Min. # of Moves: {min,3}";
        } catch {
            Overview.text = "Best Time: 00:00.000\nGames Played: 0\nGames Won: 0\nGames Lost: 0\nOnline Games: 0\nPractice Games: 0\nAvg. # of Moves: 0\nMin. # of Moves: 0";
        }
        UpdateDisplay();
    }

    public void OnLeftArrow() {
        CurPage--;
        if (CurPage <= 0) CurPage = MaxPage;
        OnUpdate();
    }

    public void UpdateDisplay() {
        PageDisplay.text = $"Page {CurPage}";
        for (int i = 0; i < Stats.Length; i++) {
            try {
                Stats[i].Populate(Zombie.CurrentProfileStats.Stats[Zombie.MiniGameList.GetCategoryFromSceneName(Filter.options[Filter.value].text)][Filter.options[Filter.value].text].GameLog[(i+((CurPage-1)*7))]);
            } catch {
                Stats[i].Populate(null);
            }
        }
    }

    public void OnRightArrow() {
        CurPage++;
        if (CurPage >= MaxPage) CurPage = 1;
        OnUpdate();
    }

}
