﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour 
{
	private int currentLevel;
	private int currentExp;
	private int[] toLevelUp;
	private float fillAmount;

	private Image content;
	private Text expText;

	private GameObject maskObj;

	public float lerpSpeed = 2f;

	void Awake() 
	{
		toLevelUp = new int[10] { 0, 20, 50, 100, 250, 500, 1000, 1750, 2500, 3500 };

		maskObj = transform.FindChild ("Experience Bar").transform.FindChild ("Mask").gameObject;
		content = maskObj.transform.FindChild("Content").GetComponent<Image>();
		expText = maskObj.GetComponentInChildren<Text>();
	}

	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.O))
			currentExp += 10;
		// Prevent out of bounds problems
		if (currentLevel < toLevelUp.Length) 
		{
			if (currentExp >= toLevelUp [currentLevel]) 
			{
				LevelUp ();
			}
			UpdateBar ();
		}
	}

	public void AddExperience(int experienceToAdd)
	{
		currentExp += experienceToAdd;
	}

	public void LevelUp()
	{
		currentLevel++;
		expText.text = "Level: " + currentLevel;
		// TODO: allow the player to pick which stat to upgrade
	}

	// Experience Bar Methods
	private void UpdateBar()
	{
		fillAmount = MapExp (currentExp, toLevelUp [currentLevel - 1], toLevelUp [currentLevel]);
		if (fillAmount != content.fillAmount) 
		{
			content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, lerpSpeed * Time.deltaTime);
		}
	}

	private float MapExp(float value, float inMin, float inMax)
	{
		return (value - inMin) * 1f / (inMax - inMin);
	}

	public int getCurrentLevel()
	{
		return currentLevel;
	}

	public int getCurrentExp()
	{
		return currentExp;
	}
}
