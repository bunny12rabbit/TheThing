using UnityEngine;
using System.Collections;

public class LayBombs : MonoBehaviour
{
	public bool bombLaid = false;		// Bomb has been planted))
	public int bombCount = 0;			// Количество бомб у игрока
	public AudioClip bombsAway;			// Звук бомбы
	public GameObject bomb;				// Префаб бомбы


	private GUITexture bombHUD;			// Текстура GUI есть ли бомбы


	void Awake ()
	{
		bombHUD = GameObject.Find("ui_bombHUD").GetComponent<GUITexture>();
	}


	void Update ()
	{
		if(Input.GetButtonDown("Fire2") && !bombLaid && bombCount > 0)
		{
			bombCount--;
			bombLaid = true;
			AudioSource.PlayClipAtPoint(bombsAway,transform.position);
			Instantiate(bomb, transform.position, transform.rotation);
		}
		bombHUD.enabled = bombCount > 0;
	}
}
