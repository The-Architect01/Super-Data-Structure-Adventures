
using UnityEngine;
using UnityEngine.SceneManagement;

public class Zombie : MonoBehaviour {

#if UNITY_WEBGL
    public static float TimeElapsed = 60f;
    static float TimeCounter = 0;
#endif 

    public static bool IsSolo { get; set; } = true;
    private static bool created = false;
    public static bool IsPractice { get; set; } = false;

    public static System.Collections.Generic.Dictionary<string, string[]> MinigameDB { get; } = new System.Collections.Generic.Dictionary<string, string[]>();

#if !UNITY_WEBGL
    private static string _mg;
#endif 

    public static string MiniGame {
#if !UNITY_WEBGL
        get { 
            return _mg;
        } 
        set { 
            _mg = value;

            DiscordController.ScreenName = _mg; 
        }
#else
    get; set;
#endif
    }

#if !UNITY_WEBGL
    public static DiscordController DiscordController { get; private set; }
#endif
    public static PlayerStats CurrentProfileStats { get; private set; }
    public static MiniGameLister MiniGameList { get; private set; }
    public static Difficulty Difficulty { get; set; } = Difficulty.Normal;
    private void Awake() { 
        if (!created) {
            // Application.quitting += Quitting;
            DontDestroyOnLoad(gameObject);
            created = true;

#if !UNITY_WEBGL
            DiscordController = new DiscordController();
#endif
            MiniGameList = new MiniGameLister();
            CurrentProfileStats = new PlayerStats();

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnApplicationQuit() {
#if !UNITY_WEBGL
        DiscordController.OnApplicationQuit();
#endif
        PlayerIO.SaveData(CurrentProfileStats);
    }

    private void Update() {
#if !UNITY_WEBGL
        DiscordController.Update();
#else
        TimeCounter += Time.deltaTime;
        if (TimeElapsed > TimeCounter) {
            PlayerIO.SaveData(CurrentProfileStats);
            TimeElapsed = 0;
        }
#endif
    }
}