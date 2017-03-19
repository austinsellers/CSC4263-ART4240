﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
	public int playerHealth = 10;
	public int playerExperience = 0;
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

		playerStats = FindObjectOfType<PlayerStats> ();
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.P))
			paused = TogglePause ();
		if (paused) 
		{
			if (Input.GetKeyDown (KeyCode.Q))
				Quit();
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
		Debug.Log("U DED NOW!!");
		Debug.Log ("Made it to level: " + playerStats.currentLevel + " with " + playerStats.currentExp + " experience!");
	} 

	public static bool isPaused()
	{
		return (paused);
	}

	public void Quit()
	{
		// May want to move this to the main menu later
		Debug.Log ("Game Exited");
		Application.Quit();
	}
}
