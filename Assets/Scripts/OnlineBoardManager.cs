using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class OnlineBoardManager : MonoBehaviour 
{
	

	public int columns = 16; 										//Number of columns in our game board.
	public int rows = 8;											//Number of rows in our game board.
	public IntRange wallCount = new IntRange (3, 6);						//Lower and upper limit for our random number of walls per level.
	public IntRange foodCount = new IntRange (1, 5);						//Lower and upper limit for our random number of food items per level.
	
	public GameObject[] floorTiles;									//Array of floor prefabs.
	public GameObject[] wallTiles;									//Array of wall prefabs.
	public GameObject[] outerWallTiles;								//Array of outer tile prefabs.
	
	private Transform boardHolder;									//A variable to store a reference to the transform of our Board object.
	private List<Vector3> gridPositions = new List<Vector3> ();		//A list of possible locations to place tiles.

	

	void Start() {
		//Creates the outer walls and floor.
		BoardSetup ();
			
		//Reset our list of gridpositions.
		InitialiseList ();
			
		//Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
		LayoutObjectAtRandom (wallTiles, wallCount.m_Min, wallCount.m_Max);
			

	}
	
	
	//Sets up the outer walls and floor (background) of the game board.
	void BoardSetup ()
	{
		//Instantiate Board and set boardHolder to its transform.
		boardHolder = new GameObject ("Board").transform;
		
		//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
		for(int x = -1; x < columns + 1; x++)
		{
			//Loop along y axis, starting from -1 to place floor or outerwall tiles.
			for(int y = -1; y < rows + 1; y++)
			{
				
				GameObject toInstantiate;
				
				// Check if our current position is at board edge, if so choose a random outer wall prefab from array of outer wall tiles.
				if (x == -1 || x == columns || y == -1 || y == rows)
				{
					toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
				}
				
				// Otherwise, we are at a floor position. 
				else 
				{
					//Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
					toInstantiate = floorTiles[Random.Range (0,floorTiles.Length)];
				}
				
				// Instantiate the GameObject instance using the prefab chosen for toInstantiate 
				// at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
				
				
				//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
				instance.transform.SetParent (boardHolder);
			}
		}
	}
	
	//Clears our list gridPositions and prepares it to generate a new board.
	void InitialiseList ()
	{
		//Clear our list gridPositions.
		gridPositions.Clear ();
		
		//Loop through x axis (columns).
		for(int x = 1; x < columns - 1; x++)
		{
			//Within each column, loop through y axis (rows).
			for(int y = 1; y < rows - 1; y++)
			{
				//At each index add a new Vector3 to our list with the x and y coordinates of that position.
				gridPositions.Add (new Vector3(x, y, 0f));
			}
		}
	}

	
	//RandomPosition returns a random position from our list gridPositions.
	Vector3 RandomPosition ()
	{
		//Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
		int randomIndex = Random.Range (0, gridPositions.Count);
		
		//Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
		Vector3 randomPosition = gridPositions[randomIndex];
		
		//Remove the entry at randomIndex from the list so that it can't be re-used.
		gridPositions.RemoveAt (randomIndex);
		
		//Return the randomly selected Vector3 position.
		return randomPosition;
	}
	
	
	
	// Accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
	void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
	{
		/*
		//Choose a random number of objects to instantiate within the minimum and maximum limits
		int objectCount = Random.Range (minimum, maximum + 1);
		
		//Instantiate objects until the randomly chosen limit objectCount is reached
		for(int i = 0; i < objectCount; i++)
		{
			//Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
			Vector3 randomPosition = RandomPosition();
			
			//Choose a random tile from tileArray and assign it to tileChoice
			GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
			
			//Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
			Instantiate(tileChoice, randomPosition, Quaternion.identity);
		}
		*/

		Vector3 pos2 = new Vector3 (6, 6, 0);
		GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];
		Instantiate (tileChoice, pos2, Quaternion.identity);

		pos2 = new Vector3 (5, 2, 0);
		tileChoice = tileArray [Random.Range (0, tileArray.Length)];
		Instantiate (tileChoice, pos2, Quaternion.identity);

		pos2 = new Vector3 (4, 2, 0);
		tileChoice = tileArray [Random.Range (0, tileArray.Length)];
		Instantiate (tileChoice, pos2, Quaternion.identity);

		int objectCount = 3;

		for (int i = 1; i < objectCount; i++) {
			Vector3 pos = new Vector3 (i + 1, i*3, 0);
			tileChoice = tileArray [Random.Range (0, tileArray.Length)];

			Instantiate (tileChoice, pos, Quaternion.identity);
		}


	}
	
	
	
	

}
