﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("Enemy")]
    [SerializeField]
    float       health = 100f;

	[Header("Bomb")]
	[SerializeField]
	float       shotCounter;
	[SerializeField]
	float       minTimeBetweenShots = 0.2f;
	[SerializeField]
	float       maxTimeBetweenShots = 3f;
	[SerializeField]
	GameObject  projectile;
	[SerializeField]
	float		bombSpeed = -10f;   // Must be negative to go down

	[Header("VFX")]
	[SerializeField]
	GameObject  explosionVFX;
	[SerializeField]
	float       durationOfExplosion = 1f;

	int         shipNumber;
	int         waveNumber;

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
		Debug.Log(GetShipID() + " is being destroyed in Enemy.cs OnApplicationQuit().");
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
		Debug.Log(GetShipID() + " signals bombs away, boss");
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
			Debug.Log(GetShipID() + "in Enemy.cs OnTriggerEnter2D(): DamageDealer was null.");
		}   // else
	}   // OnTriggerEnter2D(Collider2D other)

	/***
	*		ProcessHit() subtracts damage and if the ship is dead it tells DamageDealer
	*	that it is dead and can be removed.
	***/
	private void ProcessHit(Collider2D other, DamageDealer damageDealer)
	{
		health -= damageDealer.GetDamage();
		Debug.Log(GetShipID() + " health is now " + health);
		damageDealer.gameObject.SetActive(false);
		if (health <= 0f)
		{
			Die(damageDealer);
		}   // if
	}   // ProcessHit(Collider2D other, DamageDealer damageDealer)

	/***
	*		Die() handles doing the explosion and making the ship inactive so it
	*	disappears.
	***/
	private void Die(DamageDealer damageDealer)
	{
		Debug.Log(GetShipID() + " has been destroyed");
		gameObject.SetActive(false);
		GameObject explosion = Instantiate(
					explosionVFX,
					transform.position,
					transform.rotation);
		Destroy(explosion, durationOfExplosion);
		damageDealer.Hit(gameObject);	// This is a NOP at this point
	}   // Die()

	/***
	*		SetShipInfo() saves information we can use to identify this ship for
	*	Debug.Log() when needed.
	***/
	public void SetShipInfo(int number, int wave)
	{
		shipNumber = number;
		waveNumber = wave;
		Debug.Log(GetShipID() + " info has been set.");
	}   // SetShipInfo()

	public string GetShipID()
	{
		return "Ship " + shipNumber + " from wave " + waveNumber;
	}   // GetShipID()
}   // class Enemy