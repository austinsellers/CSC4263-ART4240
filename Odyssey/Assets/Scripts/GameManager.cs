﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
	public int playerHealth = 10;
	private static bool paused = false;
	private static bool gameOver = false;
	public GameObject pauseCanvas;
	public GameObject gameOverCanvas;

	private Text gameOverText;

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

	void Start()
	{
		gameOverText = gameOverCanvas.GetComponentsInChildren<Text> ()[1];
		DontDestroyOnLoad (pauseCanvas);
		DontDestroyOnLoad (gameOverCanvas);
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
				Restart ();
		}
		if (gameOver) 
		{
			if (Input.GetKeyDown (KeyCode.R)) 
			{
				gameOverCanvas.SetActive (false);
				Restart ();
			}
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
		gameOver = true;
		gameOverText.text = "Player Reached LVL: "  + playerStats.getCurrentLevel() + "\nWith Experience of: " + playerStats.getCurrentExp() + "\nPress \"R\" to Restart";
		gameOverCanvas.SetActive (true);
		Debug.Log("GAME OVER!");
		Debug.Log ("Made it to level: " + playerStats.getCurrentLevel() + " with " + playerStats.getCurrentExp() + " experience!");
	} 

	public static bool isPaused()
	{
		return (paused);
	}

	public void Restart()
	{
		if (paused)
			paused = TogglePause ();
		gameOver = false;
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
