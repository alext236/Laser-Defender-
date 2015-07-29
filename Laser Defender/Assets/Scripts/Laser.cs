using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

    public float damage = 25f;
    public GameObject hitParticle;

    public void OnBecameInvisible() {
        Destroy(this.gameObject);        
    }

    public float GetDamage() {
        return damage;
    }

    public void Hit() {
        GameObject particle = Instantiate(hitParticle, transform.position, Quaternion.identity) as GameObject;
        Destroy(gameObject);
        Destroy(particle, particle.GetComponent<ParticleSystem>().startLifetime);
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        Hit();
    }


}
