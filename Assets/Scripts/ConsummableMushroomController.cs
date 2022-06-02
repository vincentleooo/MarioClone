using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsummableMushroomController : MonoBehaviour
{
	private Rigidbody2D mushroomBody;
	private int multiplier;
	// Start is called before the first frame update
	void Start()
	{
		mushroomBody = GetComponent<Rigidbody2D>();
		System.Random random = new System.Random();
		float randomNumber = random.Next(100);
		if (randomNumber >= 50)
		{
			multiplier = 1;
		}
		else
		{
			multiplier = -1;
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.CompareTag("GroundObs"))
		{
			Vector2 movement = mushroomBody.velocity;
			mushroomBody.AddForce(-movement * 50);
			Debug.Log("Hit");
		};
	}

	// Update is called once per frame
	void Update()
	{	
		Vector2 speed = new Vector2(multiplier * 5, 0);

		mushroomBody.velocity = speed;

	}

	void  OnBecameInvisible(){
	Destroy(gameObject);	
}
}
