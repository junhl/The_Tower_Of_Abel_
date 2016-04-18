using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

// Unlike single player mode, mutlieplayer mode now requires some sort of commucation to sync the movement of the many players via 'commands'
public class PlayerMovement : NetworkBehaviour {

	public GameObject bulletPrefab;
	public float playerAttackSpeed = 5.0f;
	private float bulletSpeed = 6.0f;			// Speed of ammunition in seconds
	
	[SyncVar]
	private int playerHealth = 100;
	private float playerSpeed = 0.1f;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {


		if (!isLocalPlayer) {
			return;
		}

		// If dead, respawn at a start position
		if (this.playerHealth <= 0) {
			
			print ("Calling respawning");
			this.playerHealth = 100;
			
			RpcRespawn();
			respawn();
			//CmdRespawn();
		}
		

		Vector2 velocity = this.GetComponent<Rigidbody2D> ().velocity;

		velocity = Vector2.zero;
		// Key setup is same as single mode
		if (Input.GetKey (KeyCode.W))
			this.transform.position += Vector3.up * playerSpeed;
		if (Input.GetKey (KeyCode.S))
			this.transform.position += Vector3.down * playerSpeed;
		if (Input.GetKey (KeyCode.A))
			this.transform.position += Vector3.left * playerSpeed;
		if (Input.GetKey (KeyCode.D))
			this.transform.position += Vector3.right * playerSpeed;

		// same keys to shoot
		if (Input.GetKeyDown(KeyCode.UpArrow)){
			CmdFireBullet (Vector2.up, "Up");
		}
		if (Input.GetKeyDown(KeyCode.DownArrow)){
			CmdFireBullet (Vector2.down, "Down");
		}
		if (Input.GetKeyDown(KeyCode.RightArrow)){
			CmdFireBullet (Vector2.right, "Right");
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow)){
			CmdFireBullet (Vector2.left, "Left");
		}

	}

	[Command]
	void CmdFireBullet(Vector2 pVector, string dir)
	{


		GameObject bullet = (GameObject)Instantiate (
			bulletPrefab,
			transform.position,
			Quaternion.identity
		);

		// Not consistently called by Client player for some reason - use revertHit() instead
		bullet.GetComponent<OnlineBullet> ().setSource (this.gameObject);


		Vector3 v = bullet.GetComponent<Rigidbody2D>().velocity; 
		if (dir == "Right")
		{
			v.x = bulletSpeed;
		}
		else if (dir == "Left")
		{
			v.x = bulletSpeed * -1.0f;
			
		}
		else if (dir == "Up")
		{
			v.y = bulletSpeed;
			
		}
		else
		{
			v.y = bulletSpeed * -1.0f;
			
		}
		bullet.GetComponent<Rigidbody2D>().velocity = v;
		
		RpcRevertHit();
		
		//spawn bullet on network
		NetworkServer.Spawn (bullet);
		
	}

	public float getHealth()
	{
		return this.playerHealth;
	}
	
	// Crappy solution to bullet colliding with source player. Only called by Player firing the bullet
	[ClientRpc]
	public void RpcRevertHit()
	{
		this.playerHealth += 10;
		print ("ClientRpc: Reverting health + 10 to " + this.playerHealth);
	}
	

	// Register health loss upon collision with bullet
	[ClientRpc]
	public void RpcGotHit()
	{
		this.playerHealth -= 10;
		print ("ClientRpc: Got hit! Health -10 to " + this.playerHealth);
	}
	
	
	// Called on host-side player
	[ClientRpc]
	private void RpcRespawn()
	{
		print ("Respawning ClientRpc");
		if (isLocalPlayer)
		{
			this.playerHealth = 100;
			//this.transform.position = this.GetComponent<NetworkManager>().GetStartPosition().position;
			
			var spawn=NetworkManager.singleton.GetStartPosition();
			var newPlayer = (GameObject) Instantiate (NetworkManager.singleton.playerPrefab, spawn.position, spawn.rotation);
			NetworkServer.Destroy (this.gameObject);
			NetworkServer.ReplacePlayerForConnection (this.connectionToClient, newPlayer, this.playerControllerId);
		}
	}
	
	// called on client side player
	private void respawn()
	{
		print ("Respawning no special command");
		if (isLocalPlayer)
		{
			this.playerHealth = 100;
			//this.transform.position = this.GetComponent<NetworkManager>().GetStartPosition().position;
			this.transform.position = new Vector3 (7,7,0);
			
		}
	}


}
