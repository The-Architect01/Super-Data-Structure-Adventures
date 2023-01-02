using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Back : MonoBehaviour {

    public string PreviousScene = "Main Menu";

    public void OnEnter() {
        GetComponentInChildren<Text>().fontStyle = FontStyle.Italic;
    }

    public void OnExit() {
        GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;
    }

    public void OnClick() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(PreviousScene);
    }

}
