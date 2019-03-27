using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieTrigger : MonoBehaviour {

    public GameObject Player;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            col.gameObject.GetComponent<Controller>().Death();
        }
    }
}
