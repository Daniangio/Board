using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arduino;


public class BoardManager : MonoBehaviour {

	//AUDIO
	private AudioSource[] audioSources;
	private AudioClip[,] audios_0;
	private AudioClip[,] audios_1;
	private int num_instruments = 2;

	//Interface to arduino
	private ArduinoPersistent ap;
	private int num_variables = 1; //Variables other than sounds
	private int num_notes = 1;
	private int buffer_dimension = 3;
	private float[,] data; //This is a Matrix: on ROWS you have the various variables related to board data,				1 0 1  <- Means that variable 0 value was 1, then 0 and now 1
							//i.e. 1 or 0 for each button. On COLUMNS you have the last 3 values of those variables			0 0 0
							//																								1 1 1
	private int buffer_pointer = 0;
	private bool buffer_ready = false;

	//Start is called once, when the scene begins
	void Start () {

		ap = (ArduinoPersistent)GameObject.Find ("ArduinoPersistent").GetComponent<ArduinoPersistent>();

		audioSources = new AudioSource[num_notes];
		for (int i = 0; i < num_notes; i++)
			audioSources [i] = gameObject.AddComponent<AudioSource> ();

		audios_0 = new AudioClip[num_notes, num_instruments];
		audios_1 = new AudioClip[num_notes, num_instruments];
		for (int i = 0; i < num_notes; i++)
			for (int j = 0; j < num_instruments; j++) {
				audios_0 [i, j] = (AudioClip)Resources.Load ("Audios/0/audio_" + i + "_" + j);  // ex. audio_0_0 is the audio of the first note with the first instrument
				audios_1 [i, j] = (AudioClip)Resources.Load ("Audios/1/audio_" + i + "_" + j);  // ex. audio_2_1 is the audio of the third note with the second instrument
			}

		data = new float[num_notes + num_variables, buffer_dimension];

	}

	//Update is called once per frame
	void Update () {

		ReadFromArduino ();

		if (buffer_ready)
			CheckSoundsToPlay ();


		/*JUST FOR TESTING
		if (Input.GetKeyDown (KeyCode.A)) {
			...
		}

		if (Input.GetMouseButtonDown (0)) {
			...
		}*/

	}

	private void ReadFromArduino() {
		string response = ap.ReadFromArduino ();
		if (response != null) {
			int type = 0;
			try {type = int.Parse (response.Split ("," [0]) [0]);} catch {Debug.Log(response);}
			switch(type) {
			case (12):
				for (int i=0; i<num_notes + num_variables; i++)
					float.TryParse (response.Split ("," [0]) [i+1], out data[i, buffer_pointer]);

				if (buffer_pointer == buffer_dimension - 1) {
					buffer_pointer = 0;
					buffer_ready = true;
				}
				else
					buffer_pointer += 1;
				
				Debug.Log (response);
				break;

			default:
				Debug.Log (response);
					break;
			}
		}
	}

	private void CheckSoundsToPlay() {
		int counter;
		for (int i = 1; i < num_notes + num_variables; i++) { //Start from 1 because variable number 0 is the one of the instrument
			
			counter = 0;
			for (int j = 0; j < buffer_dimension; j++) {
				counter += (int)data [i, j];
			}

			if (counter == buffer_dimension) {
				if (!audioSources [i - 1].isPlaying) {
					audioSources [i - 1].clip = audios_1 [i - 1, (int)data [0, buffer_dimension - 1]];
					audioSources [i - 1].Play ();
					// Do some visual things ...
				}
			} else if (counter == 0) {
				if (!audioSources [i - 1].isPlaying) {
					audioSources [i - 1].clip = audios_0 [i - 1, (int)data [0, buffer_dimension - 1]];
					audioSources [i - 1].Play ();
					// Do some visual things ...
				}
			} else {
				if (audioSources[i - 1].isPlaying) {
					audioSources [i - 1].Stop ();
					// Stop some visual things ...
				}
			}
		}
	}

	//Methods for Arduino

	public void WriteOnArduino(string command) {
		ap.WriteOnArduino (command);
	}

	/*void OnDestroy ()
	{
		if (ap != null) {
			ap.SwitchOff ();
			ap.Destroy ();
		}
	}*/

	void OnApplicationQuit ()
	{
		if (ap != null) {
			ap.SwitchOff ();
			ap.Destroy ();
		}
	}

	public void ShowEffect(int effectNumber, int millis = 10000) {
		ap.ShowEffect (2, effectNumber, millis);
	}

}
