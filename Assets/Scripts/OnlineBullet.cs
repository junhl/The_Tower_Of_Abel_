using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class OnlineBullet : NetworkBehaviour {

	public GameObject aSource;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/*
	 * Set the Source of the bullet.
	 */
	public void setSource(GameObject obj){

		this.aSource = obj;
		print ("source is now : " + obj.name);

	}

	
	// Called by engine upon collision
	void OnTriggerEnter2D(Collider2D coll){
		
		if (coll.gameObject.tag == "Player") 
		{
			
			print ("Bullet collided with Player!");
			
			GameObject hitPlayer = coll.gameObject;
			hitPlayer.GetComponent<PlayerMovement> ().RpcGotHit ();
/*
			print ("aSource = " + aSource + ", coll.gameObject = " + coll.gameObject);

			if (hitPlayer != aSource && hitPlayer.gameObject.tag != "Bullet") {
				print ("Source : " + aSource);
				hitPlayer.GetComponent<PlayerMovement> ().gotHit ();
				//Destroy (this.gameObject);
			}
	*/		
			
		}
		
		else if (coll.gameObject.tag == "Wall")
        {
			print ("Collided with Wall!");
			Destroy(this.gameObject);
		}
		
		else if (coll.tag == "Bullet")
		{
			print ("Collided with another Bullet!");
			Destroy (this.gameObject);
		}
	}
}
