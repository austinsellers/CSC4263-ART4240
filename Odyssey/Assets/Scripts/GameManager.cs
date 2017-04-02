﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
	private static bool paused = false;
	private static bool gameOver = false;
	private static bool upgrade = false;
	public GameObject pauseCanvas;
	public GameObject gameOverCanvas;
	public GameObject upgradeCanvas;

	private Text upgrade1;
	private Text upgrade2;
	private Text upgrade3;

	private Text gameOverText;

	[HideInInspector]
	public PlayerStats playerStats;
	private MapManager mapManager;

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
		mapManager = GetComponent<MapManager> ();
		SetupGame ();
	}

	void SetupGame()
	{
		mapManager.SetupMap ();
	}

	void Start()
	{
		gameOverText = gameOverCanvas.GetComponentsInChildren<Text> ()[1];
		upgrade1 = upgradeCanvas.transform.FindChild ("Upgrade Image 1").GetComponentsInChildren<Text> () [1];
		upgrade2 = upgradeCanvas.transform.FindChild ("Upgrade Image 2").GetComponentsInChildren<Text> () [1];
		upgrade3 = upgradeCanvas.transform.FindChild("Upgrade Image 3").GetComponentsInChildren<Text> () [1];
		DontDestroyOnLoad (upgradeCanvas);
		DontDestroyOnLoad (pauseCanvas);
		DontDestroyOnLoad (gameOverCanvas);
	}

	void Update()
	{
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
				Restart ();
				gameOverCanvas.SetActive (false);
			}
		} 
		else if (upgrade) 
		{
			if (Input.GetKeyDown (KeyCode.Alpha1) || Input.GetKeyDown (KeyCode.Keypad1)) 
			{
				playerStats.AddHealth (5);
				upgrade = ToggleScale (upgradeCanvas);
			} 
			else if (Input.GetKeyDown (KeyCode.Alpha2) || Input.GetKeyDown (KeyCode.Keypad2)) 
			{
				playerStats.barkDamage += 2;
				upgrade = ToggleScale (upgradeCanvas);
			} 
			else if (Input.GetKeyDown (KeyCode.Alpha3) || Input.GetKeyDown (KeyCode.Keypad3)) 
			{
				playerStats.biteDamage += 3;
				upgrade = ToggleScale (upgradeCanvas);
			}
		}
		else 
		{
			if (Input.GetKeyDown (KeyCode.P))
				paused = ToggleScale (pauseCanvas);
		}
	}

	bool ToggleScale(GameObject go)
	{
		if (Time.timeScale == 0f) 
		{
			go.SetActive (false);
			Time.timeScale = 1f;
			return(false);
		} 
		else 
		{
			go.SetActive (true);
			Time.timeScale = 0f;
			return(true);
		}
	}

	public void Upgrade()
	{
		upgrade = true;
		upgrade1.text = "Press 1\n+5 Health\nCurrent: " + playerStats.health;
		upgrade2.text = "Press 2\n+2 Damage\nCurrent: " + playerStats.barkDamage;
		upgrade3.text = "Press 3\n+3 Damage\nCurrent: " + playerStats.biteDamage;
		upgradeCanvas.SetActive (true);
		upgrade = ToggleScale (upgradeCanvas);
	}

	public void GameOver()
	{
		// TODO: handle GameOver state displaying experience
		gameOver = true;
		gameOverText.text = "Player Reached LVL: "  + playerStats.getCurrentLevel() + "\nWith Experience of: " + playerStats.getCurrentExp() + "\nPress \"R\" to Restart";
		gameOverCanvas.SetActive (true);
	} 

	public static bool isPaused()
	{
		return (paused);
	}

	public static bool isUpgrade()
	{
		return (upgrade);
	}

	public void Restart()
	{
		if (paused)
			paused = ToggleScale (pauseCanvas);
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
