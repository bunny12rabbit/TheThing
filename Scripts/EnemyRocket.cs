using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRocket : MonoBehaviour
{    
    public GameObject BOOM;
    public int Damage = 1;
    [SerializeField] float _lifeTime;
    [SerializeField] int _speed = 10;
    public AudioClip rocketSound;

    //Vector3 _dir = new Vector3(0, 0, 0);
    Rigidbody2D _myRb;

    // Use this for initialization
    void Start()
    {
        //Уничтожаем рокету через 2 секунды если никуда не попали
        Destroy(gameObject, _lifeTime);
        //_myRb = GetComponent<Rigidbody2D>();
        //GetComponent<Rigidbody2D>().AddForce(transform.right * _speed, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public void Launch(float num)
    {
        _myRb = GetComponent<Rigidbody2D>();
        _myRb.AddForce(transform.right * _speed * Mathf.Sign(num), ForceMode2D.Impulse);
    }

    void OnExplode()
    {
        // Создаем кватернион с рандомным вращением по оси Z
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        // Эффект взрыва
        GameObject boom = Instantiate(BOOM, transform.position, randomRotation);
        Destroy(boom, 2);
        AudioSource.PlayClipAtPoint(rocketSound, transform.position);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            col.gameObject.GetComponent<Controller>().Hurt(Damage);
            OnExplode();
            Destroy(gameObject);
        }
        else
            if (col.IsTouchingLayers(LayerMask.NameToLayer("Ground")))
        {
            OnExplode();
            Destroy(gameObject);
        }

    }
}
