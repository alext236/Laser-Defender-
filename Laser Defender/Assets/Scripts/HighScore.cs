using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighScore : MonoBehaviour {

    public Text text;
    
    public static int highScore;

	// Use this for initialization
	void Start () {        
        UpdateText();
	}

    void UpdateText() {
        text.text = "Best: " + highScore;
    }

    public void UpdateHighScore() {
        highScore = Score.score;
        UpdateText();
    }
}
