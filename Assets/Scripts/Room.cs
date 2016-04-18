using UnityEngine;

// THe board is consisted of room and corriors where the player is able to move through. THis class is for ROom
public class Room : MonoBehaviour
{
    public int xPos;                          // Left bottom of room. X coordiante.
    public int yPos;                          // The y coordinate
    public int roomWidth;                     // for size of room
    public int roomHeight;                    // for size of room
    public Direction enteringCorridor;        // in-coming corridor's direction


    // setting up the first room to base for others
    public void SetupRoom (IntRange widthRange, IntRange heightRange, int columns, int rows)
    {

        roomWidth = widthRange.Random;
        roomHeight = heightRange.Random;

        // Set the x and y coordinates so the room is roughly in the middle of the board for the first room
        xPos = Mathf.RoundToInt(columns / 2f - roomWidth / 2f);
        yPos = Mathf.RoundToInt(rows / 2f - roomHeight / 2f);
    }


    // This is an overload of the SetupRoom function. Difference is the presence of Corridor
    public void SetupRoom (IntRange widthRange, IntRange heightRange, int columns, int rows, Corridor corridor)
    {
        enteringCorridor = corridor.direction;

        roomWidth = widthRange.Random;
        roomHeight = heightRange.Random;

        switch (corridor.direction)
        {
            // as usual, 4 directions to cover
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
	// Multi-use function to fill up the content of the rooms. Let that be NPC, items, enemies etc...
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
