using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public float xMargin = 1f;		//Порог чувствительности камеры по Х
	public float yMargin = 1f;		//Порог чувствительности камеры по У
	public float xSmooth = 3f;		//Сглаживание движения по Х
	public float ySmooth = 3f;      //Сглаживание движения по У
    public float MinY = -1.66f;     //Крайняя нижняя точка камеры
    public float MaxY = 6f;         //Крайняя верхняя точка камеры
    public float MinX = -28f;       //Крайняя левая точка камеры


    public Transform player;


	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}


	bool CheckXMargin()
	{
		return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
	}


	bool CheckYMargin()
	{
		return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
	}


	void FixedUpdate ()
	{
        if (player != null && !Controller.dead)
        TrackPlayer();
	}
	
	
	void TrackPlayer ()
	{
		float targetX = transform.position.x;
		float targetY = transform.position.y;

		if(CheckXMargin())
			targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);

		if(CheckYMargin())			
			targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);

        targetX = Mathf.Clamp(targetX, MinX, Mathf.Infinity);
        targetY = Mathf.Clamp(targetY, MinY, MaxY);

        transform.position = new Vector3(targetX, targetY, transform.position.z);
	}
}
