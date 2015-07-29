using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float maxHealth = 150f;
    public float speed;
    public float edgePadding;       //so the ship does not move out of the edge    
    public float shotSpeed;
    public float shotRatePerSec;    //how many shots fired in one second       

    public Vector3 shotOriginOffset;
    public Laser projectile;
    public AudioClip shotSound;
    public GameObject explosionParticle;
    public AudioClip explosionSound;
    public GameObject protectedWave;

    public int life;

    private Vector3 originalPos;
    private float currentHealth;
    private bool unmovable = false;

    public void Start() {
        originalPos = transform.position;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update() {
        KeyboardMovement();
        //MouseRotation();
        if (!unmovable) {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
                InvokeRepeating("Shoot", 0.00001f, 1f / shotRatePerSec);
            }
            else if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0)) {
                CancelInvoke("Shoot");
            }
        }
    }

    public float MaxHealth {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    public float CurrentHealth {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    void KeyboardMovement() {
        //movement change depends on speed given
        Vector3 moveLeftRight = Vector3.right * speed * Time.deltaTime;
        //Vector3 moveUpDown = Vector3.up * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A)) {
            transform.position -= moveLeftRight;
        }

        if (Input.GetKey(KeyCode.D)) {
            transform.position += moveLeftRight;
        }
        //Temporarily disable moving up and down
        //if (Input.GetKey(KeyCode.S)) {
        //    transform.position -= moveUpDown;
        //}

        //if (Input.GetKey(KeyCode.W)) {
        //    transform.position += moveUpDown;
        //}

        RestrictPlayerPosition();
    }

    private void RestrictPlayerPosition() {
        Vector3 limitPos = transform.position;
        Camera camera = Camera.main;

        float xMin = camera.ViewportToWorldPoint(new Vector3(0, 0)).x + edgePadding;
        float xMax = camera.ViewportToWorldPoint(new Vector3(1, 1)).x - edgePadding;
        float yMin = camera.ViewportToWorldPoint(new Vector3(0, 0)).y + edgePadding;
        float yMax = camera.ViewportToWorldPoint(new Vector3(1, 1)).y - edgePadding;

        limitPos.x = Mathf.Clamp(limitPos.x, xMin, xMax);
        limitPos.y = Mathf.Clamp(limitPos.y, yMin, yMax);
        //this is a better way than directly giving values for xMin, xMax etc for when we change sprite size
        //we only need to adjust the edge padding variable

        transform.position = limitPos;
    }

    void MouseRotation() {
        Vector3 targetPos = Input.mousePosition;        //this is in pixels
        Vector3 currentPos = Camera.main.WorldToScreenPoint(transform.position);
        //so both mouse position and player position are calculated in pixels

        targetPos.x -= currentPos.x;
        targetPos.y -= currentPos.y;
        float angle = -Mathf.Atan2(targetPos.x, targetPos.y) * Mathf.Rad2Deg;
        //draw the goddamned triangle if don't understand this part

        transform.rotation = Quaternion.Euler(0, 0, angle);

    }

    void Shoot() {
        Laser shot = Instantiate(projectile, transform.position + shotOriginOffset, Quaternion.identity) as Laser;
        shot.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, shotSpeed);
        AudioSource.PlayClipAtPoint(shotSound, shot.transform.position, 0.5f);    //sound
        //TODO: think about how to determine x for vector3 so that its direction is the same with player (may not need)
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (!unmovable) {
            Laser laserShot = collision.gameObject.GetComponent<Laser>();
            if (laserShot) {
                currentHealth -= laserShot.GetDamage();
                laserShot.Hit();
            }

            if (currentHealth <= 0) {
                Explode();
                if (life >= 0) {                    
                    Invoke("Respawn", 1.5f);
                }
                else {
                    Invoke("GameOver", 2f);
                }
            }
        }

    }
    
    private void Explode() {
        if (!unmovable) {
            StopShooting();           //make sure cannot shoot after exploded
            unmovable = true;         //so collision won't happen
            MinusLife();
            SpawnParticle();
        }
    }

    public void StopShooting() {
        CancelInvoke("Shoot");
    }

    public void ResumeShooting() {
        
            InvokeRepeating("Shoot", 0.00001f, 1f / shotRatePerSec);
        
    }

    private void SpawnParticle() {
        GameObject particle = Instantiate(explosionParticle, transform.position, Quaternion.identity) as GameObject;
        AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        Destroy(particle, 3f);
    }

    public int GetLife() {
        return life;
    }

    //for use with Powerup Item
    public void AddLife() {
        life += 1;
        LifeUI lifeController = FindObjectOfType<LifeUI>() as LifeUI;
        lifeController.AddLife();
    }

    public void MinusLife() {
        life -= 1;
        
        HideSprite();
        LifeUI lifeController = FindObjectOfType<LifeUI>() as LifeUI;
        lifeController.MinusLife();

    }

    private void HideSprite() {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().enableEmission = false;
    }

    private void UnhideSprite() {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().enableEmission = true;
    }

    private void Respawn() {
        unmovable = false;
        transform.position = originalPos;
        currentHealth = maxHealth;
        UnhideSprite();
        SpawnShield(2f);
    }

    private void GameOver() {
        LevelManager lvl = FindObjectOfType<LevelManager>();
        lvl.LoadNextLevel();        
    }

    
    public void SpawnShield(float flashCount) {
        GameObject obj = Instantiate(protectedWave, transform.position, Quaternion.identity) as GameObject;
        obj.transform.parent = transform;
        //blink when respawned
        //for (int i = 0; i < flashCount; i++) {
        //    //GetComponent<SpriteRenderer>().color = Color.clear;
        //    yield return new WaitForSeconds(0.5f);
        //    //GetComponent<SpriteRenderer>().color = Color.white;
        //    yield return new WaitForSeconds(0.5f);
        //}

        Destroy(obj, flashCount);
    }
}
