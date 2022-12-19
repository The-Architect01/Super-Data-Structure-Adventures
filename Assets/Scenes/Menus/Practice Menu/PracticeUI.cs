using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PracticeUI : MonoBehaviour {

    [Header("Difficulty Settings")]
    public Dictionary<Difficulty, string> DifficultyDesc = new Dictionary<Difficulty, string>(){
        { Difficulty.Easy, "In easy difficulty, you have more time to complete the minigame and 4 hearts." },
        { Difficulty.Normal, "In normal difficulty, you have the default time to complete the minigame and 3 hearts." },
        { Difficulty.Hard, "In hard difficulty, you have less time to complete the minigame and 2 hearts." },
        { Difficulty.Expert, "In expert difficulty, you have a lot less time to complete the minigame and 1 heart." }
    };

    public Slider DifficultySlider;
    public Text DifficultyText;
    public Text DifficultyHeader;

    [Header("Filter Settings")]
    public Dropdown FilterDropDown;
    public SnapPreset[] Snaps;

    public void Start() {
        FilterDropDown.ClearOptions();
        FilterDropDown.AddOptions(new List<string>() { "All" }.Union(Zombie.MiniGameList.GetListofCategories()).ToList());
        Snaps = FindObjectsOfType<SnapPreset>();
    }

    public void OnSliderChange() {
        Difficulty d = (Difficulty)DifficultySlider.value;
        DifficultyText.text = DifficultyDesc[d];
        DifficultyHeader.text = $"Difficulty: {d}";
        Zombie.Difficulty = d;
    }

    public void OnCatChange() {
        foreach (SnapPreset snapPreset in Snaps) {
            if(FilterDropDown.options[FilterDropDown.value].text == "All") { snapPreset.gameObject.SetActive(true); continue; }
            snapPreset.gameObject.SetActive(Zombie.MiniGameList.GetCategoryFromSceneName(snapPreset.Name.text) == FilterDropDown.options[FilterDropDown.value].text);
        }
    }

   
}
