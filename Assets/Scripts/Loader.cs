using UnityEngine;
using System.Collections;

// Used solely to instantiate GameManager singleton
public class Loader : MonoBehaviour 
{

	public GameObject gameManager;

	// Check if GameManager is instantiated. If not, instantiate. Singleton Design Pattern.
	void Awake () 
	{
		if (GameManager.instance == null)
		{
			Instantiate (gameManager);
		}
	
	}
}
