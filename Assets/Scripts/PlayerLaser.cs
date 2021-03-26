using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    static int  count = 1;

	[Header("VFX")]
	[SerializeField]
	GameObject  explosionVFX;
	[SerializeField]
	float       durationOfExplosion = 1.0f;

	[Header("SFX")]
	[SerializeField]
	AudioClip   deathSFX;
	[SerializeField]
	[Range(0,1)]
	float       deathSoundVolume = 0.75f;

	/***
	*		Cached component references.
	***/

	GameStatus gameStatus;

	// Start is called before the first frame update
	void Start()
    {
        this.name = "PlayerLaser #" + count.ToString();
        count++;
		gameStatus = FindObjectOfType<GameStatus>();
	}   // Start()

	// Update is called once per frame
	void Update()
    {
        
    }   // Update()

	/***
	*		OnTriggerEnter2D() handles when we are hit by a laser.
	***/
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.CompareTag("Laser") && !other.CompareTag("Player"))
		{
			DamageDealer    damageDealer = other.gameObject.GetComponent<DamageDealer>();
			if (damageDealer)
			{   // Only do this if not NULL (should not happen, but check to be safe)
				ProcessHit(other, damageDealer);
			}   // if
			else
			{   // It was NULL, so report it as an error for debugging purposes
				Debug.Log(gameObject.name + " in PlayerLaser.cs OnTriggerEnter2D() was hit by " + other.name + " and DamageDealer was null and is being destroyed.");
				Destroy(gameObject);
			}   // else
		}   // if
		else
		{   // Report any hit that we are ignoring, in case we ignored something we shouldn't
			Debug.Log("Ignoring " + gameObject.name + " being hit by " + other.name + ".");
		}   // else
	}   // OnTriggerEnter2D(Collider2D other)

	/***
	*		ProcessHit() subtracts damage and if the ship is dead it tells DamageDealer
	*	that it is dead and can be removed.
	***/
	private void ProcessHit(Collider2D other, DamageDealer damageDealer)
	{
		int damage = damageDealer.GetDamage();
		Debug.Log(gameObject.name + " has hit " + other.name + " and is being damaged by " + damage + " hits.");
		Die(damageDealer);

		if (other.CompareTag("Laser"))
		{   // Destroy any laser that hits the bomb
			Debug.Log(other.gameObject.name + " is being destroyed.");
			Destroy(other.gameObject);
		}   // if
	}   // ProcessHit(Collider2D other, DamageDealer damageDealer)

	/***
	*		Die() handles doing the explosion and making the ship inactive so it
	*	disappears.
	***/
	private void Die(DamageDealer damageDealer)
	{
		Debug.Log(gameObject.name + " has been destroyed");
		gameObject.SetActive(false);
		GameObject explosion = Instantiate(
					explosionVFX,
					transform.position,
					transform.rotation);
		Destroy(explosion, durationOfExplosion);
		AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSoundVolume);
		damageDealer.Hit(gameObject);   // This is a NOP at this point
		if (damageDealer.gameObject.CompareTag("Laser"))
		{   // Only add to the score if the bomb hit by the player's laser
			Bomb bomb = damageDealer.gameObject.GetComponent<Bomb>();
			gameStatus.AddToScore(bomb.GetBombValue());
		}   // if
	}   // Die()
}   // class PlayerLaser