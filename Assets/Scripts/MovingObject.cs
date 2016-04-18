using UnityEngine;
using System.Collections;

// Provides movement functionality for Player and Enemy
public abstract class MovingObject : MonoBehaviour
{
	public float moveTime = 0.1f;			//Time it will take object to move, in seconds.
	public LayerMask blockingLayer;			//Layer on which collision will be checked.

	private BoxCollider2D boxCollider;		//The BoxCollider2D component attached to this object.
	private Rigidbody2D rb2D;				//The Rigidbody2D component attached to this object.
	private float inverseMoveTime;			//Used to make movement calculations more efficient (multiply instead of divide)

	// Getter for this moving object's RigidBody2D component
	public Rigidbody2D getRigidBody2D()
	{
		return rb2D;
	}

	// Getter for this moving object's BoxCollider2D component
	public BoxCollider2D getBoxCollider2D()
	{
		return boxCollider;
	}

	//Protected, virtual allows this method to be overridden by inheriting classes.
	protected virtual void Start () 
	{
		boxCollider = GetComponent<BoxCollider2D> ();
		rb2D = GetComponent<Rigidbody2D> ();
		inverseMoveTime = 1f / moveTime;
	}

	// Basic Movement in direction of HIT
	// Returns true if it is able to move and false if not. 
	// Takes parameters for x direction, y direction and a RaycastHit2D to check collision.
	protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
	{
		//Store start position to move from, based on objects current transform position.
		Vector2 start = transform.position;
		
		// Calculate end position based on the direction parameters passed in when calling Move.
		Vector2 end = start + new Vector2 (xDir, yDir);

		boxCollider.enabled = false;
		hit = Physics2D.Linecast (start, end, blockingLayer);
		boxCollider.enabled = true;

		if (hit.transform == null) 
		{
			StartCoroutine (SmoothMovement (end));
			return true;
		}

		return false;
	}

	// Smooth out movement code.
	// Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
	protected IEnumerator SmoothMovement (Vector3 end)
	{
		// Remaining distance to move.
		// Square magnitude is used instead of magnitude because it's computationally cheaper than magnitude.
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

		while (sqrRemainingDistance > float.Epsilon) 
		{
			Vector3 newPosition = Vector3.MoveTowards (rb2D.position, end, inverseMoveTime * Time.deltaTime);
			rb2D.MovePosition(newPosition);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			yield return null;
		}
	}

	// Attempt Move if Raycast does not collide in a direction.
	protected virtual void AttemptMove <T> (int xDir, int yDir)
		where T : Component
	{
		// Will store whatever our linecast hits when Move is called.
		RaycastHit2D hit;
		
		// Set canMove to true if Move was successful, false if failed.
		bool canMove = Move (xDir, yDir, out hit);

		if (hit.transform == null)
		{
			return;
		}

		//Get a component reference to the component of type T attached to the object that was hit
		T hitComponent = hit.transform.GetComponent<T> ();

		if (!canMove && hitComponent != null)
		{
			OnCantMove (hitComponent);
		}
	}
	
	// Will be overriden by functions in the inheriting classes.
	protected abstract void OnCantMove <T> (T component)
		where T : Component;
	
}
