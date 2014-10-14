using System;
using System.Collections.Generic;
using CommonUtils;

namespace SDIR2WavConverter
{
	/// <summary>
	/// Description of SdirPreset.
	/// </summary>
	public class SdirPreset
	{
		List<string> comments = new List<string>();
		int channels;
		int numSampleFrames;
		int bitsPerSample;
		int sampleRate;
		byte[] waveformData;

		public List<string> Comments
		{
			get{ return this.comments;  }
			set{ this.comments = value; }
		}

		public int Channels
		{
			get{ return this.channels;  }
			set{ this.channels = value; }
		}

		public int SampleFrames
		{
			get{ return this.numSampleFrames;  }
			set{ this.numSampleFrames = value; }
		}

		public int BitsPerSample
		{
			get{ return this.bitsPerSample;  }
			set{ this.bitsPerSample = value; }
		}
		
		public int SampleRate
		{
			get{ return this.sampleRate;  }
			set{ this.sampleRate = value; }
		}
		
		public byte[] WaveformData
		{
			get{ return this.waveformData;  }
			set{ this.waveformData = value; }
		}
		
		public SdirPreset()
		{
		}
		
		public bool Read(string filePath)
		{
			BinaryFile bFile = new BinaryFile(filePath, BinaryFile.ByteOrder.BigEndian);
			
			string firstChunkID = bFile.ReadString(4); // chunk ID	= "FORM"
			int ckSize = bFile.ReadInt32();
			string formType = bFile.ReadString(4);
			
			// read first data chunk
			string chunkID = bFile.ReadString(4);
			
			// if chunkID == "COMT" then CommentsChunk
			if (chunkID.Equals("COMT")) {
				long curposTmpComt = bFile.GetPosition();
				bFile.Seek(curposTmpComt-4);
				
				// CommentsChunk
				string chunkIDComt = bFile.ReadString(4); // chunk ID	= "COMT"
				int chunkSizeComt = bFile.ReadInt32();
				int numComments = bFile.ReadUInt16();
				long curposTmpComt2 = bFile.GetPosition();

				//comments = bFile.ReadString(chunkSizeComt-2);
				for (int i = 0; i < numComments; i++) {
					int commentTimestamp = (int) bFile.ReadUInt32();
					string marker = bFile.ReadString(4);
					int count = (int) bFile.ReadByte();
					comments.Add(bFile.ReadString(count));
				}
				
				bFile.Seek(curposTmpComt2+chunkSizeComt-2);
			}
			
			string chunkID2 = bFile.ReadString(4);

			// if chunkID2 == "COMM" then CommonChunk
			if (chunkID2.Equals("COMM")) {
				long curposTmpComm = bFile.GetPosition();
				bFile.Seek(curposTmpComm-4);
				
				// CommonChunk
				string chunkIDComm = bFile.ReadString(4); // chunk ID = "COMM"
				int chunkSizeComm = bFile.ReadInt32();
				
				channels = bFile.ReadInt16();
				numSampleFrames = (int) bFile.ReadUInt32();
				bitsPerSample = bFile.ReadInt16();

				byte[] sampleRateBytes1 = bFile.ReadBytes(2);
				sampleRate = bFile.ReadUInt16();
				byte[] sampleRateBytes2 = bFile.ReadBytes(6);
			}
			
			string chunkID3 = bFile.ReadString(4);
			
			// if chunkID3 == "SSND" then SoundDataChunk
			if (chunkID3.Equals("SSND")) {
				long curposTmpSsnd = bFile.GetPosition();
				bFile.Seek(curposTmpSsnd-4);
				
				// SoundDataChunk
				string chunkIDSsnd = bFile.ReadString(4); // chunk ID = "SSND"
				int chunkSizeSsnd = bFile.ReadInt32();
				
				int offset = (int) bFile.ReadUInt32();
				int blocksize = (int) bFile.ReadUInt32();
				byte[] data = bFile.ReadBytes(offset, chunkSizeSsnd-8, BinaryFile.ByteOrder.LittleEndian);
				
				// swap waveform data
				WaveformData = SwapAiffEndian(data);
			}
			
			return true;
		}

		private byte[] SwapAiffEndian (byte[] data) {
			
			byte[] swappedData = new byte[data.Length];
			int align = bitsPerSample / 8;

			for (int i = 0; i < data.Length; i++)
			{
				int pos = (int)Math.Floor((double)i / align) * align + (align - (i % align) - 1);
				swappedData[i] = data[pos];
			}
			
			return swappedData;
		}
		
		public bool Write(string filePath)
		{
			throw new NotImplementedException();
		}
	}
}
