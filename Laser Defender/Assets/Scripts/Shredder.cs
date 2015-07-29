using UnityEngine;
using System.Collections;

public class Shredder : MonoBehaviour {

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<Laser>()) {
            Destroy(collision.gameObject);
        } 
        //else if (collision.gameObject.GetComponent(typeof(IPowerup)) ) {
        //    Destroy(collision.gameObject);
        //}
    }
}
