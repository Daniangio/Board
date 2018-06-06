using System;

namespace Arduino
{
	public enum Commands
	{
		Acknowledge=0,
		Error=1,
		LedNumberRequest=2,
		LedNumberResponse=3,
		LightUpLed=4,
		SetIntensity=5,
		SetBrightness=6,
		SetColor=7,
		StartBar=8,
		ShowEffect=9,
		GetStatus=10,
		SwitchOff=11,
		BoardState=12,
	};

}