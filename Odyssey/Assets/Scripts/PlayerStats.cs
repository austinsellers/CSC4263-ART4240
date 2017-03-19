using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour 
{
	public int currentLevel;

	public int currentExp;

	public int[] toLevelUp;

	public float fillAmount;

	public Image content;

	public Text expText;

	public float lerpSpeed;

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
}
