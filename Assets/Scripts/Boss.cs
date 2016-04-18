using UnityEngine;
using System.Collections;

public class Boss : Opponent {

	private Room room;						// Room in which Boss and exit reside
	private int baseXPos;					// X coordinate at which Boss is instantiated to gaurd exit
	private int baseYPos;					// Y coordinate at which Boss is instantiated to gaurd exit

	public AudioClip bossAttack1;			// Sound effect for attacking move
	public AudioClip bossAttack2;			// Another sound effect for attacking move

	//Instantiate Object, and begin looking for Player Object
	protected override void Start () 
	{
		health = 20;

		GameManager.instance.AddOpponentToList (this);
		
		base.Start ();
	}
	
	// Setter for private variables, called by BoardCreator
	public void setRoom(Room room, int guardExitXPos, int guardExitYPos)
	{
		this.room = room;
		
		this.baseXPos = guardExitXPos;
		this.baseYPos = guardExitYPos;
	}


	// Call AttemptMove from MovingObject class
	protected override void AttemptMove <T> (int xDir, int yDir)
	{
		
		base.AttemptMove<T> (xDir, yDir);
		
	}


	// Move Enemy in Player Direction
	public override void MoveEnemy()
	{
		if (this != null)
		{
			
			int xDir = 0;
			int yDir = 0;

			Vector3 targetPos = target.position;
			Vector3 myPos = transform.position;


			// If Player is inside Boss room, move towards Player.
			if (inRoom(targetPos))
			{

				xDir = targetPos.x > myPos.x ? 1 : -1;
				yDir = targetPos.y > myPos.y ? 1 : -1;
				
				AttemptMove <Player> (xDir, yDir);

			}
			
			
			// Otherwise, Player isn't inside the room containing this Boss, move towards exit to guard it
			// rather than moving towards Player and blocking corridor.
			else {
			
				// If already at exit-guarding spot, return without moving
				if ( Mathf.Abs (baseXPos - myPos.x) < 0.5 && Mathf.Abs (baseYPos - myPos.y) < 0.5 )
				{
					return;
				}
				
				// Otherwise, move towards exit-guarding spot
				else {
					
					xDir = baseXPos > myPos.x ? 1 : -1;
					yDir = baseYPos > myPos.y ? 1 : -1;
					
					AttemptMove <Player> (xDir, yDir);
				}
			}

		}

	}
	
	
	// Helper to determine if Player is inside Boss' room
	private bool inRoom(Vector3 targetPos)
	{
		
		// (room.xPos, room.yPos) is the bottom left corner of room
		if (targetPos.x > room.xPos && targetPos.x < (room.xPos + room.roomWidth) &&
			targetPos.y > room.yPos && targetPos.y < (room.yPos + room.roomHeight))
		{
			return true;
		}
		
		else
		{
			return false;
		}
	}
	

	// Method that calls an attack on Player Collision
	protected override void OnCantMove <T> (T component)
	{
		SoundManager.instance.RandomizeSfx (bossAttack1, bossAttack2);
		
		base.OnCantMove(component);
	}
	
}
