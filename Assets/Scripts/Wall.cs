using UnityEngine;
using System.Collections;

// The board has made of ROom, Corridors, WAll. Player can move in room and corridors but NOT in walls. It serves to block player to move too freely
public class Wall : MonoBehaviour 
{
	public Sprite dmgSprite;			//Alternate sprite for later use
	public int hp = 999;				//hit points for the wall. But you don't want it to be destoryed. So High HP
	public AudioClip chopSound1;			// 1 of 2 audio clips to play when player attacks wall
	public AudioClip chopSound2;			// 1 of 2 audio clips to play when player attacks wall

	private SpriteRenderer spriteRenderer;		// component reference to the attached SpriteRenderer.

	// Use this for initialization
	void Awake () 
	{
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}
	
	//Wall can be destoryed if player attacks it enough....that is 999 times. Can be used as a hidden achievement
	public void DamageWall (int loss)
	{
		SoundManager.instance.RandomizeSfx (chopSound1, chopSound2);
		spriteRenderer.sprite = dmgSprite;
		hp -= loss;
		if (hp <= 0)
		{
			gameObject.SetActive (false);
		}
	}
}
