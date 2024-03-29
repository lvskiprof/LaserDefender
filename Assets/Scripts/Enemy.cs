﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("Enemy")]
    [SerializeField]
    float       health = 100f;
	[SerializeField]
	long        enemyValue = 10;
	[SerializeField]
	float       yPadding = 0.75f;

	[Header("Bomb")]
	float       shotCounter;
	[SerializeField]
	float       minTimeBetweenShots = 0.2f;
	[SerializeField]
	float       maxTimeBetweenShots = 1f;
	[SerializeField]
	GameObject  projectile;
	[SerializeField]
	float		bombSpeed = -10f;   // Must be negative to go down

	[Header("VFX")]
	[SerializeField]
	GameObject  explosionVFX;
	[SerializeField]
	float       durationOfExplosion = 1f;

	[Header("SFX")]
	[SerializeField]
	AudioClip   deathSFX;
	[SerializeField] [Range(0,1)]
	float       deathSoundVolume = 0.75f;
	[SerializeField]
	AudioClip   bombDropSfx;
	[SerializeField]
	[Range(0,1)]
	float       bombSoundVolume = 0.75f;

	int         shipNumber;

	/***
	*		Cached component references.
	***/

	GameStatus  gameStatus; // GameStatus object for this level the player is on
	Level       level;

	// Start is called before the first frame update
	void Start()
    {
		gameStatus = FindObjectOfType<GameStatus>();
		level = FindObjectOfType<Level>();
		shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }	// Start()

    // Update is called once per frame
    void Update()
    {
		CountDownAndShoot();
    }   // Update()

	/***
	*		OnApplicationQuit() will destroy all the cloned Enemy objects we
	*	created.
	***/
	void OnApplicationQuit()
	{
		Debug.Log(GetShipID() + " is being destroyed in Enemy.cs OnApplicationQuit().");
		Destroy(gameObject);
	}   // OnApplicationQuit

	/***
	*		CountDownAndShoot() will drop a bomb as soon as it show up and then
	*	drop the next one after an randomized period.
	***/
	private void CountDownAndShoot()
	{
		shotCounter -= Time.deltaTime;
		if (shotCounter <= 0f)
		{
			if (projectile != null)
				Fire();

			shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
		}   // if
	}   // CountDownAndShoot()

	/***
	*		Fire() creates the bomb and drops it.
	***/
	private void Fire()
	{
		Vector2 bombStart = new Vector2(transform.position.x,
				transform.position.y - yPadding);   // Start below ship
		bombStart = transform.position;
		Debug.Log("Ship @ (" + transform.position.x + "," + transform.position.y + ") dropping bomb @ (" + bombStart.x + "," + bombStart.y + ")");
		GameObject bomb = Instantiate(projectile, bombStart, Quaternion.identity) as GameObject;
		bomb.GetComponent<Rigidbody2D>().velocity = new Vector2(0, bombSpeed);
		bomb.name = "Bomb from " + GetShipID();
		AudioSource.PlayClipAtPoint(bombDropSfx, Camera.main.transform.position, bombSoundVolume);
		Debug.Log(bomb.name + " is away, boss");
	}   // Fire()

	/***
	*		OnTriggerEnter2D() handles when we are hit by a laser.
	***/
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.CompareTag("Bomb") && !other.CompareTag("Enemy"))
		{	// Only let hits happen if they are from something other than a bomb or another enemy ship
			DamageDealer    damageDealer = other.gameObject.GetComponent<DamageDealer>();
			if (damageDealer)
			{   // Only do this if not NULL (should not happen, but check to be safe)
				ProcessHit(other, damageDealer);
			}   // if
			else
			{   // It was NULL, so report it as an error for debuggin purposes
				Debug.Log(GetShipID() + " in Enemy.cs OnTriggerEnter2D() was hit by " + other.name + " and DamageDealer was null and is being destroyed.");
				level.EnemyDestroyed();
				Destroy(gameObject);
			}   // else
		}	// if
		else
		{	// Report any hit that we are ignoring, in case we ignored something we shouldn't
			Debug.Log("Ignoring " + GetShipID() + " being hit by " + other.name + ".");
		}	// else
	}   // OnTriggerEnter2D(Collider2D other)

	/***
	*		ProcessHit() subtracts damage and if the ship is dead it tells DamageDealer
	*	that it is dead and can be removed.
	***/
	private void ProcessHit(Collider2D other, DamageDealer damageDealer)
	{
		health -= damageDealer.GetDamage();
		Debug.Log(GetShipID() + " health is now " + health + " after being hit by " + other.name + ".");
		if (health <= 0f)
		{	// Destroy what hit the ship first and then destroy the ship
			Debug.Log(gameObject.name + " is being destroyed after being hit by " + other.name + ".");
			Destroy(damageDealer.gameObject);
			Die(damageDealer);

			if (other.CompareTag("Laser"))
			{   // Destroy any laser that hits the ship
				Debug.Log(other.gameObject.name + " is being destroyed.");
				Destroy(other.gameObject);
			}   // if
		}   // if
	}   // ProcessHit(Collider2D other, DamageDealer damageDealer)

	/***
	*		Die() handles doing the explosion and making the ship inactive so it
	*	disappears.  It also updates the current score based on the ship value.
	***/
	private void Die(DamageDealer damageDealer)
	{
		gameStatus.AddToScore(enemyValue);
		Debug.Log(GetShipID() + " has been destroyed");
		//gameObject.SetActive(false);
		GameObject explosion = Instantiate(
					explosionVFX,
					transform.position,
					transform.rotation);
		Destroy(explosion, durationOfExplosion);
		AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSoundVolume);
		damageDealer.Hit(gameObject);   // This is a NOP at this point
		level.EnemyDestroyed();
		Destroy(gameObject);
	}   // Die()

	/***
	*		SetShipInfo() saves information we can use to identify this ship for
	*	Debug.Log() when needed.
	***/
	public void SetShipInfo(int number)
	{
		shipNumber = number;
		Debug.Log(GetShipID() + " info has been set.");
	}   // SetShipInfo()

	/***
	*		GetShipID() returns information we can use to identify this ship for
	*	Debug.Log() when needed.
	***/
	public string GetShipID()
	{
		return "Ship " + gameObject.name;
	}   // GetShipID()

	/***
	*		GetEnemyValue() returns information we can use to assign a value for
	*	this ship when we start keeping score.
	***/
	public long GetEnemyValue()
	{
		return enemyValue;
	}   // GetEnemyValue()
}   // class Enemy