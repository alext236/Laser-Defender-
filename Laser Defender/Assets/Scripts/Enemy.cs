using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public float health = 100f;    
    public float shotSpeed;
    public float ShotRatePerSec;
    public bool idleState = false;

    public Laser projectile;
    public Vector3 shotOriginOffset;
    public AudioClip shotSound;
    public AudioClip explosionSound;
    public GameObject explosionParticle;

    public GameObject[] drops;

    private int scoreValue;

    public void Start() {
        RandomArrivalAnim();
        scoreValue = Random.Range(5, 15);
    }

    private void RandomArrivalAnim() {
        Animator anim = GetComponent<Animator>();
        int rand = Random.Range(1, 3);  //max not inclusive??
        string state = string.Format("EnemyArrival_0{0}", rand);
        Debug.Log(state);

        anim.Play(state);
    }

    void Update() {
        //enemies only shoot if they are already in idle state (i.e. not arriving)
        if (idleState) {
            float probability = ShotRatePerSec * Time.deltaTime;
            if (Random.value < probability) {
                Shoot();
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        //enemies are invincible until they are in idle state
        if (idleState) {
            Laser laserShot = collision.gameObject.GetComponent<Laser>();
            if (laserShot) {
                health -= laserShot.GetDamage();
                laserShot.Hit();
            }

            if (health <= 0) {
                Explode();
                SpawnDrop();
                Score scoreKeeper = GameObject.Find("Score").GetComponent<Score>();
                scoreKeeper.AddScore(scoreValue);
                //TODO: tweak here about the score
            }
        }
        //Collision differentiation is handled with layers
    }

    private void Explode() {
        Destroy(gameObject);
        GameObject particle = Instantiate(explosionParticle, transform.position, Quaternion.identity) as GameObject;
        AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        Destroy(particle, particle.GetComponent<ParticleSystem>().startLifetime);
    }

    void Shoot() {
        Laser shot = Instantiate(projectile, transform.position + shotOriginOffset, Quaternion.identity) as Laser;
        shot.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, -shotSpeed);
        AudioSource.PlayClipAtPoint(shotSound, shot.transform.position, 0.5f);    //sound        
    }

    //Doesn't seem like good code ...
    void SpawnDrop() {
        Debug.Log("Start spawn drop");
        int rand = Random.Range(0, drops.Length);
        GameObject item = drops[rand];

        //If item is a fire boost
        if (item.GetComponent<FireBoost>()) {
            if (Random.value < item.GetComponent<FireBoost>().GetDropChance()) {
                Debug.Log("Instantiated");
                Instantiate(item, transform.position, Quaternion.identity);
            }
        }
        //if item is a shield
        else if (item.GetComponent<Shield>()) {
            if (Random.value < item.GetComponent<Shield>().GetDropChance()) {
                Debug.Log("Instantiated");
                Instantiate(item, transform.position, Quaternion.identity);
            }
        }
    }
}
