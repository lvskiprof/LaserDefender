using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    List<WaveConfig>    waveConfigs;
    [SerializeField]
    int                 startingWave = 0;
    [SerializeField]
    bool                looping = false;

    /***
	*		Cached componenet references.
	***/

    GameStatus  gameStatus; // GameStatus object for this level the player is on
    Level       level;      // Level the player is on

    // Start is called before the first frame update
    IEnumerator Start()
    {
        gameStatus = FindObjectOfType<GameStatus>();
        level = FindObjectOfType<Level>();

        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        } while (looping);
    }   // Start()

    /***
	*		This coroutine spawns all waves, starting with the startingWave
	*	one.	
	***/
    private IEnumerator SpawnAllWaves()
	{
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)
        {
            if (waveConfigs[waveIndex] == null)
            {   // Check for values that are not set
                Debug.Log("waveConfig[" + waveIndex + "] is null.");
            }   // if
            else
            {
                yield return StartCoroutine(SpawnAllEnemiesInWave(waveConfigs[waveIndex]));
            }   // else
        }   // for
    }   // IEnumerator SpawnAllWaves()

    /***
    *		This coroutine spawns all enemies in a wave.	
    ***/
    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        Debug.Log("Starting wave " + waveConfig.name);
        waveConfig.BuildShips();
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++)
        {
            Debug.Log("Spawning enemy #" + enemyCount + " for wave " + waveConfig.name);
            var newEnemy = waveConfig.GetEnemyShip(enemyCount);
            newEnemy.transform.position = waveConfig.GetWaypoints()[0].transform.position;
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
            level.CountEnemies();
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }   // for
    }   // IEnumerator SpawnAllEnemiesInWave()
}   // class EnemySpawner