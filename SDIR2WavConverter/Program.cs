using System;

using CommonUtils.Audio.NAudio;

namespace SDIR2WavConverter
{
	class Program
	{
		public static void Main(string[] args)
		{
			
			SdirPreset sdir = new SdirPreset();
			sdir.Read(@"..\..\Resources\0.2s_Closet B.SDIR");
			
			string fileName = @"..\..\Resources\0.2s_Closet B.wav";
			AudioUtilsNAudio.CreateWaveFile(sdir.WaveformData, fileName, new NAudio.Wave.WaveFormat(sdir.SampleRate, sdir.BitsPerSample, sdir.Channels));
		}
		
	}
}