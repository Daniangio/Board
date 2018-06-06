using System;
using UnityEngine;
using System.Threading;
using System.Collections;
using System.IO.Ports;
using System.Linq;

namespace Arduino
{
	public class SerialSender
	{
		private string _portName;
		private int _baudRate;
		private SerialPort _stream;

		private Thread _thread;
		private Queue _outputQueue;
		private Queue _inputQueue;

		private bool _looping = true;

		public void StartThread(string portName, int baudRate = 9600)
		{
			_portName = portName;
			_baudRate = baudRate;
			_outputQueue = Queue.Synchronized (new Queue ());
			_inputQueue = Queue.Synchronized (new Queue ());

			_thread = new Thread (ThreadLoop);
			_thread.Start ();

		}

		public void StopThread ()
		{
			lock (this)
			{
				_looping = false;
			}
		}

		public bool IsLooping()
		{
			lock (this) {
				return _looping;
			}
		}

		public void ResetQueues()
		{
			_inputQueue.Clear ();
			_outputQueue.Clear ();
		}

		private void WriteToSerial(string command)
		{
			if (_stream.IsOpen) {
				try {
					//Debug.Log(command);
					_stream.Write (command + ";");
				} catch (Exception e) {
					Debug.Log (e.ToString ());
				}
				_stream.BaseStream.Flush ();
			} else
				Debug.Log ("Stream closed");
		}

		private string ReadFromSerial(int timeout = 0)
		{
			_stream.ReadTimeout = timeout;
			try
			{
				return _stream.ReadTo(";");
			}
			catch (TimeoutException)
			{
				return null;
			}
		}

		public void SendToArduino (string command)
		{
			_outputQueue.Enqueue (command);
		}

		public string ReadFromArduino ()
		{
			if (_inputQueue.Count == 0)
				return null;

			return (string)_inputQueue.Dequeue ();
		}

		void OnDestroy() {
			_looping = false;
		}

		private void ThreadLoop ()
		{

			string[] ports = SerialPort.GetPortNames();

			//Debug.Log("The following serial ports were found:");

			// Display each port name to the console.
			foreach(string port in ports)
			{
				//Debug.Log(port);
			}

			_stream = new SerialPort (_portName, _baudRate);
			_stream.ReadTimeout = 10;
			try {_stream.Open ();}catch (Exception e){
				Debug.Log (e.ToString());
			}
			while (IsLooping())
			{
				if (_outputQueue.Count != 0)
				{
					string command = (string)_outputQueue.Dequeue();
					WriteToSerial(command);
				}

				string result = ReadFromSerial(50);
				if (result != null)
					_inputQueue.Enqueue (result);
			}
				
			if (_stream.IsOpen) {
				_stream.Close ();
				Debug.Log ("Stream closed");
			} else
				Debug.Log ("Stream was not open");
		}


	}
}
