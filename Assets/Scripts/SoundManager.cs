using UnityEngine;
using System.Collections;

// Manages music, sound effects and plays audio clips
public class SoundManager : MonoBehaviour 
{
	public AudioSource efxSource;			// Sound effect source
	public AudioSource musicSource;			// Background music source
	public static SoundManager instance = null;	// set it public for other settings to use SoundMangers easily

	// pitch factor for random uses later
	public float lowPitchRange = .95f;
	public float highPitchRange = 1.05f;

	// Use this for initialization
	void Awake () 
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);

		efxSource.volume = PlayerPrefs.GetFloat ("tower_of_abel_volume");
		musicSource.volume = PlayerPrefs.GetFloat ("tower_of_abel_volume");
	}

	//Play GameMusic
	public void PlaySingle(AudioClip clip)
	{
		efxSource.clip = clip;
		efxSource.Play ();
	}

	//Play a random sound effect with the use of pitch
	public void RandomizeSfx (params AudioClip [] clips)
	{
		int randomIndex = Random.Range (0, clips.Length);
		float randomPitch = Random.Range (lowPitchRange, highPitchRange);

		efxSource.pitch = randomPitch;
		efxSource.clip = clips [randomIndex];
		efxSource.Play ();
	}
}
