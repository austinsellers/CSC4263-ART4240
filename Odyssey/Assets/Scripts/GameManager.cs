﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	public int playerHealth = 10;
	public int playerExperience = 0;

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

	public void GameOver()
	{
		// TODO: handle GameOver state displaying experience
		Debug.Log("U DED NOW!!");
	} 
}
