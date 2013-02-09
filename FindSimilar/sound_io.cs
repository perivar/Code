using System;
using CommonUtils;

public static class SoundIO
{
	// compression code, 2 bytes
	//static int WAVE_FORMAT_UNKNOWN           =  0x0000; /* Microsoft Corporation */
	static int WAVE_FORMAT_PCM               =  0x0001; /* Microsoft Corporation */
	//static int WAVE_FORMAT_ADPCM             =  0x0002; /* Microsoft Corporation */
	static int WAVE_FORMAT_IEEE_FLOAT        =  0x0003; /* Microsoft Corporation */
	
	/*
	 * Native Formats
	 * Number of Bits	MATLAB Data Type			Data Range
	 * 8				uint8 (unsigned integer) 	0 <= y <= 255
	 * 16				int16 (signed integer) 		-32768 <= y <= +32767
	 * 24				int32 (signed integer) 		-2^23 <= y <= 2^23-1
	 * 32				single (floating point) 	-1.0 <= y < +1.0
	 * 
	 * typedef uint8_t  u8_t;     ///< unsigned 8-bit value (0 to 255)
	 * typedef int8_t   s8_t;     ///< signed 8-bit value (-128 to +127)
	 * typedef uint16_t u16_t;    ///< unsigned 16-bit value (0 to 65535)
	 * typedef int16_t  s16_t;    ///< signed 16-bit value (-32768 to 32767)
	 * typedef uint32_t u32_t;    ///< unsigned 32-bit value (0 to 4294967296)
	 * typedef int32_t  s32_t;    ///< signed 32-bit value (-2147483648 to +2147483647)
	 */
	public static void Read8Bit(BinaryFile wavfile, float[][] sound, int samplecount, int channels)
	{
		int i = 0;
		int ic = 0;
		byte @byte = new byte();

		#if DEBUG
		Console.Write("Read8Bit...\n");
		#endif

		for (i = 0; i<samplecount; i++) {
			for (ic = 0;ic<channels;ic++)
			{
				@byte = wavfile.ReadByte();
				sound[ic][i] = (float) @byte/128.0f - 1.0f;
			}
		}
	}
	
	public static void Write8Bit(BinaryFile wavfile, float[][] sound, int samplecount, int channels)
	{
		int i = 0;
		int ic = 0;
		double val;
		byte @byte = new byte();

		#if DEBUG
		Console.Write("Write8Bit...\n");
		#endif

		for (i = 0; i<samplecount; i++) {
			for (ic = 0;ic<channels;ic++)
			{
				val = Util.RoundOff((sound[ic][i]+1.0)*128.0);
				
				if (val>255)
					val = 255;
				if (val<0)
					val = 0;

				@byte = (byte) val;

				wavfile.Write(@byte);
			}
		}
	}
	
	public static void Read16Bit(BinaryFile wavfile, float[][] sound, int samplecount, int channels)
	{
		int i = 0;
		int ic = 0;

		#if DEBUG
		Console.Write("Read16Bit...\n");
		#endif

		for (i = 0; i<samplecount; i++) {
			for (ic = 0; ic<channels; ic++) {
				float d = (float) wavfile.ReadInt16();
				d = d / 32768.0f;
				sound[ic][i] = d;
			}
		}
	}
	
	public static void Write16Bit(BinaryFile wavfile, float[][] sound, int samplecount, int channels)
	{
		int i = 0;
		int ic = 0;
		double val;

		#if DEBUG
		Console.Write("Write16Bit...\n");
		#endif

		for (i = 0; i<samplecount; i++) {
			for (ic = 0;ic<channels;ic++)
			{
				val = Util.RoundOff(sound[ic][i]*32768.0);
				
				if (val>32767.0)
					val = 32767.0;
				if (val<-32768.0)
					val = -32768.0;

				wavfile.Write((Int16) val);
			}
		}
	}
	
	public static void Read32Bit(BinaryFile wavfile, float[][] sound, int samplecount, int channels)
	{
		int i = 0;
		int ic = 0;

		#if DEBUG
		Console.Write("Read32Bit...\n");
		#endif

		for (i = 0;i<samplecount;i++) {
			for (ic = 0;ic<channels;ic++)
			{
				float d = (float) wavfile.ReadInt32();
				d = d / 2147483648.0f;
				sound[ic][i] = d;
			}
		}
	}
	
	public static void Write32Bit(BinaryFile wavfile, float[][] sound, int samplecount, int channels)
	{
		int i = 0;
		int ic = 0;
		double val;

		#if DEBUG
		Console.Write("Write32Bit...\n");
		#endif

		for (i = 0; i<samplecount; i++) {
			for (ic = 0;ic<channels;ic++)
			{
				val = Util.RoundOff(sound[ic][i]*2147483648.0);
				
				if (val>2147483647.0)
					val = 2147483647.0;
				if (val<-2147483648.0)
					val = -2147483648.0;

				wavfile.Write((int) val);
			}
		}
	}

	public static void Read32BitFloat(BinaryFile wavfile, float[][] sound, int samplecount, int channels)
	{
		int i = 0;
		int ic = 0;

		#if DEBUG
		Console.Write("Read32BitFloat...\n");
		#endif

		for (i = 0;i<samplecount;i++) {
			for (ic = 0;ic<channels;ic++)
			{
				float d = (float) wavfile.ReadSingle();
				sound[ic][i] = d;
			}
		}
	}
	
	public static void Write32BitFloat(BinaryFile wavfile, float[][] sound, int samplecount, int channels)
	{
		int i = 0;
		int ic = 0;

		#if DEBUG
		Console.Write("Write32BitFloat...\n");
		#endif

		for (i = 0; i<samplecount; i++) {
			for (ic = 0;ic<channels;ic++)
			{
				wavfile.Write((float)sound[ic][i]);
			}
		}
	}

	public static float[][] ReadWaveFile(String wavfile, ref int channels, ref int samplecount, ref int samplerate)
	{
		RiffRead riffRead = new RiffRead(wavfile);
		riffRead.Process();
		channels = riffRead.Channels;
		samplecount = riffRead.SampleCount;
		samplerate = riffRead.SampleRate;
		return riffRead.SoundData;
	}

	public static float[][] ReadWaveFile(BinaryFile wavfile, ref int channels, ref int samplecount, ref int samplerate)
	{
		float[][] sound;
		int i = 0;
		int ic = 0;
		int[] tag = new int[13];
		
		//			Size  Description                  Value
		// tag[0]	4	  RIFF Header				   RIFF (1179011410)
		// tag[1] 	4	  RIFF data size
		// tag[2] 	4	  form-type (WAVE etc)			(1163280727)
		// tag[3] 	4     Chunk ID                     "fmt " (0x666D7420) = 544501094
		// tag[4]	4     Chunk Data Size              16 + extra format bytes
		// tag[5]	2     Compression code             1 - 65,535
		// tag[6]	2     Number of channels           1 - 65,535
		// tag[7]	4     Sample rate                  1 - 0xFFFFFFFF
		// tag[8]	4     Average bytes per second     1 - 0xFFFFFFFF
		// tag[9]	2     Block align                  1 - 65,535 (4)
		// tag[10]	2     Significant bits per sample  2 - 65,535 (32)
		// tag[11]	4	  IEEE = 1952670054 (0x74636166) = fact chunk
		// 				  PCM = 1635017060 (0x61746164)  (datachunk = 1635017060)
		// tag[12] 	4	  IEEE = 4 , 						PCM = 5292000 (0x0050BFE0)

		#if DEBUG
		Console.Write("ReadWaveFile...\n");
		#endif

		for (i = 0; i<13; i++) // tag reading
		{
			tag[i] = 0;

			if ((i == 5) || (i == 6) || (i == 9) || (i == 10)) {
				tag[i] = Util.ReadUInt16(wavfile);
			} else {
				tag[i] = (int) Util.ReadUInt32(wavfile);
			}
		}

		//********File format checking********
		if (tag[0]!=1179011410 || tag[2]!=1163280727)
		{
			Console.Error.WriteLine("This file is not in WAVE format\n");
			Util.WinReturn();
			Environment.Exit(1);
		}

		if (tag[3]!=544501094 || tag[4]!=16 || tag[11]!=1635017060) //
		{
			Console.Error.WriteLine("This WAVE file format is not currently supported\n");
			Util.WinReturn();
			Environment.Exit(1);
		}

		if (tag[10]==24)
		{
			Console.Error.WriteLine("24 bit PCM WAVE files is not currently supported\n");
			Util.WinReturn();
			Environment.Exit(1);
		}
		
		if (tag[5] != WAVE_FORMAT_PCM) {
			Console.Error.WriteLine("Non PCM WAVE files is not currently supported\n");
			Util.WinReturn();
			Environment.Exit(1);
		}
		
		//--------File format checking--------

		channels = tag[6];

		samplecount = tag[12]/(tag[10]/8) / channels;
		samplerate = tag[7];

		sound = new float[channels][];
		
		for (ic = 0; ic < channels; ic++) {
			sound[ic] = new float[samplecount];
		}

		//********Data loading********
		if (tag[10] == 8) {
			Read8Bit(wavfile, sound, samplecount, channels);
		}
		if (tag[10] == 16) {
			Read16Bit(wavfile, sound, samplecount, channels);
		}
		if (tag[10] == 32) {
			if (tag[5] == WAVE_FORMAT_PCM) {
				Read32Bit(wavfile, sound, samplecount, channels);
			} else if (tag[5] == WAVE_FORMAT_IEEE_FLOAT) {
				Read32BitFloat(wavfile, sound, samplecount, channels);
			}
		}
		
		//--------Data loading--------

		wavfile.Close();
		return sound;
	}
	
	public static void WriteWaveFile(BinaryFile wavfile, float[][] sound, int channels, int samplecount, int samplerate, int format_param)
	{
		int i = 0;
		int[] tag = {1179011410, 0, 1163280727, 544501094, 16, 1, 1, 0, 0, 0, 0, 1635017060, 0, 0};

		#if DEBUG
		Console.Write("WriteWaveFile...\n");
		#endif

		//********WAV tags generation********

		tag[12] = samplecount*(format_param/8)*channels;
		tag[1] = tag[12]+36;
		tag[7] = samplerate;
		tag[8] = samplerate *format_param/8;
		tag[9] = format_param/8;
		tag[6] = channels;
		tag[10] = format_param;

		if ((format_param == 8) || (format_param == 16))
			tag[5] = 1;
		if (format_param == 32)
			tag[5] = 3;
		//--------WAV tags generation--------

		// tag writing
		for (i = 0; i<13; i++) {
			if ((i == 5) || (i == 6) || (i == 9) || (i == 10)) {
				Util.WriteUInt16((ushort)tag[i], wavfile);
			} else {
				Util.WriteUInt32((uint)tag[i], wavfile);
			}
		}

		if (format_param == 8)
			Write8Bit(wavfile, sound, samplecount, channels);
		if (format_param == 16)
			Write16Bit(wavfile, sound, samplecount, channels);
		if (format_param == 32)
			Write32BitFloat(wavfile, sound, samplecount, channels);

		wavfile.Close();
	}
	
	public static int GetWaveOutParameters()
	{
		int bps = 0;

		do
		{
			Console.Write("Bits per sample (8/16/32) [16] : ");
			bps = (int) Util.GetFloat();
			if (bps == 0) {
				bps = 16;
			}
		}
		while (bps!=8 && bps!=16 && bps!=32);

		return bps;
	}
}
