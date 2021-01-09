using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Bomb")]
    [SerializeField]
    float       health = 100f;
    [SerializeField]
    long        bombValue = 10;

    [Header("VFX")]
    [SerializeField]
    GameObject  explosionVFX;
    [SerializeField]
    float       durationOfExplosion = 1f;

    [Header("SFX")]
    [SerializeField]
    AudioClip   deathSFX;
    [SerializeField]
    [Range(0,1)]
    float       deathSoundVolume = 0.75f;

    /***
	*		OnApplicationQuit() will destroy all the cloned Bomb
	*	objects we created.
	***/
    void OnApplicationQuit()
    {
        Debug.Log(gameObject.name + " is being destroyed in Bomb.cs OnApplicationQuit().");
        Destroy(gameObject);
    }   // OnApplicationQuit

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
			Debug.Log(gameObject.name + "in Bomb.cs OnTriggerEnter2D(): DamageDealer was null.");
			Destroy(gameObject);
		}   // else
	}   // OnTriggerEnter2D(Collider2D other)

	/***
	*		ProcessHit() subtracts damage and if the ship is dead it tells DamageDealer
	*	that it is dead and can be removed.
	***/
	private void ProcessHit(Collider2D other, DamageDealer damageDealer)
	{
		health -= damageDealer.GetDamage();
		Debug.Log(gameObject.name + " health is now " + health);
		if (health <= 0f)
		{
			Die(damageDealer);
		}   // if

		Debug.Log(damageDealer.gameObject.name + " is being destroyed.");
		Destroy(damageDealer.gameObject);
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
	}   // Die()
}   // class Bomb