using UnityEngine;
using UnityEngine.Networking;

// OnlineRoomCreator is responsible of making rooms in which player is able to move and connect to corridors
public class OnlineRoomCreator : NetworkBehaviour
{
	public int xPos;                          // room's left bottom's x coordinate
	public int yPos;                          // room's left bottom's y coordinate
	public int roomWidth;                     // width for size of room
	public int roomHeight;                    // height for size of room
	public Direction enteringCorridor;        // Just like singleplayer mode, corridors has direction. In this case, this is in-coming corridors

	// Just like single player mode, we need to set up the first room first
	public void SetupRoom (IntRange widthRange, IntRange heightRange, int columns, int rows)
	{
		roomWidth = widthRange.Random;
		roomHeight = heightRange.Random;

		// Unlike the singleplyer mode, the starting point of player is roughly in middle of the room
		xPos = Mathf.RoundToInt(columns / 2f - roomWidth / 2f);
		yPos = Mathf.RoundToInt(rows / 2f - roomHeight / 2f);
	}
	
	// This is an overload of the SetupRoom function and has a corridor parameter that represents the corridor entering the room.
	public void SetupRoom (IntRange widthRange, IntRange heightRange, int columns, int rows, Corridor corridor)
	{
		enteringCorridor = corridor.direction;
		// Set random values for width and height.
		roomWidth = widthRange.Random;
		roomHeight = heightRange.Random;

		switch (corridor.direction)
		{
		// Like single mode, the corridors have 4 directions, therefore 4 cases to cover
		case Direction.North:
			roomHeight = Mathf.Clamp(roomHeight, 1, rows - corridor.EndPositionY);
			yPos = corridor.EndPositionY;
			xPos = Random.Range (corridor.EndPositionX - roomWidth + 1, corridor.EndPositionX);
			xPos = Mathf.Clamp (xPos, 0, columns - roomWidth);
			break;
		case Direction.East:
			roomWidth = Mathf.Clamp(roomWidth, 1, columns - corridor.EndPositionX);
			xPos = corridor.EndPositionX;
			yPos = Random.Range (corridor.EndPositionY - roomHeight + 1, corridor.EndPositionY);
			yPos = Mathf.Clamp (yPos, 0, rows - roomHeight);
			break;
		case Direction.South:
			roomHeight = Mathf.Clamp (roomHeight, 1, corridor.EndPositionY);
			yPos = corridor.EndPositionY - roomHeight + 1;
			xPos = Random.Range (corridor.EndPositionX - roomWidth + 1, corridor.EndPositionX);
			xPos = Mathf.Clamp (xPos, 0, columns - roomWidth);
			break;
		case Direction.West:
			roomWidth = Mathf.Clamp (roomWidth, 1, corridor.EndPositionX);
			xPos = corridor.EndPositionX - roomWidth + 1;
			yPos = Random.Range (corridor.EndPositionY - roomHeight + 1, corridor.EndPositionY);
			yPos = Mathf.Clamp (yPos, 0, rows - roomHeight);
			break;
		}
	}
	// Fill up the rooms with the array of prefabs. THis is multi-use helper function to load NPC, enemies, items etc
	public void populateRoom(GameObject[] prefabs, int max)
	{
		int randomIndex = Random.Range(0, prefabs.Length);
		int num = Random.Range(1,max);
		for (int i = 0 ; i < num ; i++)
		{
			Vector3 position = new Vector3(Random.Range(this.xPos,this.xPos+roomWidth), Random.Range(this.yPos,this.yPos+roomHeight), 0f);
			GameObject obj = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;
		}

	}
}
