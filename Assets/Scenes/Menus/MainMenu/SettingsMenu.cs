using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

    public AudioMixer MasterMixer;
    public Slider OST;
    public Slider SFX;

    public void Start(){
        Screen.fullScreen = false;
        MasterMixer.SetFloat("OST", Mathf.Log10(Zombie.CurrentProfileStats.OSTVolume) * 20);
        MasterMixer.SetFloat("SFX", Mathf.Log10(Zombie.CurrentProfileStats.SFXVolume) * 20);
        OST.value = Zombie.CurrentProfileStats.OSTVolume;
        SFX.value = Zombie.CurrentProfileStats.SFXVolume;
    }

    public void Update() {
        MasterMixer.SetFloat("OST", Mathf.Log10(OST.value) * 20);
        Zombie.CurrentProfileStats.OSTVolume = OST.value;
        MasterMixer.SetFloat("SFX", Mathf.Log10(SFX.value) * 20);
        Zombie.CurrentProfileStats.SFXVolume = SFX.value;
    }

    public void OnFullScreenChange(Toggle Setting) {
        Screen.fullScreen = Setting.isOn;
    }

    public void OnCredits(Animator Anim) {
        Anim.SetTrigger("Credits");
    }
}
