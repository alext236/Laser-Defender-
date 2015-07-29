using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    void Start() {
        if (Application.loadedLevelName == "Start") {
            Score.ResetScore();
        }
    }

    public void LoadLevel(string name) {
        Debug.Log("Level load requested for " + name);
        Application.LoadLevel(name);
    }

    public void QuitRequest() {
        Debug.Log("Quit the game");
        Application.Quit();
    }

    public void LoadNextLevel() {
        Application.LoadLevel(Application.loadedLevel + 1);
    }


}
