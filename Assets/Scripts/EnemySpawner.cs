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
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        } while (looping);
    }

    /***
	*		This coroutine spawns all waves, starting with the startingWave
	*	one.	
	***/
    private IEnumerator SpawnAllWaves()
	{
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)
        {
            yield return StartCoroutine(SpawnAllEnemiesInWave(waveConfigs[waveIndex]));
        }   // for
    }   // IEnumerator SpawnAllWaves()

    /***
    *		This coroutine spawns all enemies in a wave.	
    ***/
    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        Debug.Log("Starting wave " + waveConfig.name);
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++)
        {
            Debug.Log("Spawning enemy #" + enemyCount + " for wave " + waveConfig.name);
            var newEnemy = waveConfig.GetEnemyShip(enemyCount);
            newEnemy.transform.position = waveConfig.GetWaypoints()[0].transform.position;
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }   // for
    }   // IEnumerator SpawnAllEnemiesInWave()
}   // class EnemySpawner