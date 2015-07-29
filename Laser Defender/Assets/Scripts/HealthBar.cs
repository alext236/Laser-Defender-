using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

    private float scale;

    private RectTransform rectTransform;    

    // Use this for initialization
    void Start() {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update() {
        UpdateHealthBar();
    }

    //TODO: think if possible to add animation to make health bar gradually decrease
    void UpdateHealthBar() {
        PlayerController player = FindObjectOfType<PlayerController>();
        scale = player.CurrentHealth / player.MaxHealth;
        scale = Mathf.Clamp(scale, 0f, 1f);
        rectTransform.localScale = new Vector3(scale, rectTransform.localScale.y);
    }


}
