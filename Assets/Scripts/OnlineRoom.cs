using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

class OnlineRoom : NetworkBehaviour
{
	[SyncVar]
	public int numTiles;
}

public class OnlineRoomSpawner : MonoBehaviour {

	public GameObject aTilePrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Spawn()
	{
		GameObject tile = (GameObject)Instantiate (aTilePrefab, transform.position, Quaternion.identity);
		NetworkServer.Spawn (tile);
	}
}
