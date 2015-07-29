using UnityEngine;
using System.Collections;

public class FireBoost : MonoBehaviour, IPowerup {

    public Status fireBoostUI;
    public float increaseRate = 0.3f;
    public float dropChance;
    public AudioClip sound;

    private PlayerController player;
    private StatusController statusController;
    private float originalFireRate = 0f;

    // Use this for initialization
    void Start() {
        player = FindObjectOfType<PlayerController>();
        statusController = FindObjectOfType<StatusController>();
        if (originalFireRate == 0f) {
            originalFireRate = player.shotRatePerSec;
        }        
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<PlayerController>()) {
            AudioSource.PlayClipAtPoint(sound, transform.position);
            StartCoroutine(ActivateBoost());
            statusController.ActivateStatus(fireBoostUI);
            
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            //give the impression that the item disappears when touched
            Destroy(gameObject, fireBoostUI.Duration + 0.5f); 
            //this needs to be destroyed only after the coroutine is finished
        }
    }

    private IEnumerator ActivateBoost() {
        player.StopShooting();
        player.shotRatePerSec *= (1 + increaseRate);
        Debug.Log("shot rate increased");
        player.ResumeShooting();

        yield return new WaitForSeconds(fireBoostUI.Duration);

        Debug.Log("shot rate reverted");
        player.StopShooting();
        player.shotRatePerSec = originalFireRate;
        player.ResumeShooting();
    }

    public float GetDropChance() {
        return dropChance;
    }

}
