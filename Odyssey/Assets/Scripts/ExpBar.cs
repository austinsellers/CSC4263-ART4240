using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour 
{
	[SerializeField]
	private float fillAmount;

	[SerializeField]
	private Image content;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateBar ();
	}

	private void UpdateBar()
	{
		content.fillAmount = MapExp(53,0,100);
	}

	private float MapExp(float value, float inMin, float inMax)
	{
		return (value - inMin) * 1f / (inMax - inMin);
	}
}
