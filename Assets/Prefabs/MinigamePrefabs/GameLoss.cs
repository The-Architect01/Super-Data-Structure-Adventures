//Written by The-Architect01
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameLoss : MonoBehaviour
{
    #region Public Variables
    public Image Screen;
    public TextMeshProUGUI Loss;
    public TextMeshProUGUI LossText;

    public Button Retry;
    public Button Next;

    public float DisplayDelay = .02f;
    string _Message = "Times Up!";
    public string Message {
        get { return _Message; }
        set { _Message = value; Loss.text = Message; }
    }
    public bool DetectWin { get; set; } = false;
    #endregion
    private CanvasGroup group;

    //Hides the screen
    private void Awake() {
        Loss.text = Message;
        Screen.gameObject.SetActive(false);
        LossText.gameObject.SetActive(false);
        Loss.gameObject.SetActive(false);
        group = GetComponent<CanvasGroup>();
        group.alpha = 0f;
    }

    private void Start() {
        if (!Zombie.IsSolo) { Retry.enabled = false; Next.transform.position = new Vector3(0f, -250f, 0f); }
        Retry.onClick.AddListener(Retry_OnClick);
        Next.onClick.AddListener(Next_OnClick);
    }

    //Shows the screen
    public void Show() {
        DetectWin = true;
        Invoke(nameof(ShowGameOver), DisplayDelay);
    }

    //Makes it look nice
    private void ShowGameOver() {
        float alphacounter = 0f;
        Screen.gameObject.SetActive(true);
        LossText.gameObject.SetActive(true);
        Loss.gameObject.SetActive(true);
        while (group.alpha < 1f) {
            alphacounter += .001f;
            group.alpha = alphacounter;
        }
    }

    private void Retry_OnClick() {
        SceneManager.LoadScene(gameObject.scene.name);
    }
    private void Next_OnClick() {
        SceneManager.LoadScene("SPCategoryMenu");
        Debug.Log("Next Clicked");
    }
}
