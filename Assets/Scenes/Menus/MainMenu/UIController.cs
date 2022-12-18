using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

    public GameObject PlayMenu;
    public GameObject SettingsMenu;

    public void OnPlay() {
        SettingsMenu.SetActive(false);
        PlayMenu.SetActive(true);
    }

    public void OnStats() {
        SceneManager.LoadScene("Stats");
    }

    public void OnOptions() {
        SettingsMenu.SetActive(true);
        PlayMenu.SetActive(false);
    }

    public void OnQuit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

}
