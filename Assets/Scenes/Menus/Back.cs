using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Back : MonoBehaviour {
    
    public void OnEnter() {
        GetComponent<Text>().fontStyle = FontStyle.Italic;
    }

    public void OnExit() {
        GetComponent<Text>().fontStyle = FontStyle.Normal;
    }

    public void OnClick() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }

}
