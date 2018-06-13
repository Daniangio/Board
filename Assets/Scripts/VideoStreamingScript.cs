using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoStreamingScript : MonoBehaviour {

	public string nextScene;
	public RawImage image;

	public VideoClip videoToPlay;

	private VideoPlayer videoPlayer;
	private VideoSource videoSource;
	private AudioSource audioSource;


	void Start () {
		Application.runInBackground = true;
		StartCoroutine (PlayVideo ());
	}

	IEnumerator PlayVideo() {

		//Add VideoPlayer to the GameObject
		videoPlayer = gameObject.AddComponent<VideoPlayer>();

		//Add AudioSource
		audioSource = gameObject.AddComponent<AudioSource>();

		//Disable Play on Awake for both Video and Audio
		videoPlayer.playOnAwake = false;
		audioSource.playOnAwake = false;
		audioSource.Pause();

		//We want to play from video clip not from url
		videoPlayer.source = VideoSource.VideoClip;

		// Vide clip from Url
		//videoPlayer.source = VideoSource.Url;
		//videoPlayer.url = "http://www.quirksmode.org/html5/videos/big_buck_bunny.mp4";

		//Set Audio Output to AudioSource
		videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

		//Assign the Audio from Video to AudioSource to be played
		videoPlayer.EnableAudioTrack(0, true);
		videoPlayer.SetTargetAudioSource(0, audioSource);

		//Set video To Play then prepare Audio to prevent Buffering
		videoPlayer.clip = videoToPlay;
		videoPlayer.Prepare();



		//Wait until video is prepared
		while (!videoPlayer.isPrepared)	{
			yield return null;
		}

		gameObject.GetComponent<RawImage> ().enabled = true;

		//Assign the Texture from Video to RawImage to be displayed
		image.texture = videoPlayer.texture;

		videoPlayer.isLooping = false;

		//Play Video
		videoPlayer.Play();

		//Play Sound
		audioSource.Play();


		while (videoPlayer.isPlaying) {
			//Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)videoPlayer.time));
			yield return null;
		}

		SceneManager.LoadScene (nextScene);

	}

	void Update () {

	}

}