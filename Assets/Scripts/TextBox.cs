using UnityEngine;
using System.Collections;

// This class is short and simple to create and GUI TextField for developers to fill up the field so player to see on gameplay
public class TextBox : MonoBehaviour {
	public string stringToEdit = "This is a test";
	void OnGui()
	{
		stringToEdit = GUI.TextField (new Rect (10, 10, 200, 20), stringToEdit, 25);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
