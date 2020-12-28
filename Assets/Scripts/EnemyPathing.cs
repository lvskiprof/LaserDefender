using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
	[SerializeField]
	WaveConfig          waveConfig;
	[SerializeField]
	List<Transform>     waypoints;
    int                 waypointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
		waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].transform.position;
    }   // Start()

	// Update is called once per frame
	void Update()
	{
		Move();
	}   // Update()

	/***
    *       SetWaveConfig() will set the WaveConfig we will be using for this
    *   enemy.
    ***/
	public void SetWaveConfig(WaveConfig newWaveConfig)
	{
		waveConfig = newWaveConfig;
	}   // SetWaveConfig()

	/***
    *       Move() will move the enemy towards the next waypoint.
    ***/
	private void Move()
	{
		if (waypointIndex <= waypoints.Count - 1)
		{
			var targetPosition = waypoints[waypointIndex].transform.position;
			var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;

			if (transform.position == targetPosition)
			{   // If we are already at the waypoint, change to the next waypoint
				targetPosition = waypoints[waypointIndex].transform.position;
				waypointIndex++;
			}   // if

			transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);
		}   // if
		else if (waypoints.Count != 1)
		{	// We only make a ship disappear if it is at the last of multiple waypoints
			gameObject.SetActive(false);    // Make is disappear so we can reuse it later
		}	// else-if
	}   // Move()

	/***
    *       SetWaypoints() will find all the waypoint objects and add them to the wayppoints List.
    ***/
	private void SetWaypoints()
    {
		/* Commented out for now until we need it.
        GameObject[] gameObjects = GetComponents<GameObject>();
        for (int i = 0; i < gameObjects.Length; i++)
		{
            if (gameObjects[i].name.Contains("Waypoint"))
			{
                waypoints.Add(gameObjects[i].transform);
			}   // if
		}   // for (i)
        */
	}   // SetWaypoints()
}   // class EnemyPathing
