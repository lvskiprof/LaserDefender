using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneLoader : MonoBehaviour
{
/***
*		LoadNextScene() will load whatever the next numbered scene in Build Settings.
***/
	public void LoadNextScene()
	{
		int     currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

		SceneManager.LoadScene(currentSceneIndex + 1);
	}   // LoadNextScene()

	/***
	*		LoadStartScene() will load the first scene in the Build Settings.
	***/
	public void LoadStartScene()
	{
		//GameStatus gameStatus = FindObjectOfType<GameStatus>();
		//gameStatus.ResetScore();
		SceneManager.LoadScene(0);
	}   // LoadStartScene()

	/***
	*		LoadGameScene() will load the first scene in the Build Settings.
	***/
	public void LoadGameScene()
	{
		//GameStatus gameStatus = FindObjectOfType<GameStatus>();
		//gameStatus.ResetScore();
		SceneManager.LoadScene("Game");
	}   // LoadGameScene()

	/***
	*		LoadGameScene() will load the first scene in the Build Settings.
	***/
	public void LoadGameOver()
	{
		//GameStatus gameStatus = FindObjectOfType<GameStatus>();
		//gameStatus.ResetScore();
		SceneManager.LoadScene("GameOver");
	}   // LoadGameScene()

	/***
	*       ExitGame() is called to exit the game.  It handles the two cases of
	*   running in standalone mode or in the Unity Editor.
	***/
	public void ExitGame()
	{
		if (Application.isPlaying & !Application.isEditor)
			Application.Quit(); // We may return from this, but the program will terminate at the end of the frame
#if true
		else
			UnityEditor.EditorApplication.isPlaying = false;    // Handle being in the editor, but set #if to true to use it
#endif
	}   // ExitGame()
}   // class SceneLoader