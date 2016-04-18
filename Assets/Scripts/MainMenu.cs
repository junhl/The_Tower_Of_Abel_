using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public AudioSource mainMenuAudio;


	// Use this for initialization
	void Start () {
		mainMenuAudio = GameObject.Find ("Audio Source").GetComponent<AudioSource> ();
		mainMenuAudio.volume = PlayerPrefs.GetFloat ("tower_of_abel_volume");
		print ("current volume is : " + mainMenuAudio.volume);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
