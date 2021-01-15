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
    *       AddEnemy() will add to the count of many enemies are on this level.
    ***/
    public void AddEnemy()
    {
        enemies++;
        if (enemies >= 1)
        {   // Only need to reset this if there are enemies to kill
            gameStatus.SetAllEnemiesDestroyed(false);
            Debug.Log("Total enemies = " + enemies);
        }   // if
    }   // AddEnemy()

    /***
    *       EnemyDestroyed() will decrement how many enemies are on this level and will
    *   load the Game Over level when it reaches 0.
    ***/
    public void EnemyDestroyed()
    {
        enemies--;
        Debug.Log("Total enemies = " + enemies);
        if (enemies <= 0)
        {   // Set the flag to show all blocks for this level have been destroyed and load the next level
            gameStatus.SetAllEnemiesDestroyed(true);
            FindObjectOfType<SceneLoader>().LoadGameOver();
        }   // if
    }   // EnemyDestroyed()
}   // class Level