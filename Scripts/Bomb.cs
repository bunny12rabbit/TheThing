using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    [SerializeField] GameObject _explosion;
    [SerializeField] float _boomTime = 3;
    [SerializeField] Mode mode;
    [SerializeField] float radius;
    [SerializeField] float power;
    [SerializeField] int damage = 2;
    [SerializeField] LayerMask layerMask;
    public AudioClip bombSound;

    enum Mode { simple, adaptive }

    // Use this for initialization
    void Start () {
        //Invoke("BOOM", _boomTime);
        //Destroy(gameObject, _boomTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            col.gameObject.GetComponent<Enemy>().Hurt(damage);
            BOOM();
            Destroy(gameObject);
        }
    }

    void BOOM()
    {
        GameObject expInst = Instantiate(_explosion, transform.position, transform.rotation);
        Destroy(expInst, 1);
        AudioSource.PlayClipAtPoint(bombSound, transform.position);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);

        foreach (Collider2D hit in colliders)
        {
            if (hit.attachedRigidbody != null)
            {
                Vector3 direction = hit.transform.position - transform.position;
                direction.z = 0;

                if (CanUse(transform.position, hit.attachedRigidbody))
                {
                    hit.attachedRigidbody.AddForce(direction.normalized * power);
                }
            }
        }
    }

    bool CanUse(Vector3 position, Rigidbody2D body)
    {
        if (mode == Mode.simple) return true; //Если режим простой, то взрывная волна не учитывает препятствия

        //Режим adaptive учитывает объекты на пути взрыва (препятствие, за которым можно спрятаться)
        RaycastHit2D hit = Physics2D.Linecast(position, body.position);
        if (hit.rigidbody != null && hit.rigidbody == body)
        {
            return true;
        }
        return false;
    }
}
