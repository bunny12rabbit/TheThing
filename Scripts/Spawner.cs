using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	[SerializeField] float spawnTime = 5f;
    [SerializeField] float spawnDelay = 3f;
    [SerializeField] GameObject[] enemies;

    // Use this for initialization
    void Start () {
        Invoke("Spawn", spawnDelay);
	}
	
	void Spawn ()
    {
        int enemyIndex = Random.Range(0, enemies.Length); //Случайный выбор врага
        Instantiate(enemies[enemyIndex], transform.position, transform.rotation);

        //Включаем эффект спауна на всех системах частиц
        foreach(ParticleSystem p in GetComponentsInChildren<ParticleSystem>())
        {
            p.Play();
        }
    }
}
