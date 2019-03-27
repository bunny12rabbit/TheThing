using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPlatform : MonoBehaviour {

    [SerializeField] GameObject FireworkLarge;
    [SerializeField] GameObject FireworkSmall;
    [SerializeField] PlayerShooting playerShooting;
    [SerializeField] Collider2D finishFlag;
    ParticleSystem psFireworkLarge;
    ParticleSystem psFireworkSmall;
    public static bool LevelComplete = false;

    private void Start()
    {
        psFireworkLarge = FireworkLarge.GetComponent<ParticleSystem>();
        psFireworkSmall = FireworkSmall.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (LevelComplete && !psFireworkSmall.isPlaying)
        {
            psFireworkLarge.Play();
            psFireworkSmall.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            LevelComplete = true;
            psFireworkLarge.Play();
            psFireworkSmall.Play();
            Cursor.visible = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            finishFlag.isTrigger = false;
        }
    }
}
