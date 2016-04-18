using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Load a Scene in the hierarchy depending on input value.
	public void LoadLevel(int level)
	{
		Application.LoadLevel (level);
	}
}
