using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MultiPlayerMenu : MonoBehaviour
{
    public Button OpenMultiplayerMenu;

    // Start is called before the first frame update
    void Start()
    {
        OpenMultiplayerMenu.onClick.AddListener(delegate {
            SceneManager.LoadScene("Main Menu");
            //    SceneManager.LoadScene("mainScene",LoadSceneMode.Single);
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
