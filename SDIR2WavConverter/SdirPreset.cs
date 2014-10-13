using System;
using CommonUtils;

namespace SDIR2WavConverter
{
	/// <summary>
	/// Description of SdirPreset.
	/// </summary>
	public class SdirPreset
	{
		public SdirPreset()
		{
		}
		
		public bool Read(string filePath)
		{
			BinaryFile bFile = new BinaryFile(filePath, BinaryFile.ByteOrder.LittleEndian);
			
			return false;
		}

		public bool Write(string filePath)
		{
			throw new NotImplementedException();
		}
	}
}
