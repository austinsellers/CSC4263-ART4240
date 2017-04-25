using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour 
{
	void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		GameManager.instance.SetLevel (scene.name);
		foreach(GameObject o in GameObject.FindGameObjectsWithTag ("Player"))
		{
			if (!o.Equals (GameManager.instance.GetPlayer()))
				DestroyImmediate (o);
		}
		foreach(GameObject o in GameObject.FindGameObjectsWithTag ("Game Over Canvas"))
		{
			if (!o.Equals (GameManager.instance.gameOverCanvas))
				DestroyImmediate (o);
		}
		foreach(GameObject o in GameObject.FindGameObjectsWithTag ("Pause Canvas"))
		{
			if (!o.Equals (GameManager.instance.pauseCanvas))
				DestroyImmediate (o);
		}
		foreach(GameObject o in GameObject.FindGameObjectsWithTag ("Upgrade Canvas"))
		{
			if (!o.Equals (GameManager.instance.upgradeCanvas))
				DestroyImmediate (o);
		}
		foreach(GameObject o in GameObject.FindGameObjectsWithTag ("Main Canvas"))
		{
			if (!o.Equals (GameManager.instance.mainCanvas))
				DestroyImmediate (o);
		}

		if (scene.name == "BossBattle") 
		{
			GameManager.instance.mapManager.SetupMap ("boss");
			GameManager.instance.GetPlayer ().transform.position = new Vector3 (20.86f, 28.37f, 0f);
			if (GameManager.instance.playerStats.GetMinimapStatus())
				GameManager.instance.playerStats.SetMinimap (false);
		} 
		else if (scene.name == "GameScene")
		{
			GameManager.instance.enemyManagers = GameObject.FindObjectsOfType<EnemyManager> ();
			GameManager.instance.GetPlayer ().transform.position = new Vector3 (17.6f, 17.3f, 0f);
			GameManager.instance.GetPlayerController ().currentPos = GameManager.instance.GetPlayer ().transform.position;
			GameManager.instance.GetPlayerController ().projectedPos = GameManager.instance.GetPlayer ().transform.position;
		}
	}
}
