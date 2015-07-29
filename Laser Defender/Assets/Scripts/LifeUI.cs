using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LifeUI : MonoBehaviour {

    private int numberOfLife;
    private List<GameObject> lifeList;

    public GameObject lifePrefab;

    public void Awake() {
        lifeList = new List<GameObject>();
        PlayerController player = FindObjectOfType<PlayerController>();
        numberOfLife = player.GetLife();
    }

    // Use this for initialization
    void Start() {
        spawnLifeUI();

    }

    void spawnLifeUI() {
        for (int i = 0; i < numberOfLife; i++) {
            //basically try to spawn at the top left, with each sprite next to each other
            Vector3 spawnPos = new Vector3();
            RectTransform lifeRectTransform = lifePrefab.GetComponent<RectTransform>();
            if (i == 0) {
                spawnPos.x = lifeRectTransform.rect.width * lifeRectTransform.localScale.x;
                spawnPos.y = Screen.height - lifeRectTransform.rect.height * lifeRectTransform.localScale.y;
            }
            else {
                //spawn next to the previous image on the right
                spawnPos.x = lifeList[i - 1].transform.position.x + lifeRectTransform.rect.width * lifeRectTransform.localScale.x;
                spawnPos.y = lifeList[i - 1].transform.position.y;
            }

            SpawnLifeImage(spawnPos);
        }
    }

    private void SpawnLifeImage(Vector3 spawnPos) {
        GameObject lifeObj = Instantiate(lifePrefab, spawnPos, Quaternion.identity) as GameObject;
        lifeObj.transform.SetParent(GameObject.Find("Life").transform);
        lifeList.Add(lifeObj);
    }

    public void MinusLife() {
        if (lifeList.Count > 0) {
            Destroy(lifeList[lifeList.Count - 1]);
            lifeList.RemoveAt(lifeList.Count - 1);
        }        
        //Remove the destroyed element
    }

    //TODO: for boost or upgrade stuff 
    public void AddLife() {
        Vector3 spawnPos = new Vector3();
        spawnPos.x = lifeList[lifeList.Count - 1].transform.position.x + lifePrefab.GetComponent<RectTransform>().rect.width * lifePrefab.GetComponent<RectTransform>().localScale.x;
        spawnPos.y = lifeList[lifeList.Count - 1].transform.position.y;

        SpawnLifeImage(spawnPos);
    }
}
