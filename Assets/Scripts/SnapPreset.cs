using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPreset : MonoBehaviour {

    public UnityEngine.UI.Image Snap;
    public UnityEngine.UI.Image Frame;
    public UnityEngine.UI.Text Name;

    public void OnPointerEnter() {
        Frame.enabled = true;
    }

    public void OnPointerExit() {
        Frame.enabled = false;
    }

    public void OnPointerClick() {
        Zombie.MiniGame = Name.text;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Load Game");
    }

}
