using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour 
{
	public GameObject[] tiles;

	public Vector2 grassSize;
	public Vector2 bushSize;
	public Vector2 fenceSize;
	public Vector2 leftFenceSize;
	public Vector2 rightFenceSize;
	public Vector2 treeSize;

	public Transform mapParent;

	// Map is indexed map[y,x]
	// They are also flipped over the x axis (indexing should be fine)
	/*
	 * 0 = GRASS
	 * 1 = BUSH
	 * 2 = TOP/BOTTOM FENCE
	 * 3 = LEFT FENCE
	 * 4 = RIGHT FENCE
	 * 5 = TREE
	 */

	public int[,] map = new int[,] { {2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
					 		  {3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
					 		  {3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
					 		  {3, 0, 0, 1, 0, 0, 5, 5, 5, 0, 0, 1, 0, 0, 4},
					 		  {3, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 4},
					 		  {3, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 4},
					 		  {3, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 4},
					 		  {3, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 4},
					 		  {3, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 4},
					 		  {3, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 4},
					 		  {3, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 4},
					 		  {3, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 4},
					 		  {3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
					 		  {3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
					 		  {2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2} };

	public int[,] bossmap = new int[,] { {2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
								  {3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
								  {3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
								  {3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
								  {3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
								  {3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
			  					  {3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
								  {3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
								  {3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
								  {3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
								  {2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2} };

	private void GetSizes()
	{
		grassSize = GetObjSize (tiles[0]);
		// This is an offset to move the bushes closer together to fix tile spacing/collision
		bushSize = GetObjSize (tiles[1], 0f, -80f);
		fenceSize = GetObjSize (tiles[2]);
		leftFenceSize = GetObjSize (tiles [3]);
		rightFenceSize = GetObjSize (tiles [4]);
		treeSize = GetObjSize (tiles [5], -190f, -170f);
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

	private void CreateMap(int[,] mapToUse)
	{
		mapParent = GameObject.Find ("Map").transform;
		for (int x = 0; x < mapToUse.GetLength (0); x++) 
		{
			for (int y = 0; y < mapToUse.GetLength (1); y++) 
			{
				Vector2 tileSize;
				int tile = mapToUse [y,x];
				switch (tile) 
				{
				case 1:
					tileSize = bushSize;
					break;
				case 2:
					tileSize = fenceSize;
					break;
				case 3:
					tileSize = leftFenceSize;
					break;
				case 4:
					tileSize = rightFenceSize;
					break;
				case 5:
					tileSize = treeSize;
					break;
				default:
					tileSize = grassSize;
					break;
				}

				GameObject tileInstance = Instantiate (tiles[tile], new Vector3 (x * tileSize.x, y * tileSize.y, 0f), Quaternion.identity) as GameObject;
				// Rotate fence tiles on left and right sides (until we get side fences)
				//if ((x == 0 || x == mapToUse.GetLength (0) - 1) && (y != 0 && y != mapToUse.GetLength(1) - 1))
					//tileInstance.transform.Rotate(new Vector3(0f, 0f, 90f));

				if (y == 0)
					tileInstance.gameObject.GetComponent<SpriteRenderer> ().sortingLayerName = "Walls";

				// If fence or bush add grass tile under it
				if (tile == 1 || tile == 2 || tile == 3 || tile == 4 || tile == 5) 
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

	public void SetupMap(string level)
	{
		Debug.Log ("Setting up map: " + level);
		GetSizes ();
		if (level.Equals ("level")) 
		{
			CreateMap (map);
		} 
		else
		{
			CreateMap (bossmap);
		}
	}
}
