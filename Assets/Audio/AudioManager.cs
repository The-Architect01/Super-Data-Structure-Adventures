using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource BGM;

    void Awake() {
        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("GameMusic");
        if (musicObj.Length > 1) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeBGM(AudioClip music) {
        if (BGM.clip.name != music.name) { 
            BGM.Stop();
            BGM.clip = music;
            BGM.Play();
        }
    }


}
