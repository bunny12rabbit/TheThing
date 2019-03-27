using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] protected float HP = 2, damage; //Жизнь и урон
    [SerializeField] GameObject startBullet;
    [SerializeField] protected EnemyRocket rocket;
    [SerializeField] float minDistance = 7, speed = 3, reloadTime = 2, lostDistance = 8;  // Дистанция на которой атакует, скорость, время перезарядки и поле зрения.
    [SerializeField] LayerMask layerMask;
    [SerializeField] LayerMask layerMaskCol;
    public GameObject firstAid;
    public ParticleSystem _psHit;
    public ParticleSystem _psDeath;
    public AudioClip shootSound;
    AudioSource _as;
    Collider2D[] colliders;
    protected GameObject target; // Цель (помещаем туда игрока, когда тот подходит).
    protected float _health;
    protected bool _isForward = true, _couldown = false; // Проверка смотрит ли вперед (направо) и проверка идёт ли сейчас перезарядка.
    Transform myTrans;
    bool _angry; // Проверка сагрили ли мы противника
    Rigidbody2D _myRb;
    bool _dead = false;
    int _dir = 1;
    Vector3 _baseTrans;
    bool _grounded = false;
    bool _baseTransformRO = false; //Переключатель ReadOnly переменной _baseTransform, защита от изменений
    float _offset = 1.5f;
    float _groundDis;
    Vector3 _gCheck;
    bool _isMoving = false;
    Vector3 _lastPos;
    bool _isHome = false;
    bool _spawned = false;

    // Use this for initialization
    void Start()
    {
        _myRb = GetComponent<Rigidbody2D>();
        myTrans = GetComponent<Transform>();
        _health = HP;
        _lastPos = transform.position;
        _as = GetComponent<AudioSource>();
    }

    void LateUpdate()
    {
        //Проверяем на возможное падение когда движемся, если да, то возвращаемся на базу
        if (transform.position != _lastPos)
            _isMoving = true;
        else
            _isMoving = false;
        _lastPos = transform.position;
        if (!_grounded && !_isMoving && !_isHome && _spawned)
            transform.position = Vector3.MoveTowards(transform.position, _baseTrans, speed * Time.deltaTime);

    }

    void FixedUpdate()
    {
        _groundDis = GetComponent<Collider2D>().bounds.extents.y + 2.5f;
        _gCheck = transform.up * -1;
        _gCheck.x += _offset * _dir;
        Debug.DrawRay(transform.position, _gCheck);
        _grounded = Physics2D.Raycast(transform.position, _gCheck, _groundDis, layerMaskCol);
        //Получаем координаты базы после начального приземления
        if (_grounded && !_baseTransformRO)
        {
            _baseTrans = transform.position;
            _baseTransformRO = true;
            _spawned = true; //Определяем первое приземление чтобы контролировать дальнейшее перемещение
        }
        //Ищем игрока в поле зрения
        colliders = Physics2D.OverlapCircleAll(transform.position, lostDistance, layerMask);

        if (colliders.Length != 0)
        {
            foreach (Collider2D hit in colliders)
            {
                if (hit.tag == "Player")
                {
                    target = hit.gameObject; // Делаем ссылку на игрока
                    _angry = true; // Становимся агрессивными           
                }
            }
        }
        else //Если потеряли игрока из виду, успокаиваемся
        {
            _angry = false;
            target = null;
        }
    
        if (_angry && target != null && _grounded)
        {
            float x = target.transform.position.x - transform.position.x; // смотрим где находится игрок x < 0 игрок слева, x > 0 справа

            // разворачиваемся в зависимости от того, где игрок и куда мы смотрим
            if (x < 0 && _isForward)
            {
                Flip();
                _dir = -1;
            }

            else if (x > 0 && !_isForward)
            {
                Flip();
                _dir = 1;
            }

            // Если дистанция позволяет атакуем
            if (Vector3.Distance(transform.position, target.transform.position) <= minDistance)
                Engage(); // метод атаки
            // Если мы далеко, двигаемся к цели
            else
                _myRb.velocity = (transform.right * _dir * speed);
        }
        // если мы не озлоблены завершаем работу кадра
        else
        {
            if (transform.position.x != _baseTrans.x && _spawned)
            {
                transform.position = Vector3.MoveTowards(transform.position, _baseTrans, speed * Time.deltaTime);
                _isHome = false;
            }
            else
                _isHome = true;
                return;
        }
    }

    public void Hurt(int damage)
    {
        _health -= damage;
        _psHit.Play();
        UIHud.ChangeHealthSlider(_health / HP, gameObject.name);
        if (_health <= 0 && !_dead)
            Death();
    }

    void Death()
    {
        Instantiate(_psDeath, transform.position, transform.rotation);
        Vector3 aidKitPos = new Vector3(transform.position.x, transform.position.y - .8f, transform.position.z);
        Instantiate(firstAid, aidKitPos, Quaternion.identity);
        Destroy(gameObject);
    }

    // Метод атаки
    void Engage()
    {
        //Если мы не на перезарядке, то не атакуем
        if (!_couldown)
        {
            _couldown = true; // включение перезарядки
            Attack();
            Invoke("Reload", reloadTime); // вызываем таймер перезарядки
        }
    }

    protected virtual void Attack()
    {
        EnemyRocket rocketInstance = Instantiate(rocket, startBullet.transform.position, startBullet.transform.rotation);
        rocketInstance.Launch(transform.localScale.x);
        //AudioSource.PlayClipAtPoint(shootSound, transform.position);
        _as.PlayOneShot(shootSound);
    }

    // перезарядка 
    void Reload()
    {
        _couldown = false;
    }

    void Flip()
    {
        _isForward = !_isForward;
        Vector3 V = transform.localScale;
        V.x *= -1;
        transform.localScale = V;
    }
}
