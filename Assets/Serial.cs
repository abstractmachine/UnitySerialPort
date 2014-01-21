using UnityEngine;
using System.Collections;

using System.IO.Ports; 
// System.IO.Ports requires a working Serial Port. On Mac, you will need to purcase the Uniduino plug-in on the Unity Store
// This adds a folder + a file into your local folder at ~/lib/libMonoPosixHelper.dylib
// This file will activate your serial port for C# / .NET
// The functions are the same as the standard C# SerialPort library
// cf. http://msdn.microsoft.com/en-us/library/system.io.ports.serialport(v=vs.110).aspx


public class Serial : MonoBehaviour {

	SerialPort serial;

	public string serialIn = "";
	string serialOut = "";

	public int ReceivedBytesCount { get { return serialIn.Length; } }
	public string ReceivedBytes { get { return serialIn; } }


	void Start() {

		string portName = GetPortName();

		if (portName == "") {
			print("Error: Couldn't find serial port.");
			return;
		}

		serial = new SerialPort(portName, 9600);

		serial.Open();
		serial.ReadTimeout = 10;

		StartCoroutine(ReadSerialLoop());

	}


	public void OnApplicationQuit () {

		if(serial != null && serial.IsOpen) serial.Close();

	}


	void Update() {

		/*if(serial.IsOpen && serial != null) {

			try {
				serialIn = serial.ReadLine();
			} catch(System.Exception) {

			}

		}*/

	}


	public IEnumerator ReadSerialLoop() {

    	while(true) {

			try {
				serialIn = serial.ReadLine();
			} catch(System.Exception) {

			}

        	yield return null;
    	}

	}


	public void Write(string message, bool overwriteCurrentValue=true) {



	}





	string GetPortName() {

		string[] portNames;

		switch (Application.platform) {

			case RuntimePlatform.OSXPlayer:
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.OSXDashboardPlayer:
			case RuntimePlatform.LinuxPlayer:

				portNames = System.IO.Ports.SerialPort.GetPortNames();
				
				if (portNames.Length ==0) {
				        portNames = System.IO.Directory.GetFiles("/dev/");                
				}
                     
				foreach (string portName in portNames) {                                
				        if (portName.StartsWith("/dev/tty.usb") || portName.StartsWith("/dev/ttyUSB")) return portName;
				}                
				return ""; 

			default: // Windows

				portNames = System.IO.Ports.SerialPort.GetPortNames();
				    
				if (portNames.Length > 0) return portNames[0];
				else return "COM3";

		}

	}

}
