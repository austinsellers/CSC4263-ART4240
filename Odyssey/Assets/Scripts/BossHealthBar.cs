using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour 
{
	[SerializeField]
	private float currentHealth;
	private float maxHealth;
	private float fillAmount;

	private BossEnemy boss;

	private Image content;
	private Text expText;

	private GameObject maskObj;

	public float lerpSpeed = 2f;

	void Awake () {
		maskObj = transform.FindChild ("Boss Health Bar").transform.FindChild ("Mask").gameObject;
		content = maskObj.transform.FindChild("Content").GetComponent<Image>();
		expText = maskObj.GetComponentInChildren<Text>();
		boss = GameObject.Find ("Boss").gameObject.GetComponent<BossEnemy>();
		maxHealth = boss.health;
		currentHealth = maxHealth;
		content.fillAmount = maxHealth;
		expText.text = "BOSS Health: " + (int)currentHealth;
	}

	void Update () 
	{
		currentHealth = boss.health;
		if (currentHealth < 0)
			currentHealth = 0;
		if (currentHealth >= 0) 
		{
			UpdateBar ();
		}
	}

	private void UpdateBar()
	{
		fillAmount = MapExp (currentHealth, 0, maxHealth);
		if (fillAmount != content.fillAmount) 
		{
			content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, lerpSpeed * Time.deltaTime);
		}
		expText.text = "BOSS Health: " + (int)currentHealth;
	}
		
	private float MapExp(float value, float inMin, float inMax)
	{
		return (value - inMin) * 1f / (inMax - inMin);
	}
}
