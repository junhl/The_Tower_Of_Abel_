using UnityEngine;
using System.Collections;

// Manages shooting. In progress.
public class FireProjectile : MonoBehaviour 
{

	public GameObject bulletPrefab;			// Prefab for current ammunition type
	public float bulletSpeed;				// Speed of bullet with respect to frame rate
	float attackSpeed = 0.5f;				// To modulate animation with respect to frame rate
	float coolDown = 0.5f;				// To modulate animation with respect to frame rate

	// Use this for initialization
	void Start () 
	{
		
		
	}
	
	// Update is called once per frame
	void Update () 
	{

        
        
        
	}

    //Called when bullet collides with an object. Not finished.
    void OnTriggerEnter2D(Collider2D target)
    {
        print("Collided with " + target.name + target.tag);
        if (target.tag == "Wall")
        {
			Destroy(this.gameObject);
		}
		else if (target.tag == "Enemy")
		{
			
			//Destroy(target.gameObject);
			Opponent opponent = target.gameObject.GetComponent<Opponent>();
			opponent.EnemyHit();
			Destroy(this.gameObject);
		}

    }	
}
