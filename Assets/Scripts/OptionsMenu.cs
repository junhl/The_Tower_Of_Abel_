using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class OptionsMenu : MonoBehaviour {

	public static float volumeLevel;
	public float sliderValue;
	public Slider aSlider;

	// Use this for initialization
	void Start () {
		aSlider.value = PlayerPrefs.GetFloat ("tower_of_abel_volume");
	}
	
	// Update is called once per frame
	void Update () {
		// when user want to skip, they will press escape key
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.LoadLevel (0);
		}
	}

	//save volume to playerprefs whenever it is changed
	public void OnValueChanged(float newValue)
	{
		print (newValue);
		PlayerPrefs.SetFloat ("tower_of_abel_volume", newValue);
		print("set volume to : " + PlayerPrefs.GetFloat("tower_of_abel_volume"));
	}

	//change resolution using dropdown menu
	public void OnResolutionChanged(int newDropdown)
	{
		// we currently have 3 resolution settings that the user can change 
		switch (newDropdown) {
		case 0:
			Screen.SetResolution (640, 480, false);
			break;
		case 1:
			Screen.SetResolution (800, 600, false);
			break;
		case 2:
			Screen.SetResolution (1024, 768, false);
			break;
		}
	}
}
