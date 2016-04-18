using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// Singleton. Manages board generation, level progression, player controls, and enemy generation
public class GameManager : MonoBehaviour 
{

	public AudioSource aSource;
	public float levelStartDelay = 2f;				//Time to wait before starting level, in seconds.
	public float turnDelay = .1f;					//Delay between each Player turn.
	public static GameManager instance = null;		//Singleton instance, accessible by any other script
	public BoardManager boardScript;				//Store a reference to our BoardManager which will set up the level.
	public int playerFoodPoints = 100;				//Starting value for Player food points.
	//[HideInInspector] public bool playersTurn = true;	//remnant from TURN BASED movement (deprecated)

	private Text levelText;							//Text to display current level number.
	private GameObject levelImage;					//Image. Blocks out level as levels are being set up. Background for levelText.
	private int level = 1;							//Current level number, expressed in game as "Day 1".
	private List<Opponent> opponents;				//List of all Enemy and Boss units, used to issue them move commands.
	private Boss bossEnemy;             			//Enemy Unit for Boss
	private bool opponentsMoving;					//Boolean to check if enemies or boss are moving.
	private bool doingSetup;						//Boolean to check if we're setting up board. 
	private bool gamePaused = false;				// To prevent Player from moving during setup.

	// Use this for initialization. Always called before any Start functions
	void Awake () 
	{
		instance = this;

		//DontDestroyOnLoad (gameObject);
		
		opponents = new List<Opponent> ();
		
		//boardScript = GetComponent<BoardManager> ();
		
		InitGame ();
	}
	

	//Increase "Level Count" on new level begin
	void OnLevelWasLoaded (int index)
	{
		level++;

		InitGame ();
	}

	//Spawn a new Level Room by calling relevant methods
	void InitGame()
	{
		doingSetup = true;

		levelImage = GameObject.Find ("LevelImage");
		levelText = GameObject.Find ("LevelText").GetComponent<Text> ();
		levelText.text = "Level " + level;
		levelImage.SetActive (true);
		Invoke ("HideLevelImage", levelStartDelay);

		opponents.Clear ();
		//boardScript.SetupScene (level);
	}

	// Block out level image as board sets up.
	private void HideLevelImage()
	{
		levelImage.SetActive (false);
		doingSetup = false;
	}

	//Called when player dies
	public void GameOver()
	{
		levelText.text = "Game Over: Level " + level;
		levelImage.SetActive (true);
		enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//PAUSE GAME
		if (Input.GetKeyDown(KeyCode.Escape)) 
		{
			if (gamePaused == false) {
				gamePaused = true;
				Time.timeScale = 0;
			} else {
				gamePaused = false;
				Time.timeScale = 1f;
			}
		}
		if (opponentsMoving || doingSetup) {
			return;
		}

		StartCoroutine (MoveEnemies ());
	}
	
	// Add a new opponent to the board
	public void AddOpponentToList(Opponent script) 
	{
		
		if (script is Boss) 
		{
			bossEnemy = script as Boss;
		}
		
		opponents.Add(script);
	}


	//Move all enemies on the current board
	IEnumerator MoveEnemies()
	{
		opponentsMoving = true;
		yield return new WaitForSeconds (turnDelay);
		if (opponents.Count == 0) 
		{
			yield return new WaitForSeconds (turnDelay);
		}

		for (int i = 0; i < opponents.Count; i++) 
		{
			opponents [i].MoveEnemy ();
			yield return new WaitForSeconds (opponents [i].moveTime);
		}


		opponentsMoving = false;
	}
}
