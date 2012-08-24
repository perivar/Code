using System;
using CommonUtils;

using CommonUtils.Audio.NAudio;


namespace SIR2_True_Stereo_Converter
{
	class Program
	{
		public static void Main(string[] args)
		{
			string inLeft = @"F:\SAMPLES\IMPULSE RESPONSES\PER IVAR IR SAMPLES\ALTIVERB-QUAD-IMPULSE-RESPONSES\Scoring Stages (Orchestral Studios)_Todd-AO - California US_st to st wide mics at 18m90_L.wav";
			string inRight = @"F:\SAMPLES\IMPULSE RESPONSES\PER IVAR IR SAMPLES\ALTIVERB-QUAD-IMPULSE-RESPONSES\Scoring Stages (Orchestral Studios)_Todd-AO - California US_st to st wide mics at 18m90_R.wav";
			
			float[] channel1;
			float[] channel2;
			float[] channel3;
			float[] channel4;
			AudioUtilsNAudio.SplitStereoWaveFileToMono(inLeft, out channel1, out channel2);
			AudioUtilsNAudio.SplitStereoWaveFileToMono(inRight, out channel3, out channel4);
			
			// find out what channel is longest
			int maxLength = Math.Max(channel1.Length, channel3.Length);
			
			string outputFile = @"Scoring Stages (Orchestral Studios)_Todd-AO - California US_st to st wide mics at 18m90V2.sir";
			
			//string cfh_data = @"abe6f4214362.34U4T9yaBCCDEuyH0uCm7T6Pf.z+PqR.kBAoHEfz3DlPB4lX.K4XirMP+3WCzJZ6RYE0ka38ty94e5j858dEG1RUZlT3iZV2EATQgrjIV5ixyF5zA0qasZdXlJpZ8FtlNjwomlnU8lHn+JhPP48khE9n1HPSpVyoJhg5itqiqqKp600.vKJEevIQJ4fXS0aTksgilV6fMkL4wNFPLDn33w5irgZ7lpiNZaJe791OXu1ATcghs1bHHwzElL49JBlnXKYBBeeT8QCedFNXTRbHdVznj7XbHjFhSlLFaURBSgnoA1RJ7UWAwYQSCSew407fANeNiyoYvERkkO.1PVR0vMSTEqnZihvsR6eC5ammIKKcBl.NPeBmsPpDLBjimqMfQB15NVIEpXEZfXflcpdxcd77xh56HaQM9Shz7rIRJa4p+EHo0YfjCv3BeKI87QR6yGIW1qI+hIdM99OMVIuF+7+KqzUe.bo2V9C";
			string cfh_data = @"abe6f42143                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      ";
			
			BinaryFile binaryFile = new BinaryFile(outputFile, BinaryFile.ByteOrder.LittleEndian, true);
			binaryFile.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			binaryFile.Write("\n\n");
			binaryFile.Write("<SirImpulseFile version=\"2.1\" cfh_data=\"");
			binaryFile.Write(cfh_data);
			binaryFile.Write("\"/>");
			binaryFile.Write("\r\n");
			binaryFile.Write((byte)0);

			WriteAudioBlock(binaryFile, channel1, maxLength, "0");
			WriteAudioBlock(binaryFile, channel2, maxLength, "1");
			WriteAudioBlock(binaryFile, channel3, maxLength, "2");
			WriteAudioBlock(binaryFile, channel4, maxLength, "3");
			
			binaryFile.Close();
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}

		private static void WriteAudioBlock(BinaryFile binaryFile, float[] audioData, int length, string channel) {
			binaryFile.Write("BLOCK");
			binaryFile.Write((byte)0);
			
			string lengthText = length.ToString();
			int lenghtTextLength = lengthText.Length;
			int blockLength = length*4+lenghtTextLength+13;
			string blockLengthText = blockLength.ToString();
			
			binaryFile.Write(blockLengthText); // length from where "AUDIODATA" starts
			binaryFile.Write((byte)0);
			binaryFile.Write("AUDIODATA");
			binaryFile.Write((byte)0);
			binaryFile.Write(channel); // channel text ("0" - "3")
			binaryFile.Write((byte)0);
			binaryFile.Write(lengthText);
			binaryFile.Write((byte)0);
			
			// Example:
			// channel 1 mono (L - 1)
			// length 701444 bytes / 4 bytes per float = 175361 float's
			WriteAudioData(binaryFile, audioData, length);
		}
		
		private static void WriteAudioData(BinaryFile binaryFile, float[] audioData, int length) {
			for (int i = 0; i < audioData.Length; i++) {
				binaryFile.Write(audioData[i]);
			}
			// pad if neccesary
			if (length > audioData.Length) {
				for (int i = 0; i < length - audioData.Length; i++) {
					binaryFile.Write(0);
				}
			}
		}
	}
}