using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// inherits from MovingObject, our base class for objects that can move
public class Player : MovingObject 
{
	public SpriteRenderer aPlayerSpriteRenderer; // our default player Abel, the male mode
	public Sprite aFemaleSprite;				// female sprites was generated way after
	public int isMale;							// checking which avatar selected
	public Slider aHealthSlider;				// UI for Hit PPOint
	public int currentLevel;					// level of board
	public int pointsPerFood = 10;
    public float playerSpeed = 0.3f;
	public int pointsPerSoda = 20;
	
	public float restartLevelDelay = 1f;		// Wait some time to reload level.
	public Text foodText;				// UI Text to display current player food total.
	
	// Audio files storing sound effects
	public AudioClip moveSound1;
	public AudioClip moveSound2;
	public AudioClip eatSound1;
	public AudioClip eatSound2;
	public AudioClip drinkSound1;
	public AudioClip drinkSound2;
	public AudioClip gameOverSound;
	
	public GameObject bulletPrefab;				// Ammunition that Player shoots

	private Animator animator;					// Controls engine's animation of Player
	private int food;							// Total number of food points held by Player during level
	private float bulletSpeed = 6.0f;			// Speed of ammunition in seconds
	private string nextLevel;


	// Use this for initialization. Overrides Start() of MovingObject
	protected override void Start () 
	{
		currentLevel = Application.loadedLevel;
		isMale = PlayerPrefs.GetInt ("tower_of_abel_male");
		print (isMale);

		if (isMale == 0) {
			aPlayerSpriteRenderer.sprite = aFemaleSprite;
		}
		animator = GetComponent<Animator> ();

		food = GameManager.instance.playerFoodPoints;

		foodText.text = "Health: " + food;

		base.Start ();
	}

	private void OnDisable()
	{
		GameManager.instance.playerFoodPoints = food;
	}

	// Update is called once per frame. Movement Based on KeyCodes.
	void Update () 
	{
		
		// Try this for walking through walls
		Vector2 now = GetComponent<Rigidbody2D>().position;
		
		//NEW MOVEMENT
		if (Input.GetKey(KeyCode.D))
		{

			now += (Vector2.right*playerSpeed);

			
        }
        if (Input.GetKey(KeyCode.A))
		{

			now += (Vector2.left*playerSpeed);
            

		}
        if (Input.GetKey(KeyCode.W))
		{

			now += (Vector2.up*playerSpeed);

		}
		if (Input.GetKey(KeyCode.S))
		{

			now += (Vector2.down*playerSpeed);

		}
		GetComponent<Rigidbody2D>().MovePosition (now);

		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			Shoot("Right");			
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			Shoot("Left");			
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			Shoot("Up");
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			Shoot("Down");
		}

	}

	// Try to move and also check when player is dead, which is gameover
	protected override void AttemptMove <T> (int xDir, int yDir)
	{
		base.AttemptMove <T> (xDir, yDir);

		RaycastHit2D hit;
		if (Move (xDir, yDir, out hit)) 
		{
			SoundManager.instance.RandomizeSfx (moveSound1, moveSound2);
		}
		CheckIfGameOver ();
	}

	//If player collides with an object, react depending on what they collided with.
	private void OnTriggerEnter2D (Collider2D other)
	{
		// upon exit, player moves to the next level
		if (other.tag == "Exit") {
			Application.LoadLevel (currentLevel+1);
			//enabled = false;
		} 
		// food will fill up player's HP
		else if (other.tag == "Food") 
		{			
			food += pointsPerFood;

			if (food > 100)
				food = 100;
			
			foodText.text = "+" + pointsPerFood + " Health: " + food;
			SoundManager.instance.RandomizeSfx (eatSound1, eatSound2);
			other.gameObject.SetActive (false);
		} 
		else if (other.tag == "Soda") {

			food += pointsPerFood;

			if (food > 100)
				food = 100;

			foodText.text = "+" + pointsPerFood + " Health: " + food;
			aHealthSlider.value = food;
			SoundManager.instance.RandomizeSfx (drinkSound1, drinkSound2);
			other.gameObject.SetActive (false);
		} else if (other.tag == "Boost") { // boost to increase player speed
			this.playerSpeed *= 1.2f;
			SoundManager.instance.RandomizeSfx (eatSound1, eatSound2);
			other.gameObject.SetActive (false);
		} else if (other.tag == "Weapon") { // weapon to increase shooting speed
			this.bulletSpeed *= 2.0f;
			other.gameObject.SetActive (false);
		} else if (other.tag == "NPC") {  // NPC interaction handled by Fungus API
			Fungus.Flowchart.BroadcastFungusMessage ("NPC_FOUND");
		}
	}

	// called in cases when player shouldn't/can't move
	protected override void OnCantMove <T> (T component)
	{
		Wall hitWall = component as Wall;
		animator.SetTrigger ("playerChop");
	}

	// name says it all....it restarts the current level
	private void Restart()
	{
		Application.LoadLevel (Application.loadedLevel);
	}

	// When hit, lose energy
	public void LoseFood (int loss)
	{
		animator.SetTrigger ("playerHit");
		food -= loss;
		foodText.text = "-" + loss + " Health: " + food;
		aHealthSlider.value = food;
		CheckIfGameOver ();
	}

	// game over when food level is 0 (or lower). GAME OVER
	private void CheckIfGameOver()
	{
		if (food <= 0)
		{
			SoundManager.instance.PlaySingle(gameOverSound);
			SoundManager.instance.musicSource.Stop();
			GameManager.instance.GameOver ();
		}
	}
	
	//Fire a Bullet Object if the player taps a certain key
	public void Shoot(string dir)
	{
		GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, Quaternion.identity);
		Vector3 v = GetComponent<Rigidbody2D>().velocity; 
		if (dir == "Right")
		{
			v.x = bulletSpeed;
		}
		else if (dir == "Left")
		{
			v.x = bulletSpeed * -1.0f;
			//bullet.transform.Rotate(Vector3.forward * 180);
		}
		else if (dir == "Up")
		{
			v.y = bulletSpeed;
			//bullet.transform.Rotate(Vector3.forward * -90);
		}
		else
		{
			v.y = bulletSpeed * -1.0f;
			//bullet.transform.Rotate(Vector3.forward * 90);
		}
		bullet.GetComponent<Rigidbody2D>().velocity = v;
	}
	
}
