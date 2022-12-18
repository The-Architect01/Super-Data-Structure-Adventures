using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour {

    public void OnPractice() {
        SceneManager.LoadScene("Practice");
    }

    public void OnHost() {
        SceneManager.LoadScene("MM_Host");
    }

    public void OnJoin() {
        string JoinCode = GameObject.FindGameObjectWithTag("JoinCode").GetComponent<InputField>().text;
        Debug.Log(JoinCode);
        GameObject.FindGameObjectWithTag("ErrorMessage").GetComponent<Text>().enabled = !GameObject.FindGameObjectWithTag("ErrorMessage").GetComponent<Text>().enabled;
    }

}
