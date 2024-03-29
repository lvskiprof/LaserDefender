﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="EnemyWaveConfig")]
public class WaveConfig : ScriptableObject
//public class WaveConfig : MonoBehaviour
{
	[SerializeField]
	GameObject			enemyPrefab;
	[SerializeField]
	GameObject			pathPrefab;
	[SerializeField]
	float				timeBetweenSpawns = 0.5f;
	[SerializeField]
	float				spawnRandomFactor = 0.3f;
	[SerializeField]
	int					numberOfEnemies = 5;
	[SerializeField]
	float				moveSpeed = 2f;

	List<GameObject>    enemyShips;

	// BuildShips() is used to create all the instances of the enemy ships in a WaveConfig before they are used.
	public void BuildShips()
	{
		Debug.Log("Creating " + numberOfEnemies + " ships for " + this.name);
		enemyShips = new List<GameObject>();

		for (int ship = 0; ship < numberOfEnemies; ship++)
		{
			var enemyShip = Instantiate(
					enemyPrefab,
					pathPrefab.transform.position,
					Quaternion.identity);

			enemyShip.GetComponent<Enemy>().SetShipInfo(ship);
			enemyShip.name = enemyPrefab.name + " #" + ship.ToString() + " in " + this.name;
			enemyShips.Add(enemyShip);
		}   // for
	}   // BuildShips()

	/***
	*		OnApplicationQuit() will destroy all the cloned Enemy objects we
	*	created.
	***/
	void OnApplicationQuit()
	{
		for (int ship = 0; ship < enemyShips.Count; ship++)
		{
			Destroy(enemyShips[ship].gameObject);
		}   // for
	}   // OnApplicationQuit

	/***
	*		What follows are the various Getter methods for our variables that
	*	we wish to make available outside this class	
	***/
	public GameObject GetEnemyPrefab() { return enemyPrefab; }
	public List<Transform> GetWaypoints() 
	{
		var waveWaypoints = new List<Transform>();

		foreach (Transform waypoint in pathPrefab.transform)
		{
			waveWaypoints.Add(waypoint);
		}	// foreach

		return waveWaypoints; 
	}	// GetWaypoints()
	public float GetTimeBetweenSpawns() { return timeBetweenSpawns; }
	public float GetSpawnRandomFactor() { return spawnRandomFactor; }
	public int GetNumberOfEnemies() { return numberOfEnemies; }
	public float GetMoveSpeed() { return moveSpeed; }
	public GameObject GetEnemyShip(int ship) 
	{
		Debug.Log("WaveConfig line #79, ship = " + ship + ", enemyShips.Count = " + enemyShips.Count + ", for " + this.name + ".");
		enemyShips[ship].transform.position = pathPrefab.transform.position;
		Debug.Log(enemyShips[ship].GetComponent<Enemy>().GetShipID() + " was created.");
		//enemyShips[ship].SetActive(true);	// Make it active now that we are using it
		return enemyShips[ship];
	}   // GameObject GetEnemyShip()
}   // class WaveConfig