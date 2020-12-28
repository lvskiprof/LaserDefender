using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("Enemy")]
    [SerializeField]
    float       health = 100f;
	[Header("Projectile")]
	[SerializeField]
	float       shotCounter;
	[SerializeField]
	float       minTimeBetweenShots = 0.2f;
	[SerializeField]
	float       maxTimeBetweenShots = 3f;
	[SerializeField]
	GameObject  projectile;
	[SerializeField]
	float   bombSpeed = -10f;	// Must be negative to go down

	// Start is called before the first frame update
	void Start()
    {
		shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }	// Start()

    // Update is called once per frame
    void Update()
    {
		CountDownAndShoot();
    }   // Update()

	/***
	*		OnApplicationQuit() will destroy all the cloned Enemy objects we created.
	***/
	void OnApplicationQuit()
	{
		Destroy(gameObject);
	}   // OnApplicationQuit

	/***
	*		CountDownAndShoot() will drop a bomb as soon as it show up and then drop the
	*	next one after an randomized period.
	***/
	private void CountDownAndShoot()
	{
		shotCounter -= Time.deltaTime;
		if (shotCounter <= 0f)
		{
			Fire();
			shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
		}   // if
	}   // CountDownAndShoot()

	/***
	*		Fire() creates the bomb and drops it.
	***/
	private void Fire()
	{
		GameObject bomb = Instantiate(
			projectile,
			transform.position,
			Quaternion.identity) as GameObject; ;
		bomb.GetComponent<Rigidbody2D>().velocity = new Vector2(0, bombSpeed);
		Debug.Log("Bombs away, boss");
	}   // Fire()

	/***
	*		OnTriggerEnter2D() handles when we are hit by a laser.
	***/
	private void OnTriggerEnter2D(Collider2D other)
	{
		DamageDealer    damageDealer = other.gameObject.GetComponent<DamageDealer>();
		if (damageDealer)
		{   // Only do this if not NULL (should not happen, but check to be safe)
			ProcessHit(other, damageDealer);
		}   // if
		else
		{   // It was NULL, so report it as an error for debuggin purposes
			Debug.Log("In Enemy.cs OnTriggerEnter2D(): DamageDealer was NULL.");
		}   // else
	}   // OnTriggerEnter2D(Collider2D other)

	/***
	*		ProcessHit() subtracts damage and if the ship is dead it tells DamageDealer
	*	that it is dead and can be removed.
	***/
	private void ProcessHit(Collider2D other, DamageDealer damageDealer)
	{
		health -= damageDealer.GetDamage();
		Debug.Log("Enemy " + other.gameObject.name + " health is now " + health);
		damageDealer.gameObject.SetActive(false);
		if (health <= 0f)
		{
			damageDealer.Hit(gameObject);
		}   // if
	}   // ProcessHit(Collider2D other, DamageDealer damageDealer)
}   // class Enemy