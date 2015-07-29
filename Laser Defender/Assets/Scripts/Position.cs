using UnityEngine;
using System.Collections;

public class Position : MonoBehaviour {

    //Draw gizmos on Screen view
    public void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
