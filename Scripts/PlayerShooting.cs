using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

    [SerializeField] Rocket bullet;
    [SerializeField] Transform startBullet;
    [SerializeField] Transform layBomb;
    [SerializeField] Bomb bomb;
    [SerializeField] float _rocketSpeed = 20f;
    public AudioClip shootSound;
    public AudioClip layBombSound;
    AudioSource _as;

    void Start()
    {
        startBullet = transform.Find("StartBullet");
        layBomb = transform.Find("LayBomb");
        _as = GetComponent<AudioSource>();
    }

	// Update is called once per frame
	void Update () {

#if UNITY_STANDALONE
        if (Input.GetButtonDown("Fire1") && !MenuUI._paused)
        {
            Shoot();
        }
        if (Input.GetButtonDown("Fire2") && !MenuUI._paused)
        {
            Instantiate(bomb, layBomb.position, layBomb.rotation);
            _as.PlayOneShot(layBombSound);
        }
#endif
    }

    public void Shoot()
    {
        Rocket bulletInstance = Instantiate(bullet, startBullet.position, startBullet.rotation);
        bulletInstance._speed = _rocketSpeed;
        bulletInstance.Launch(transform.localScale.x);
        _as.PlayOneShot(shootSound);
    }

    public void LayMine()
    {
        Instantiate(bomb, layBomb.position, layBomb.rotation);
        _as.PlayOneShot(layBombSound);
    }
}
