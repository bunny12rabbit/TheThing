using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] float _jumpForce = 22f, _acceleration = 6f;
    [SerializeField] LayerMask _mask;
    [HideInInspector] public static bool _isForward = true;
    [SerializeField] protected float HP = 5f;
    FloatingJoystick floatingJoystick;
    public AudioClip jumpSound, deathSound;
    public ParticleSystem _ps;
    protected float _health;
    Rigidbody2D _myRb;
    Collider2D _myCol;
    Transform _gCheck;
    public static bool dead = false;
    int _doubleJump = 0;
    bool _grounded = false;
    float _rayDistance, _dir;
    Animator _animator;
    AudioSource _as;


    void Start ()
    {
        _myRb = GetComponent<Rigidbody2D>();
        _gCheck = transform.Find("groundCheck");
        _rayDistance = GetComponent<Collider2D>().bounds.extents.y + 0.15f;
        _myCol = GetComponent<Collider2D>();
        _animator = GetComponentInChildren<Animator>();
        _health = HP;
        dead = false;
        _as = GetComponent<AudioSource>();
        floatingJoystick = FindObjectOfType<FloatingJoystick>();
    }

    void Update()
    {
#if UNITY_STANDALONE
        _dir = Input.GetAxis("Horizontal");
#endif
#if UNITY_ANDROID
        _dir = floatingJoystick.Horizontal;
#endif

        // Проверка на приземление
        _grounded = Physics2D.Linecast(transform.position, _gCheck.position, 1 << LayerMask.NameToLayer("Ground"));

#if UNITY_STANDALONE
        if ((Input.GetKeyDown(KeyCode.Space)) && !MenuUI._paused)
        {
            Jump();
        }
#endif

        if (_grounded)
            _animator.SetBool("IsJumping", false);
        else
            _animator.SetBool("IsJumping", true);
    }

    void FixedUpdate()
    {
        _animator.SetFloat("Speed", Mathf.Abs(_dir));

        if (_dir < 0)
        {
            if (_isForward)
            {
                Flip();
            }
        }

        else if (_dir > 0 && !_isForward)
            Flip();

        if (_dir > 0.1 || _dir < 0.1)
            Move(_dir);

        if (_doubleJump >= 2 && _grounded)
            _doubleJump = 0;
    }

    public void Jump()
    {
        if (_doubleJump < 2 || _grounded)
        {
            _doubleJump++;
            _myRb.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);
            var maxHeight = _myRb.velocity;
            maxHeight.y = Mathf.Clamp(maxHeight.y, 0, 10);
            _myRb.velocity = maxHeight;
            _as.PlayOneShot(jumpSound);
        }
    }

    void Flip()
    {
        _isForward = !_isForward;

        Vector3 V = transform.localScale;
        V.x *= -1;
        transform.localScale = V;
    }

    public void Hurt(int damage)
    {
        _health -= damage;
        _ps.Play();
        UIHud.ChangeHealthSlider((float)_health / HP, gameObject.name);
        if (_health <= 0 && !dead)
            Death();
    }

    public void Heal(int health)
    {
        _health += health;
        UIHud.ChangeHealthSlider((float)_health / HP, gameObject.name);
    }

    public void Death()
    {
        dead = true;
        //AudioSource.PlayClipAtPoint(deathSound, transform.position);
        _as.PlayOneShot(deathSound);
            _myCol.enabled = false;
            _myRb.AddForce(Vector2.up * 25, ForceMode2D.Impulse);
        Destroy(gameObject, 3f);
    }

    void Move(float ax)
    {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, 1, _mask);
        Vector3 _dir = transform.right * ax;

        //transform.position = Vector2.Lerp(transform.position, transform.position + _dir, _acceleration * Time.deltaTime);
        transform.Translate(_dir * _acceleration * Time.deltaTime);
    }
}
