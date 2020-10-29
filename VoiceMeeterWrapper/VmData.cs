using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceMeeterWrapper
{
	public class VmData
	{
		public static int InputLevels = 34;
		public static int OutputLevels = 34;
		public static readonly List<string> StripToggles = new List<string>()
		{
			"Mono",
			"Mute",
			"Solo",
			"MC",
			"PostReverb",
			"PostDelay",
			"PostFx1",
			"PostFx2",
			"A1",
			"A2",
			"A3",
			"A4",
			"A5",
			"B1",
			"B2",
			"B3"
		};

		public static readonly List<string> BusToggles = new List<string>()
		{
			"Mono",
			"Mute",
			"EQ.on",
			"Sel"
		};

		public static readonly List<string> StripValues = new List<string>()
		{
			"Gain",
			"Pan_x",
			"Pan_y",
			"Color_x",
			"Color_y",
			"fx_x",
			"fx_y",
			"Comp",
			"Gate",
			"Limit",
			"EQGain1",
			"EQGain2",
			"EQGain3",
			"Reverb",
			"Delay",
			"Fx1",
			"Fx2"
		};

		public static readonly List<string> BusValues = new List<string>()
		{
			"Gain",
			"ReturnReverb",
			"ReturnDelay",
			"ReturnFx1",
			"ReturnFx2"
		};
	}
}
