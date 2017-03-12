using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTracking : MonoBehaviour 
{
	Transform player;
    Vector2 mousePos;
	private float moveSpeed = 1f;

	public Texture2D cursorTexture;
	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;

	void Awake()
	{
		Cursor.SetCursor (cursorTexture, hotSpot, cursorMode);
		Cursor.visible = true;
	}

	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	void OnMouseExit()
	{
		Cursor.SetCursor (null, Vector2.zero, cursorMode);
	}

	void Update ()
    {
        mousePos = Input.mousePosition;
		mousePos = Camera.main.ScreenToWorldPoint (mousePos);
		//.position = Vector2.Lerp (reticlePos.position, mousePos, moveSpeed);
		/*player.rotation = Quaternion.Euler (0, 0, Mathf.Atan2 (mousePos.y - player.position.y, 
			mousePos.x - player.position.x) * Mathf.Rad2Deg - 90);
		Debug.Log (player.rotation);*/
	}
}
