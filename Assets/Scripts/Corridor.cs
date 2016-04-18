using UnityEngine;

// As mentioned in board creator, corridors will have a direction to which it leads to the next room
public enum Direction
{
    North, East, South, West,
}

// Class has starting coordinates and extend from that point by its length towards the direction
public class Corridor
{
    public int startXPos;         // The x coordinate to start of corridor
    public int startYPos;         // The y coordinate
    public int corridorLength;            // how long is the corridor
    public Direction direction;   // direction of corridor to lead to next room


    // end position is from start's x position extened by the length towards direction
    public int EndPositionX
    {
        get
        {
			if (direction == Direction.North || direction == Direction.South) 
			{
				
				return startXPos;
			}
			if (direction == Direction.East) 
			{
				return startXPos + corridorLength - 1;
			}
            return startXPos - corridorLength + 1;
        }

    }

	// same logic for Y position as well.
    public int EndPositionY
    {
        get
        {
			if (direction == Direction.East || direction == Direction.West) 
			{
				return startYPos;
			}
			if (direction == Direction.North) 
			{
				return startYPos + corridorLength - 1;
			}
            return startYPos - corridorLength + 1;
        }
    }

    // the acutal making process of the corridors
    public void SetupCorridor (Room room, IntRange length, IntRange roomWidth, IntRange roomHeight, int columns, int rows, bool firstCorridor)
    {
		
		// Hard-coded the first direction of the first corridor
		if (firstCorridor)
		{
			direction = Direction.South;
		}
		
		// After the fisrt one, we go random 
		else 
		{
			// this random number to be used for choosing direction
			direction = (Direction)Random.Range(0, 4);
			
			Direction oppositeDirection = (Direction)(((int)room.enteringCorridor + 2) % 4);

			// applying logic test cover up the random factor because we dont want same direction
			if (direction == oppositeDirection)
			{
				// inversing the direction
				int directionInt = (int)direction;
				directionInt++;
				directionInt = directionInt % 4;
				direction = (Direction)directionInt;
				
			}
		}
        corridorLength = length.Random;
        int maxLength = length.m_Max;

		// 4 cases of direction
        switch (direction)
        {
            case Direction.North:
                startXPos = Random.Range (room.xPos, room.xPos + room.roomWidth - 1);
                startYPos = room.yPos + room.roomHeight;
				maxLength = rows - startYPos - roomHeight.m_Min;
                break;
            case Direction.East:
                startXPos = room.xPos + room.roomWidth;
                startYPos = Random.Range(room.yPos, room.yPos + room.roomHeight - 1);
                maxLength = columns - startXPos - roomWidth.m_Min;
                break;
            case Direction.South:
                startXPos = Random.Range (room.xPos, room.xPos + room.roomWidth);
                startYPos = room.yPos;
                maxLength = startYPos - roomHeight.m_Min;
                break;
            case Direction.West:
                startXPos = room.xPos;
                startYPos = Random.Range (room.yPos, room.yPos + room.roomHeight);
                maxLength = startXPos - roomWidth.m_Min;
                break;
        }
        corridorLength = Mathf.Clamp (corridorLength, 1, maxLength);
    }
}
