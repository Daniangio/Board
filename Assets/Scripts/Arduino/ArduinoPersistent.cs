using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arduino;

public class ArduinoPersistent : MonoBehaviour {

	public static ArduinoPersistent _persistent;

	private static ArduinoManager _am;

	void Awake() {
		
		if (_persistent == null) {
			DontDestroyOnLoad (gameObject);
			_persistent = this;

			_am = new ArduinoManager ("\\\\.\\COM7");
			_am.StartBar ();
			_am.SetIntensity (1);
			_am.SetBrightness (20);

		} else if (_persistent != this) {
			Destroy (gameObject);
		}

	}
	
	public void CloseConnection() {
		_am.CloseConnection ();
	}

	public string ReadFromArduino() {
		return _am.ReadFromArduino ();
	}

	public void WriteOnArduino(string command) {
		_am.WriteOnArduino (command);
	}

	public void SwitchOff() {
		_am.SwitchOff ();
	}

	public void SetIntensity(int intensity) {
		_am.SetIntensity (intensity);
	}

	public void SetColor(string hexColor) {
		_am.SetColor (hexColor);
	}

	public void SetBrightness(int brightness) {
		_am.SetBrightness(brightness);
	}

	public void ShowEffect(int barPortion, int effectNumber, int duration) {
		_am.ShowEffect(barPortion, effectNumber, duration);
	}

	public void Destroy() {
		_am.Destroy ();
	}
}
