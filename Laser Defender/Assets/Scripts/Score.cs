using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    private Text text;    
    public static int score = 0;

	// Use this for initialization
	void Start () {
        text = GameObject.Find("Score").GetComponent<Text>();
        UpdateText();
	}
	
    public void AddScore(int points) {
        score += points;
        UpdateText();
        if (score > HighScore.highScore) {
            FindObjectOfType<HighScore>().UpdateHighScore();
        }
    }

    public static void ResetScore() {
        score = 0;        
    }

    void UpdateText() {
        text.text = score.ToString();
    }
}
