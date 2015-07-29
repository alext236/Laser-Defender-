using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour, IPowerup {

    public Status shieldUI;
    public float dropChance;
    public AudioClip sound;
    public GameObject protectedWave;

    private PlayerController player;
    private StatusController statusController;

    // Use this for initialization
    void Start() {
        player = FindObjectOfType<PlayerController>();
        statusController = FindObjectOfType<StatusController>();
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<PlayerController>()) {
            //trigger shield powerup effect
            AudioSource.PlayClipAtPoint(sound, transform.position);
            SpawnShield();
            statusController.ActivateStatus(shieldUI);

            Destroy(gameObject);
            //Destroy the item sprite when touched
            
        }
    }

    public float GetDropChance() {
        return dropChance;
    }

    public void SpawnShield() {
        GameObject obj = Instantiate(protectedWave, player.transform.position, Quaternion.identity) as GameObject;
        obj.transform.parent = player.transform;

        Destroy(obj, shieldUI.Duration);
    }
}
