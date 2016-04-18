using UnityEngine;
using System.Collections;

// Class for enemy objects that player has to defeat in the board
public class Enemy : Opponent 
{
	private static int turnsToWait = 0;		// Determines enemy speed
	private int turnsLeft = turnsToWait;	// Counter for enemy speed
	private int alignment = 2;				// Enemy position +/- alignment will be the range of space that enmey movement is changed by player aka chasing
	private int bubble = 10;				// Enemy position +/- bubble will be the range of space of enemy's normal movement
	
	public AudioClip enemyAttack1;			// Sound effect for attacking move
	public AudioClip enemyAttack2;			// Another sound effect for attacking move
	
	//Instantiate Object, and begin looking for Player Object
	protected override void Start () 
	{
		health = 2;
		
		GameManager.instance.AddOpponentToList (this);
		
		base.Start ();
	}
	

	// Call AttemptMove from MovingObject class
	protected override void AttemptMove <T> (int xDir, int yDir)
	{
		//Check if turns need to be skipped
		if(turnsLeft > 0)
		{
			turnsLeft--;
			return;
		}
		
		base.AttemptMove<T> (xDir, yDir);
		
		//Now that Enemy has moved, reset to the maximum number of turns to wait before next move
		turnsLeft = turnsToWait;
	}

	// Move Enemy in Player Direction to chase the player
	public override void MoveEnemy()
	{
		if (this != null)
		{
			int xDir = 0;
			int yDir = 0;
			
			float xDifference = Mathf.Abs(target.position.x - transform.position.x);
			float yDifference = Mathf.Abs(target.position.y - transform.position.y);

			// if Player not within range, return without attempting move
			if (xDifference > bubble && yDifference > bubble) 
			{
				return;
			}
			
			// difference based movement
			if (xDifference < yDifference) 
			{
				// when player is in close range with enemy
				if (xDifference < alignment)
				{
					yDir = target.position.y > transform.position.y ? 1 : -1;
					AttemptMove <Player> (xDir, yDir);
				}
			}
			else 
			{
				if (yDifference < alignment)
				{
					xDir = target.position.x > transform.position.x ? 1 : -1;
					AttemptMove <Player> (xDir, yDir);
				}
			}
		}
	}

	// Method that calls an attack on Player Collision to damage the player
	protected override void OnCantMove <T> (T component)
	{
		
		SoundManager.instance.RandomizeSfx (enemyAttack1, enemyAttack2);
		
		base.OnCantMove(component);
	}
}	
