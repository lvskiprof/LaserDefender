using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{

    /***
	*		Cached component references.
	***/

    Scene   currentScene;	// When this doesn't match the current scene, save it and check if we are in Game Over scene

    // Awake is called before the first note is played
    void Awake()
    {
		int musicPlayerCount = FindObjectsOfType<MusicPlayer>().Length;
		if (musicPlayerCount > 1)
		{   // We only want to keep the first one to stop the track from starting over with each scene
			gameObject.SetActive(false);    // Prevent anything from being able to find or interact with this instance of GameStatus
			Destroy(gameObject);    // Destroy this secondary version of GameStatus
		}   // if
		else
		{   // Tell Unity not to destroy this gameObject when another scene loads in the future
			DontDestroyOnLoad(gameObject);
		}   // else
	}   // Awake()

	// Update is called once per frame
	void Update()
    {
        
    }   // Update()
}   // class MusicPlayer