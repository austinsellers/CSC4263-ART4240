using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyMovement {

	string Name = "Ranged";
	protected void Start () {
		base.Start (Name);
	}
	
	// Update is called once per frame
	void Update () {
		base.Move();
	}
}
