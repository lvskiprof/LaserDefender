using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    float       health = 1000f;
    [SerializeField]
    float       moveSpeed = 10;
    [SerializeField]
    float       xPadding = 0.5f;
    [SerializeField]
    float       YPadding = 0.45f;
    [Header("Projectile")]
    [SerializeField]
    GameObject laserPrefab;
    [SerializeField]
    float       projectileSpeed = 10f;
    [SerializeField]
    float       projectileFiringPeriod = 0.25f;
    float       xMin;
    float       xMax;
    float       yMin;
    float       yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
    }   // Start()

	// Update is called once per frame
	void Update()
    {
        Move();
        Fire();
    }   // Update()

    /***
    *       Move() will use the older Input API to move the ship from left/right
    *   and up/down.  The variable moveSpeed will control how fast it moves
    *   per frame and can be adjusted in the Inspector.
    ***/
    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
    }   // Move

    /***
    *       Fire() will handle firing the laser from the Player ship.
    ***/
    private void Fire()
	{
        if (Input.GetButtonDown("Fire1"))
		{
            StartCoroutine(FireContinuously());
		}   // if
	}   // Fire()

    /***
    *     SetUpMoveBoundaries() sets the limits for where our display limits
    *   are based on World coordinates, so it should work on any size screen.
    ***/
    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + xPadding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - xPadding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + YPadding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - YPadding;
    }   // SetUpMoveBoundaries()

    /***
    *       FireContinuously() will fire the laser from the Player ship and keep
    *   firing as long as the space bar or mouse button 0 are pressed.  Presumably
    *   it will also work with a joystick, but I don't have one to test with.
    ***/
    private IEnumerator FireContinuously()
    {
        do
        {   // Fire laser and check to see if we should keep firing
            // Things to do before
            Vector2 laserStart = new Vector2(transform.position.x,
                transform.position.y + YPadding);   // Start above ship
            GameObject laser = Instantiate(laserPrefab,
                laserStart,
                Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            Debug.Log("Laser fired, boss");
            yield return new WaitForSeconds(projectileFiringPeriod);
            // Things to do afterwards
            Debug.Log("Checking to see if we should keep firing, boss");
        } while (Input.GetButton("Fire1"));
        Debug.Log("Firing has ceased, boss");
    }   //    FireContinuously()

    /***
	*		OnTriggerEnter2D() handles when we are hit by a bomb.
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
            Debug.Log("In Player.cs OnTriggerEnter2D(): DamageDealer was NULL.");
		}   // else
    }   // OnTriggerEnter2D(Collider2D other)

    /***
	*		ProcessHit() subtracts damage and if the ship is dead it tells DamageDealer
	*	that it is dead and can be removed.
	***/
    private void ProcessHit(Collider2D other, DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        Debug.Log("Player health is now " + health);
        damageDealer.gameObject.SetActive(false);
        if (health <= 0f)
        {
            damageDealer.Hit(gameObject);
        }   // if
    }   // ProcessHit(Collider2D other, DamageDealer damageDealer)
}   // class Player