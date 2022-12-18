using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

    public AudioMixer MasterMixer;

    private void Start() {
        Screen.fullScreen = false;
        MasterMixer.SetFloat("OST", Mathf.Log10(.8f) * 20);
        MasterMixer.SetFloat("SFX", Mathf.Log10(.8f) * 20);
    }

    public void OnOSTChange(Slider Value) {
        MasterMixer.SetFloat("OST", Mathf.Log10(Value.value) * 20);
    }

    public void OnSFXChange(Slider Value) {
        MasterMixer.SetFloat("SFX", Mathf.Log10(Value.value) * 20);
    }

    public void OnFullScreenChange(Toggle Setting) {
        Screen.fullScreen = Setting.isOn;
    }

    public void OnCredits() {
        Debug.Log("Credits Pressed");
    }
}
