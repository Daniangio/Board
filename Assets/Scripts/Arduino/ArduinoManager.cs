using System;
using System.Threading;
using UnityEngine;

namespace Arduino
{
	public class ArduinoManager
	{
		private ArduinoConnector _connector;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:LedBarManager.LedBarManager"/> class.
		/// Default Baud rate 9600.
		/// </summary>
		/// <param name="portName">Port name.</param>
		public ArduinoManager(string portName)
		{
			_connector = new ArduinoConnector(portName);
			Setup ();
			/*Thread t = new Thread (Setup);
			t.Start();
			WaitForThread (t);*/
		}

		private void Setup()
		{
			_connector.Setup();
			//while (!_connector.isReady()) {	};
		}

		private void WaitForThread (Thread t)
		{
			while (!t.Join (TimeSpan.FromSeconds (5))) {
				t.Abort ();
				Debug.Log ("ASPETTO");
				t = new Thread (Setup);
				t.Start();
			}
		}

		public void WriteOnArduino(string command) {
			_connector.WriteOnArduino(command);
		}

		public string ReadFromArduino() {
			return _connector.ReadFromArduino ();
		}

		/// <summary>
		/// Starts the progress bar to display score.
		/// Default colors are used if not previously set.
		public void StartBar()
		{
			_connector.StartBar();
		}

		/// <summary>
		/// Sets the intensity of the light (the number of lit leds).
		/// </summary>
		/// <param name="intensity">intensity value: 1 to 4.</param>
		public void SetIntensity(int intensity)
		{
			_connector.SetIntensity(intensity);
		}

		/// <summary>
		/// Sets the brightness of the light.
		/// </summary>
		/// <param name="brightness">brightness value: 1 to 200.</param>
		public void SetBrightness(int brightness)
		{
			_connector.SetBrightness(brightness);
		}

		/// <summary>
		/// Sets the color of the team.
		/// </summary>
		/// <param name="hexColor">HEX color string.</param>
		public void SetColor(string hexColor)
		{
			_connector.SetColor(hexColor);
		}


		/// <summary>
		/// Resets the progress bar.
		/// If game is on it brings back score to 0.
		/// </summary>
		public void ResetProgressBar()
		{
			_connector.StartBar();
		}

		/// <summary>
		/// Shows the effect.
		/// </summary>
		/// <param name="barPortion">Bar portion (0- first player, 1-second player, 2-entire bar).</param>
		/// <param name="effectNumber">Effect number.</param>
		/// <param name="duration">Duration in milliseconds. (default 5000ms)</param>
		public void ShowEffect(int barPortion, int effectNumber, int duration = 5000)
		{
			_connector.ShowEffect(barPortion, effectNumber, duration);
		}

		/// <summary>
		/// Close this instance.
		/// </summary>
		public void SwitchOff(){
			_connector.SwitchOff();
		}

		public void Destroy(){
			_connector.CloseConnector();
		}

		public void CloseConnection() {
			_connector.CloseConnector();
		}

	}
}