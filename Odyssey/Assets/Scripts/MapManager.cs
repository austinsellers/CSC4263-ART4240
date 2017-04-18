using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour 
{
	public GameObject[] tiles;

	private Vector2 grassSize;
	private Vector2 bushSize;
	private Vector2 fenceSize;

	private Transform mapParent;

	// Map is indexed map[y,x]
	int[,] map = new int[,] { {2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
					 		  {2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
					 		  {2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
					 		  {2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 2},
					 		  {2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 2},
					 		  {2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 2},
					 		  {2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 2},
					 		  {2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 2},
					 		  {2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 2},
					 		  {2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 2},
					 		  {2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 2},
					 		  {2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 2},
					 		  {2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
					 		  {2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
					 		  {2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2} };

	private void GetSizes()
	{
		grassSize = GetObjSize (tiles[0]);
		// This is an offset to move the bushes closer together to fix tile spacing/collision
		bushSize = GetObjSize (tiles[1], 0f, -80f);
		fenceSize = GetObjSize (tiles[2]);
	}

	private Vector2 GetObjSize(GameObject go, float xOffset = 0f, float yOffset = 0f)
	{
		Vector2 size = go.GetComponent<SpriteRenderer> ().sprite.rect.size;
		size.x += xOffset;
		size.y += yOffset;
		Vector2 localSize = size / go.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
		localSize.x *= go.transform.lossyScale.x;
		localSize.y *= go.transform.lossyScale.y;
		return localSize;
	}

	private void CreateMap()
	{
		mapParent = new GameObject ("Map").transform;

		for (int x = 0; x < map.GetLength (0); x++) 
		{
			for (int y = 0; y < map.GetLength (1); y++) 
			{
				Vector2 tileSize;
				int tile = map [y,x];
				switch (tile) 
				{
				case 1:
					tileSize = bushSize;
					break;
				case 2:
					tileSize = fenceSize;
					break;
				default:
					tileSize = grassSize;
					break;
				}

				GameObject tileInstance = Instantiate (tiles[tile], new Vector3 (x * tileSize.x, y * tileSize.y, 0f), Quaternion.identity) as GameObject;
				// Rotate fence tiles on left and right sides (until we get side fences)
				if ((x == 0 || x == map.GetLength (0) - 1) && (y != 0 && y != map.GetLength(1) - 1))
					tileInstance.transform.Rotate(new Vector3(0f, 0f, 90f));

				if (y == 0)
					tileInstance.gameObject.GetComponent<SpriteRenderer> ().sortingLayerName = "Walls";

				// If fence or bush add grass tile under it
				if (tile == 1 || tile == 2) 
				{
					// Offset is hardcoded to move bush line up until we find a better way to deal with bush sizes
					if (tile == 1)
						tileInstance.transform.Translate (new Vector3 (0f, 5f, 0f));
					GameObject grassChild = Instantiate (tiles[0], new Vector3 (x * grassSize.x, y * grassSize.y, 1f), Quaternion.identity) as GameObject;
					grassChild.transform.SetParent (tileInstance.transform);
				}
					
				tileInstance.transform.SetParent (mapParent);
			}
		}
	}

	public void SetupMap()
	{
		GetSizes ();
		CreateMap ();

	}
}
