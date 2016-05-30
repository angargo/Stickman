using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public AudioClip[] levelMusicChangeArray;

	private AudioSource audioSource;

	void Awake(){
		DontDestroyOnLoad(gameObject);
		Debug.Log("Don't Destroy On Load " + name);
		
	}

	// Use this for initialization
	void Start () {
		audioSource = this.GetComponent<AudioSource>();
		AudioClip thisLevelMusic = levelMusicChangeArray[0];
		if (thisLevelMusic != null){
			audioSource.clip = thisLevelMusic;
			audioSource.loop = true;
			audioSource.Play();
		}
		//audioSource.volume = PlayerPrefsManager.GetMasterVolume();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeVolume (float volume){
		audioSource.volume = volume;
	}

	void OnLevelWasLoaded(int level){
		AudioClip thisLevelMusic = levelMusicChangeArray[level];
		Debug.Log("Playing Clip: " + levelMusicChangeArray[level]);
		if (thisLevelMusic != null){
			audioSource.clip = thisLevelMusic;
			audioSource.loop = true;
			audioSource.Play();
		}
	}
}
