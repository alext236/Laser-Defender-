using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusController : MonoBehaviour {

    //Define more status here if necessary
    public Status shieldUI;
    public Status fireBoostUI;

    public void ActivateStatus(Status status) {
        Vector3 spawnPos = new Vector3();
        if (transform.childCount < 1) { //no any other status            
            spawnPos.x = transform.position.x;
            spawnPos.y = transform.position.y;
        }
        else {  //another status in effect                       
            Vector3 lastChildPos = transform.GetChild(transform.childCount - 1).transform.position;

            spawnPos.x = lastChildPos.x + status.GetComponent<RectTransform>().rect.width + 5;
            spawnPos.y = lastChildPos.y;
            //take position of the last child to determine the spawn position for the next status            
        }

        Status obj = Instantiate(status, spawnPos, Quaternion.identity) as Status;
        obj.transform.SetParent(transform);        
        
        RemoveStatus(obj);
    }

    public void RemoveStatus(Status status) {
        Destroy(status.gameObject, status.Duration);
    }
}
