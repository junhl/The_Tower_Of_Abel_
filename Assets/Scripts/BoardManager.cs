using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

// BoardManager responsible of containing all prefabs that will be used in the board
public class BoardManager : MonoBehaviour
{
	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;
		
		public Count (int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}

	public int columns = 8;					// Number of columns in the room
	public int rows = 8;					// Number of rows in the room
	public Count wallCount = new Count (5,9);		// limit for our random number of walls per level.
	public Count foodCount = new Count (1,5);		// limit for our random number of food items per level.
	public GameObject exit;						// Exit for player to clear the level
	public GameObject[] floorTiles;				// Array of floor prefabs.
	public GameObject[] wallTiles;				// Array of wall prefabs.
	public GameObject[] foodTiles;				// Array of food prefabs.
	public GameObject[] enemyTiles;				// Array of enemy prefabs.
	public GameObject[] outerWallTiles;			// Array of outer tile prefabs.

	private Transform boardHolder;				// Variable to store a reference to the transform of the board.
	private List <Vector3> gridPositions = new List <Vector3> ();	// list of locations to place tiles 

	// initialize our list gridPositions in order to prepares it to make a new level
	void InitialiseList()
	{
		// clean up the old info
		gridPositions.Clear ();

		for (int x = 1; x < columns - 1; x++) 
		{
			for (int y = 1; y < rows - 1; y++) 
			{
				gridPositions.Add (new Vector3 (x, y, 0f));
			}
		}
	}

	// function to be used after initiliziation. This will set up the board
	void BoardSetup ()
	{
		boardHolder = new GameObject ("Board").transform;

		for (int x = -1; x < columns + 1; x++) 
		{
			for (int y = -1; y < rows + 1; y++) 
			{
				GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
				if (x == -1 || x == columns || y == -1 || y == rows)
				{
					toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
				}

				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

				instance.transform.SetParent (boardHolder);
			}
		}
	}

	// random position for a prefab to load
	Vector3 RandomPosition()
	{
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}

	// random placement of objects from tiles
	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
	{
		int objectCount = Random.Range (minimum, maximum + 1);

		for (int i = 0; i < objectCount; i++) 
		{
			Vector3 randomPosition = RandomPosition ();
			GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];
			Instantiate (tileChoice, randomPosition, Quaternion.identity);
		}
	}

	// scene will set up based on the level of the board
	public void SetupScene(int level)
	{
		BoardSetup ();
		InitialiseList ();
		LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
		LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);
		int enemyCount = (int)Mathf.Log (level, 2f);
		LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
		Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0F), Quaternion.identity);
	}


	// initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
}
