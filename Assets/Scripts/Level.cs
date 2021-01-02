using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    long        enemies;

    /***
	*		Cached componenet references.
	***/

    GameStatus  gameStatus; // GameStatus object for this level the block is on

    /***
    *       Start is used to cache the Level object.
    ***/
    void Start()
    {
        gameStatus = FindObjectOfType<GameStatus>();
    }   // Start()

    /***
    *       CountEnemies() will count up how many blocks are on this level.
    ***/
    public void CountEnemies()
    {
        enemies++;
        if (enemies >= 1)
        {   // Only need to reset this if there are enemies to kill
            gameStatus.SetAllEnemiesDestroyed(false);
            Debug.Log("Total enemies = " + enemies);
        }   // if
    }   // CountEnemies()

    /***
    *       BlockDestroyed() will decrement how many blocks are on this level and will
    *   load the next level when it reaches 0.
    ***/
    public void EnemyDestroyed()
    {
        enemies--;
        if (enemies <= 0)
        {   // Set the flag to show all blocks for this level have been destroyed and load the next level
            gameStatus.SetAllEnemiesDestroyed(true);
            FindObjectOfType<SceneLoader>().LoadNextScene();
        }   // if
    }   // EnemyDestroyed()
}   // class Level