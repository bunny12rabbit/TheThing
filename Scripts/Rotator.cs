using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{ 
    Rigidbody2D RG;
    //bool _isForward;

    // Use this for initialization
    void Start ()
    {
        RG = GetComponent<Rigidbody2D>();
        GameObject player = GameObject.Find("Player");
        //_isForward = player.GetComponent<Controller>()._isForward;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 Velocity = RG.velocity;      // Направление силы Rigidbody2D    

        float angle = Mathf.Atan2(Velocity.y, Velocity.x) * Mathf.Rad2Deg;   // Возвращаем угол в радианах тангенса y/x (направления) и переводим их в градусы
                                 
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));   // Поворачиваем		
    }
}
