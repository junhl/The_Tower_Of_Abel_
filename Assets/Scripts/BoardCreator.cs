using System.Collections;
using UnityEngine;

// Sets parameters for semi-random room and map generation, and instantiates rooms and map
public class BoardCreator : MonoBehaviour
{
    // The type of tile that will be laid in a specific position.
    public enum TileType
    {
        Wall, Floor,
    }

	
    public int columns = 100;                                 // Number of columns on the board (how wide it will be).
    public int rows = 100;                                    // Number of rows on the board (how tall it will be).
    
    public IntRange roomWidth = new IntRange (3, 10);         // Range of widths rooms can have.
    public IntRange roomHeight = new IntRange (3, 10);        // Range of heights rooms can have.
    public IntRange corridorLength = new IntRange (6, 10);    // Range of lengths corridors between rooms can have.	
	
	public Level level;										// Contains information for level generation, including enemy difficulty and health
    public GameObject[] floorTiles;                           // Array of floor tile prefabs to place on board
    public GameObject[] wallTiles;                            // Array of wall tile prefabs to place on board
    public GameObject[] outerWallTiles;                       // Array of outer wall tile prefabs on board
	public GameObject[] enemies;								// Array of enemies on level
	public GameObject[] bosses;
	public GameObject[] foods;
	public GameObject[] NPCs;
	public GameObject player;									// player that that user is controlling
	public GameObject exit;									  //Prefab to spawn for exit.
	
    private TileType[][] tiles;                               // A jagged array of tile types representing the board, like a grid.
    private Room[] rooms;                                     // Array to keep track of the rooms created
    private Corridor[] corridors;                             // Array to keep track of the corridors created
    private GameObject boardHolder;                           // GameObject that acts as a container for all other tiles.
	
	// Level-determined variables
	private int numRooms;
	private int maxEnemyNumber;
	private int maxFoodNumber;
	
	
    // Calls all methods to do a 'setup' of the board by placing the walls, rooms etc. 
    private void Start ()
    {
        // Create the board holder.
        boardHolder = new GameObject("BoardHolder");
		
		//level.GetComponent<Level>().Awake();
		// Set level-specific parameters
		SetupLevelDifficulty();

        SetupTilesArray ();

        CreateRoomsAndCorridors ();

        SetTilesValuesForRooms ();
        SetTilesValuesForCorridors ();

        InstantiateTiles ();
        InstantiateOuterWalls ();
    }
	
	
	// Instantiate private variables determining level difficulty, using our Level object
	private void SetupLevelDifficulty()
	{
		// Instantiate level's difficulty parameters.
		level.Setup();
		
		maxEnemyNumber = level.getMaxEnemyNumber();
		maxFoodNumber = level.getMaxFoodNumber();
		numRooms = level.getNumRooms();
		
	}

	
    // Sets board as a 2D array of tiles
    void SetupTilesArray ()
    {
        // Set the tiles jagged array to the correct width.
        tiles = new TileType[columns][];

        for (int i = 0; i < tiles.Length; i++)
        {
            // set each tile array is the correct height.
            tiles[i] = new TileType[rows];
        }
    }

    // Sets the map by instantiating rooms and corridors arrays, and by calling SetupRoom and SetupCorridor for each element
	// this is helper function that is used in start() that use sub-helper functions
    void CreateRoomsAndCorridors ()
    {
        rooms = new Room[numRooms];
        corridors = new Corridor[rooms.Length - 1];

        // Creating the FIRST room and corridor to base on for other rooms/corridors
        rooms[0] = new Room ();
        corridors[0] = new Corridor ();
        rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);
		
		
		//Instantiate the exit tile in the upper right hand corner of the first room
		int exitXPos = rooms[0].xPos + rooms[0].roomWidth - 1;
		int exitYPos = rooms[0].yPos + rooms[0].roomHeight - 1;
		Instantiate (exit, new Vector3(exitXPos, exitYPos, 0), Quaternion.identity);
		
		// Instantiate boss guarding the exit
		int offX = Random.Range(2,5);
		int offY = Random.Range(2,5);
		GameObject bossObj = Instantiate(bosses[0], new Vector3(exitXPos-offX, exitYPos-offY, 0), Quaternion.identity) as GameObject;
		// Pass room and exit coordinates for Boss AI
		bossObj.GetComponent<Boss>().setRoom(rooms[0], exitXPos-offX, exitYPos-offY);
				
        // Setup the first corridor using the first room.
        corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, columns, rows, true);
		// Once you have the first room, we have a room to base on. We can keep on building base on the first room.
        for (int i = 1; i < rooms.Length; i++)
        {
            rooms[i] = new Room ();
            rooms[i].SetupRoom (roomWidth, roomHeight, columns, rows, corridors[i - 1]);
			
			// Last room contains the level's boss and exit. Try with not including enemies to make boss move more. 
			if (i != rooms.Length - 1) 
			{
				rooms[i].populateRoom(enemies, maxEnemyNumber);
			}
			rooms[i].populateRoom(foods, maxFoodNumber);
            // If we haven't reached the end of the corridors array...
            if (i < corridors.Length)
            {
                corridors[i] = new Corridor ();
                // Setup the corridor based on the room that was just created.
                corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, columns, rows, false);
            }
			
        }
		
		// Spawn player in bottom left corner of last room of game board as well as NPC near the player's spawning spot
		Vector3 playerPos = new Vector3 (rooms[rooms.Length - 1].xPos, rooms[rooms.Length - 1].yPos, 0);
        Instantiate(player, playerPos, Quaternion.identity);
		Vector3 npcPos = new Vector3 (rooms[rooms.Length - 1].xPos+5, rooms[rooms.Length - 1].yPos+5, 0);
		int npcV = Random.Range(0,NPCs.Length);
		Instantiate(NPCs[npcV], npcPos, Quaternion.identity);
		

    }

    // Sets floor tiles to TileType. Floor for each room of the map
    void SetTilesValuesForRooms ()
    {
        // Loop over all rooms because rooms will be filled with floor tiles !
        for (int i = 0; i < rooms.Length; i++)
        {
            Room currentRoom = rooms[i];
            for (int j = 0; j < currentRoom.roomWidth; j++)
            {
                int xCoord = currentRoom.xPos + j;
                for (int k = 0; k < currentRoom.roomHeight; k++)
                {
                    int yCoord = currentRoom.yPos + k;
                    tiles[xCoord][yCoord] = TileType.Floor;
                }
            }
        }
    }

    // Similarly to previous fucntions, corridors will be filled with floor tiles
    void SetTilesValuesForCorridors ()
    {
        // Looping on all corridors
        for (int i = 0; i < corridors.Length; i++)
        {
            Corridor currentCorridor = corridors[i];
            for (int j = 0; j < currentCorridor.corridorLength; j++)
            {
                // coordinates of the corridor
                int xCoord = currentCorridor.startXPos;
                int yCoord = currentCorridor.startYPos;
				
				// Corridors will have directions, towards which it leads to the next room
                switch (currentCorridor.direction)
                {
                    case Direction.North:
                        yCoord += j;
                        break;
                    case Direction.East:
                        xCoord += j;
                        break;
                    case Direction.South:
                        yCoord -= j;
                        break;
                    case Direction.West:
                        xCoord -= j;
                        break;
                }
                tiles[xCoord][yCoord] = TileType.Floor;
            }
        }
    }

	// using the array of tiles, we will actually instaniate the floor prefabs
    void InstantiateTiles ()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                // the actual instantiation here
                InstantiateFromArray (floorTiles, i, j);
                if (tiles[i][j] == TileType.Wall)
                {
                    InstantiateFromArray (wallTiles, i, j);
                }
            }
        }
    }

    // the walls will be either inner or outer wall. THiis is outer case
    void InstantiateOuterWalls ()
    {
        float leftEdgeX = -1f;
        float rightEdgeX = columns + 0f;
        float bottomEdgeY = -1f;
        float topEdgeY = rows + 0f;

        // Actual instantiation. They have direction of how to be placed
		// using the helper functions to do so
        InstantiateVerticalOuterWall (leftEdgeX, bottomEdgeY, topEdgeY);
        InstantiateVerticalOuterWall(rightEdgeX, bottomEdgeY, topEdgeY);

        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, bottomEdgeY);
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, topEdgeY);
    }

    // Helper function for InstantiateOuterWalls
    void InstantiateVerticalOuterWall (float xCoord, float startingY, float endingY)
    {
        // Start point 
        float currentY = startingY;
        while (currentY <= endingY)
        {
            // actual instantiate
            InstantiateFromArray(outerWallTiles, xCoord, currentY);

            currentY++;
        }
    }

    // Helper function for InstantiateOuterWalls
    void InstantiateHorizontalOuterWall (float startingX, float endingX, float yCoord)
    {
        // Start point
        float currentX = startingX;
        while (currentX <= endingX)
        {
            // actual instantiate
            InstantiateFromArray (outerWallTiles, currentX, yCoord);

            currentX++;
        }
    }

    // helper method to randomly assign a GameObject tile from an array that can be used in many different context/cases
    void InstantiateFromArray (GameObject[] prefabs, float xCoord, float yCoord)
    {
        int randomIndex = Random.Range(0, prefabs.Length);
        Vector3 position = new Vector3(xCoord, yCoord, 0f);

        // Instantiate from a random index
        GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;
        tileInstance.transform.parent = boardHolder.transform;
    }
	
}
