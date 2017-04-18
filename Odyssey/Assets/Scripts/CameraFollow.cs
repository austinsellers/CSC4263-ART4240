using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour 
{
	public GameObject player;
	Vector3 offset;

	public BoxCollider2D boundBox;
	private Vector3 minBounds;
	private Vector3 maxBounds;

	private Camera theCamera;
	private float halfHeight;
	private float halfWidth;
	public float sideOffset;

	void Start () 
	{
		boundBox = GameObject.Find("Map Bounds").GetComponent<BoxCollider2D>();
		player = GameObject.Find ("Player");
		CalcOffsets ();
	}

	void CalcOffsets ()
	{
		offset = transform.position - player.transform.position;
		minBounds = boundBox.bounds.min;
		maxBounds = boundBox.bounds.max;

		theCamera = GetComponent<Camera> ();
		halfHeight = theCamera.orthographicSize;
		halfWidth = halfHeight * Screen.width / Screen.height;
	}

	void Update () 
	{
		gameObject.transform.position = player.transform.position + offset;

		float clampedX = Mathf.Clamp (transform.position.x, minBounds.x + halfWidth - sideOffset, maxBounds.x - halfWidth + sideOffset);
		float clampedY = Mathf.Clamp (transform.position.y, minBounds.y + halfHeight - sideOffset, maxBounds.y - halfHeight + sideOffset);
		transform.position = new Vector3 (clampedX, clampedY, transform.position.z);
	}

	public void SetPlayer(GameObject newPlayer) 
	{
		boundBox = GameObject.Find("Map Bounds").GetComponent<BoxCollider2D> ();
		this.player = newPlayer;
		CalcOffsets ();
	}
}
