using System;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

using CommonUtils.Audio.NAudio;

namespace StereoToQuadJoiner
{
	class Program
	{
		public static void Main(string[] args)
		{
			// choose directory to process
			string directoryPath = @"F:\SAMPLES\IMPULSE RESPONSES\PER IVAR IR SAMPLES\ALTIVERB-OUTPUT-QUAD";
		
			SearchDirAndJoin(directoryPath);
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
		
		private static void SearchDirAndJoin(string directoryPath) {

			// make sure that the directory exists...
			if (directoryPath == null || directoryPath.Length <= 0) {
				directoryPath = PromptForPath("C:\\", "Choose directory to process");
				Console.Out.WriteLine("Directory {0} selected.", directoryPath);
			}
			if (directoryPath == null || directoryPath.Length <= 0) {
				Console.Out.WriteLine("No input directory. Script canceled.");
				return;
			}

			// locate all left files (and then match pairwise afterwards
			DirectoryInfo di = new DirectoryInfo(directoryPath);
			FileInfo[] leftFiles = di.GetFiles("*_L.wav");
			foreach(FileInfo fi in leftFiles)
			{
				string fileNameLeft = fi.Name;
				Console.Out.WriteLine("Left filename:  " +  fileNameLeft);

				// parse the left file name
				string tmpName = Regex.Match(fileNameLeft, @"(.*)_L.wav$").Groups[1].Value;
				string fileNameRight = String.Format("{0}{1}.{2}", tmpName, "_R", "wav");
				Console.Out.WriteLine("Right filename: " +  fileNameRight);
				
				string combinedFileName = String.Format("{0}{1}.{2}", tmpName, "_Quad", "wav");
				Console.Out.WriteLine("Combined filename: " +  combinedFileName);

				// locate the matching right file
				string filePathRightFull = Path.Combine(directoryPath, fileNameRight);
				if (File.Exists(filePathRightFull)) {
					// skip if file exists
					string combinedFileNamePath = Path.Combine(directoryPath, combinedFileName);
					if (!File.Exists(combinedFileNamePath)) {
						// Open the files
						if (CombineStereoToQuad(fi.FullName, filePathRightFull, combinedFileNamePath)) {
							Console.Out.WriteLine("Sucessfully combined the stereo files to quad.");
							Console.Out.WriteLine("----------------------");
						} else {
							Console.Out.WriteLine("Could not combine the stereo files to quad. Script canceled.");
							return;
						}
					} else {
						Console.Out.WriteLine("{0} already exist. Skipping file.", combinedFileNamePath);
					}
				} else {
					Console.Out.WriteLine("No matching right file found ({0}). Skipping file.", filePathRightFull);
					Console.Out.WriteLine("----------------------");
					continue;
				}
			}
		}
		
		private static bool CombineStereoToQuad(string filePathLeft, string filePathRight, string combinedFileNamePath) {
			return false;
		}
		
		public static string PromptForPath(string directoryPath, string szDescription) {

			FolderBrowserDialog dlg = new FolderBrowserDialog();
			if (null != directoryPath && "" != directoryPath) {
				dlg.SelectedPath = directoryPath;
			} else {
				dlg.SelectedPath = @"C:\";
			}

			if (null != szDescription && "" != szDescription) {
				dlg.Description = szDescription;
			} else {
				dlg.Description = "Select a folder";
			}
			
			dlg.ShowNewFolderButton = true;
			DialogResult res = dlg.ShowDialog();
			if (res == DialogResult.OK) {
				return dlg.SelectedPath;
			}
			
			return null;
		}

		/*
	Prompt for a file and return a string
	szFilter can be like this: "txt files (*.txt)|*.txt|All files (*.*)|*.*";
		 */
		public static string PromptForFile(string directoryPath, string szDescription, string szFilter) {

			OpenFileDialog dlg = new OpenFileDialog();
			if (null != directoryPath && "" != directoryPath) {
				dlg.InitialDirectory = directoryPath;
			} else {
				dlg.InitialDirectory = @"C:\";
			}

			if (null != szDescription && "" != szDescription) {
				dlg.Title = szDescription;
			} else {
				dlg.Title = "Select a file";
			}

			if (null != szFilter && "" != szFilter) {
				dlg.Filter = szFilter;
			}

			DialogResult res = dlg.ShowDialog();
			if (res == DialogResult.OK) {
				return dlg.FileName;
			}
			
			return null;
		}
	}
}