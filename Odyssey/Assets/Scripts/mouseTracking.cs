using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTracking : MonoBehaviour 
{
    private static Vector2 mousePos;

	public Texture2D cursorTexture;
	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;

	void Awake()
	{
		Cursor.SetCursor (cursorTexture, hotSpot, cursorMode);
		Cursor.visible = true;
	}

	void OnMouseExit()
	{
		Cursor.SetCursor (null, Vector2.zero, cursorMode);
	}

	void Update ()
    {
        mousePos = Input.mousePosition;
		mousePos = Camera.main.ScreenToWorldPoint (mousePos);
	}

	public static Vector2 getMousePos() 
	{
		return mousePos;
	}
}
