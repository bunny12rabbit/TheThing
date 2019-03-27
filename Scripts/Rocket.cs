using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour 
{
	[SerializeField] GameObject explosion;		// Префаб эффекта взрыва
    [SerializeField]int _damage = 1;
    public float _speed, _lifetime;
    public AudioClip rocketSound;
    Rigidbody2D _myRb;


	void Start () 
	{
		//Уничтожаем рокету через 2 секунды если никуда не попали
		Destroy(gameObject, _lifetime);
    }

    void Flip()
        {
            Vector3 V = transform.localScale;
            V.x *= -1;
            transform.localScale = V;
        }

    public void Launch(float num)
    {
        //if (num < 0 && transform.localScale.x > 0)
        //    Flip();
        _myRb = GetComponent<Rigidbody2D>();
        _myRb.AddForce(transform.right * _speed * Mathf.Sign(num), ForceMode2D.Impulse);
    }

	void OnExplode()
	{
		// Создаем кватернион с рандомным вращением по оси Z
		Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
		// Эффект взрыва
		GameObject boom = Instantiate(explosion, transform.position, randomRotation);
        Destroy(boom, 2);
        AudioSource.PlayClipAtPoint(rocketSound, transform.position);
	}
	
	void OnTriggerEnter2D (Collider2D col) 
	{
		if(col.tag == "Enemy")
		{
			col.gameObject.GetComponent<Enemy>().Hurt(_damage);
			OnExplode();
			Destroy (gameObject);
		} else
            if (col.IsTouchingLayers(LayerMask.NameToLayer("Ground")))
        {
            OnExplode();
            Destroy(gameObject);
        }

    }
}
