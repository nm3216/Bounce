using UnityEngine;
using System.Collections;

public class AudioPlay : MonoBehaviour {
	public AudioClip audio;

	public void playSound(AudioClip audio1)
	{
		AudioSource.PlayClipAtPoint(audio1,new Vector3(0,0,0));
	}
	public void playMenuSound()
	{
		AudioSource.PlayClipAtPoint(audio,new Vector3(0,0,0));
	}

}
