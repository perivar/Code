using System;
using System.IO;
using System.Text.RegularExpressions;

using NAudio;
using NAudio.Wave;

using CommonUtils.Audio.NAudio;
using CommonUtils.GUI;
using CommonUtils;

namespace StereoToQuadJoiner
{
	class Program
	{
		static string _version = "1.0.1";
		
		[STAThread]
		public static void Main(string[] args)
		{
			string directoryPath = "";
			
			// Command line parsing
			Arguments CommandLine = new Arguments(args);
			if(CommandLine["dir"] != null) {
				directoryPath = CommandLine["dir"];
			}
			if(CommandLine["?"] != null) {
				PrintUsage();
				return;
			}
			if(CommandLine["help"] != null) {
				PrintUsage();
				return;
			}
			
			// make sure that the directory exists...
			if (directoryPath == null || directoryPath.Length <= 0) {
				directoryPath = GUIUtils.PromptForPath("C:\\", "Choose directory to process");
			}
			if (directoryPath == null || directoryPath.Length <= 0) {
				Console.Out.WriteLine("No input directory. Script canceled.");
				Console.WriteLine();
				PrintUsage();
				return;
			} else {
				Console.Out.WriteLine("Directory {0} selected.", directoryPath);
			}

			SearchDirAndJoin(directoryPath);
		}
		
		public static void PrintUsage() {
			Console.WriteLine("StereoToQuadJoiner. Version {0}.", _version);
			Console.WriteLine("Copyright (C) 2009-2012 Per Ivar Nerseth.");
			Console.WriteLine();
			Console.WriteLine("Usage: StereoToQuadJoiner.exe <Arguments>");
			Console.WriteLine("Choose a directory and this utility will join all the files");
			Console.WriteLine("with the same name that ends with '_L.wav' and '_R.wav'");
			Console.WriteLine();
			Console.WriteLine("Optional Arguments:");
			Console.WriteLine("\t-dir=<path to where the *_L.wav and *_R.wav files are stored>");
			Console.WriteLine("\t-? or -help=show this usage helå>");
			
			Console.WriteLine("");
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
		
		private static void SearchDirAndJoin(string directoryPath) {

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
						if (AudioUtilsNAudio.CombineStereoToQuad(fi.FullName, filePathRightFull, combinedFileNamePath)) {
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
	}
}