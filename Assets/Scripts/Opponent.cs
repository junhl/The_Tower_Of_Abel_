using UnityEngine;
using System.Collections;


// Abstract parent of Boss and Enemy. Meant to capture common functionality; most differences
// between Boss and Enemy are in MoveEnemy() and AttemptMove()
public abstract class Opponent : MovingObject {

	public int playerDamage;			// Number of food points player loses as a result of this Opponent hitting it
	
	protected int health;				// Number of times this Opponent can be hit by a bullet before dying

	private Animator animator;			// For engine animation
	
	protected Transform target;			// Transform of player GameObject
	

	//Instantiate Object, and begin looking for Player Object to chase
	protected override void Start () 
	{
		animator = GetComponent<Animator> ();
		
		// the target is player, which enemies want to chase/kill
		GameObject[] targets = GameObject.FindGameObjectsWithTag ("Player");
		
		// Make last target the Player that this Enemy follows
		if (targets.Length > 0) 
		{
			target = targets[targets.Length - 1].transform;
		}
		
		base.Start ();
	}

	// Call AttemptMove from MovingObject class
	protected override void AttemptMove <T> (int xDir, int yDir) 
	{
		base.AttemptMove<T> (xDir, yDir);
	}

	// Move Enemy in Player Direction
	public abstract void MoveEnemy();
	
	// Method that calls an attack on Player Collision
	protected override void OnCantMove <T> (T component)
	{
		Player hitPlayer = component.gameObject.GetComponent<Player>();

		hitPlayer.LoseFood (playerDamage);

		animator.SetTrigger ("enemyAttack");
	
	}
	
	// called when opponent makes contact with player
	void OnCollisionEnter2D (Collision2D target) 
	{
		if (target.gameObject.tag == "Player")
		{
			// debug purpose
			print ("Hit player! Hit player! Hit player! Hit player! Hit player! Hit player! Hit player! Hit player! ");
			OnCantMove(target.rigidbody);
		}
	}

	//Deduct health from the Enemy when called
	public void EnemyHit() 
	{
		// Losing 1 HP when hit by player
		if (health > 1)
		{
			health--;
		}
		// if less than 1 HP, the enemy shall die...
		else 
		{
			Destroy(this.gameObject);
		}
	}
}
