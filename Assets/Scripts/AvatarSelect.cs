using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AvatarSelect : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	/*
	 * Set PlayerPrefs variable depending on which Gender is selected
	 * 
	 */
	public void selectedAvatar(string pName)
	{
		if (pName == "Male") {
			PlayerPrefs.SetInt ("tower_of_abel_male", 1);
			print ("Avatar set to Abel.");
		} else if (pName == "Female") {
			PlayerPrefs.SetInt ("tower_of_abel_male", 0);
			print ("Avatar set to Abelia.");
		}


	}
}
