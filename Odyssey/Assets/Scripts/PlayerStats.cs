using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour 
{
	[SerializeField]
	private int currentLevel;
	[SerializeField]
	private float currentExp;
	private int[] toLevelUp;
	private float fillAmount;

	public Text healthText;
	public int health = 25;

    [HideInInspector]
    public int defense = 0;

    [HideInInspector]
    public bool upgrade8 = false;

	[HideInInspector]
	public int barkDamage = 1;
	[HideInInspector]
	public int biteDamage = 3;
    [HideInInspector]
    public float expModifier = 1;

	private Image content;
	private Text expText;

	private GameObject maskObj;
	public GameObject minimap;

	public float lerpSpeed = 2f;

	void Awake() 
	{
		toLevelUp = new int[10] { 0, 20, 60, 100, 150, 200, 300, 350, 420, 500 };
		minimap = gameObject.transform.FindChild ("Minimap").gameObject;
		maskObj = transform.FindChild ("Experience Bar").transform.FindChild ("Mask").gameObject;
		content = maskObj.transform.FindChild("Content").GetComponent<Image>();
		expText = maskObj.GetComponentInChildren<Text>();

		healthText.text = "x " + health; 
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

	public void HurtPlayer(int amt)
	{
		health -= (amt - defense);
		healthText.text = "x " + health;
		IsGameOver ();
	}

	public void AddHealth(int amt)
	{
		health += amt;
		healthText.text = "x " + health;
	}

	private void IsGameOver()
	{
		if (health <= 0) 
		{
			GameManager.instance.GameOver ();
		}
	}

	public void AddExperience(int experienceToAdd)
	{
		currentExp += experienceToAdd * expModifier;
        if(upgrade8)
        {
            int random8 = Random.Range(0, 8);
            if(random8 == 7)
            {
                health += 2;
            }
        }
	}

	public void LevelUp()
	{
		currentLevel++;
		expText.text = "Level: " + currentLevel;
		// TODO: allow the player to pick which stat to upgrade
		if (currentLevel != 1 && !GameManager.hasWon())
			GameManager.instance.Upgrade ();
	}

	public void ResetStats()
	{
		health = 25;
		defense = 0;
		upgrade8 = false;
		barkDamage = 1;
		biteDamage = 3;
		currentLevel = 1;
		currentExp = 0;
		expModifier = 1;
		fillAmount = 0f;
		healthText.text = "x " + health; 
		expText.text = "Level: " + currentLevel;
		content.fillAmount = 0f;
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

	public bool GetMinimapStatus()
	{
		return (minimap.activeInHierarchy);
	}

	public void SetMinimap(bool active)
	{
		minimap.SetActive (active);
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
		return (int)currentExp;
	}
}
