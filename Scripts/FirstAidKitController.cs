using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAidKitController : MonoBehaviour {

    [SerializeField] int healAmount = 1; 
    Collider2D _trigger;

	// Use this for initialization
	void Start () {
        _trigger = GetComponent<Collider2D>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Controller>().Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
