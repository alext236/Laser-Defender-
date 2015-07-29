using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EnemySpawner : MonoBehaviour {

    public Enemy enemyPrefab;
    public float gizmoWidth;
    public float gizmoHeight;
    public float speed;
    public float spawnDelay;

    public AudioClip[] flySound;
    
    private bool goingLeft = true;  //move left on start

    // Use this for initialization
    void Start() {
        SpawnUntilFull();
    }

    // Update is called once per frame
    void Update() {
        AutomatedMovement();

        if (AllMembersDead()) {
            Debug.Log("Empty formation");
            SpawnUntilFull();
        }
    }

    private IEnumerator SpawnEnemies() {
        for (int i = 0; i < transform.childCount; i++) {
            int rand = Random.Range(0, flySound.Length);
            AudioSource.PlayClipAtPoint(flySound[rand], transform.position);

            Transform spawnPos = NextRandomFreePosition();
            Enemy enemy = Instantiate(enemyPrefab, spawnPos.position, Quaternion.identity) as Enemy;
            enemy.transform.parent = spawnPos;     //Set the created clone under EnemyFormation parent                        

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnUntilFull() {        
        StartCoroutine(SpawnEnemies());
    }

    public void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gizmoWidth, gizmoHeight, 0));
    }

    void AutomatedMovement() {
        Vector3 moveLeftRight = Vector3.right * speed * Time.deltaTime;

        if (goingLeft) {
            transform.position -= moveLeftRight;
        }
        else {
            transform.position += moveLeftRight;
        }

        CheckEndOfMovement();
    }
        
    private void CheckEndOfMovement() {
        float xMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0)).x;
        float xMax = Camera.main.ViewportToWorldPoint(new Vector3(1, 1)).x;
        //float yMin = camera.ViewportToWorldPoint(new Vector3(0, 0)).y;
        //float yMax = camera.ViewportToWorldPoint(new Vector3(1, 1)).y;

        float leftEdgeX = 0;
        float rightEdgeX = 0;

        //assign left edge and right edge to the first available position that has an enemy
        foreach (Transform child in transform) {
            if (child.transform.childCount > 0) {
                leftEdgeX = child.position.x - 0.5f;
                rightEdgeX = child.position.x - 0.5f;
                break;
            }
        }

        //assign left egde with leftmost enemy and right edge with rightmost enemy
        foreach (Transform child in transform) {
            if (child.transform.childCount == 0) {
                continue;
            }
            if (child.transform.position.x < leftEdgeX) {                
                leftEdgeX = child.transform.position.x - 0.5f;
            }

            if (child.transform.position.x > rightEdgeX) {                
                rightEdgeX = child.transform.position.x + 0.5f;
            }            
        }
        
        if (leftEdgeX <= xMin) {
            goingLeft = false;
        }
        else if (rightEdgeX >= xMax) {
            goingLeft = true;
        }
        //change direction if hit sides: left to right or vice versa
    }

    bool AllMembersDead() {
        foreach (Transform child in transform) {
            if (child.childCount > 0) {
                return false;
            }
        }

        return true;
    }

    Transform NextRandomFreePosition() {
        Transform[] array = RandomizeTransformOrder();
        foreach (Transform pos in array) {
            if (pos.childCount <= 0) {
                return pos;
            }
        }

        return null;
    }

    private Transform[] RandomizeTransformOrder() {
        Transform[] array = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++) {
            array[i] = transform.GetChild(i);
        }

        for (int i = 0; i < array.Length; i++) {
            int rand = Random.Range(0, array.Length);
            Transform temp = array[rand];
            array[rand] = array[i];
            array[i] = temp;
            //switch each element with another randomly chosen element in the array
        }
        return array;
    }
}
