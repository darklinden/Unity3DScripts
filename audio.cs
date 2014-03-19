using UnityEngine;
using System.Collections;

public class audio : MonoBehaviour
{
	public AudioSource music;
	public float musicVolume;

	// Use this for initialization
	void Start ()
	{
		musicVolume = 1f;
		music.Play ();
	}

	// Update is called once per frame
	void Update ()
	{
	}
}
