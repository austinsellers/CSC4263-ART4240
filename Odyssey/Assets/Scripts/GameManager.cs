﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
	public int playerHealth = 10;
	private static bool paused = false;
	public GameObject pauseCanvas;

	private PlayerStats playerStats;

	public static GameManager instance = null;

	void Awake () 
	{
		// Makes sure there is only 1 GameManager
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		// Doesn't destroy when reloading scene
		DontDestroyOnLoad (gameObject);
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.P))
			paused = TogglePause ();
		if (paused) 
		{
			if (Input.GetKeyDown (KeyCode.Q))
				Quit();
			if (Input.GetKeyDown (KeyCode.R))
				Debug.Log ("Will Restart Game");
				//Restart ();
		}
	}

	bool TogglePause()
	{
		if (Time.timeScale == 0f) 
		{
			pauseCanvas.SetActive (false);
			Time.timeScale = 1f;
			return(false);
		} 
		else 
		{
			pauseCanvas.SetActive (true);
			Time.timeScale = 0f;
			return(true);
		}
	}

	public void GameOver()
	{
		// TODO: handle GameOver state displaying experience
		Debug.Log("GAME OVER!");
		Debug.Log ("Made it to level: " + playerStats.getCurrentLevel() + " with " + playerStats.getCurrentExp() + " experience!");
		//Restart ();
	} 

	public static bool isPaused()
	{
		return (paused);
	}

	public void Restart()
	{
		if (paused)
			paused = TogglePause ();
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}

	public void Quit()
	{
		// May want to move this to the main menu later
		Debug.Log ("Game Exited");
		Application.Quit();
	}

	public void SetPlayerStats(PlayerStats stats)
	{
		playerStats = stats;
	}
}
