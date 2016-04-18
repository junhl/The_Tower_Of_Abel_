using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour 
{


	public int level;						// Game level. Increasing level = increasing difficulty
	
	// Food difficulty
	private int maxFoodNumber;				// Maximum number (minimum 1) of food items to generate randomly in each room of board
	
	// Health difficulty
	// TODO: how to pass parameters into Player instantiation
	
	// Enemy difficulty
	private int maxEnemyNumber;				// Maximum number (minimum 1) of enemies to generate randomly in each room of board
	// TODO: pass bubble and alignment to Enemy instantiation
	
	// Board difficulty
	private int numRooms;					// Number of rooms in the game board. More rooms = more time needed to reach exit
	

	// Called to instantiate level difficulty parameters
	// TODO: Make this an automatic Unity call during Level object instantiation. MonoBehaviour.Start() and Awake() aren't called for some reason
	public void Setup()
	{
		
		// For each level, instantiate appropriate difficulty of board/enemy/player variables
		switch(level)
		{
			case 1:
			
				maxFoodNumber = 4;
				maxEnemyNumber = 3;
				numRooms = 3;
				
				break;
				
			case 2:
			
				maxFoodNumber = 3;
				maxEnemyNumber = 4;
				numRooms = 5;
				
				break;
				
			case 3:
			
				maxFoodNumber = 3;
				maxEnemyNumber = 6;
				numRooms = 6;
				
				break;
			case 4:
			
				maxFoodNumber = 3;
				maxEnemyNumber = 7;
				numRooms = 7;
				
				break;
			case 5:
			
				maxFoodNumber = 2;
				maxEnemyNumber = 8;
				numRooms = 7;
				
				break;
			case 6:
			
				maxFoodNumber = 1;
				maxEnemyNumber = 8;
				numRooms = 7;
				
				break;
			case 7:
			
				maxFoodNumber = 3;
				maxEnemyNumber = 10;
				numRooms = 8;
				
				break;
		}
		
		
	}
	
	// Getters for each of the private class variables determining level difficulty
	
	public int getMaxFoodNumber()
	{
		return maxFoodNumber;
	}
	
	public int getMaxEnemyNumber()
	{
		return maxEnemyNumber;
	}
	
	public int getNumRooms() 
	{
		
		// might want to be safer here to check for uninstantiated variable
		return numRooms;
	}
	
	
}
