/*
 * Testing Phonetic String Comparisons
 * User: perivar.nerseth
 * Date: 14/02/2015
 * Time: 12:45
 */
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using CommonUtils; // for CSVWriter
using PhoneticStringComparison; // for DoubleMetaphone

namespace PhoneticCompareApp
{
	class Program
	{
		static string _version = "0.0.1";

		private static DoubleMetaphone dm = new DoubleMetaphone();

		private static void PrintUsage() {
			Console.WriteLine("Phonetic String Comparison. Version {0}.", _version);
			Console.WriteLine("Copyright (C) 2009-2015 Per Ivar Nerseth");
			Console.WriteLine();
			Console.WriteLine("Usage: PhoneticCompareApp.exe <input csv file> <output csv file>");
			Console.WriteLine("Choose an input csv file and an output csv file and this utility");
			Console.WriteLine("will perform a DoubleMetaphone encoding on the first column.");
			Console.WriteLine("The resulting csv file will contain two columns:");
			Console.WriteLine("One with the encoded name and one with the original name.");
			Console.WriteLine();
			Console.WriteLine("or specify the following to run an internal test:");
			Console.WriteLine("PhoneticCompareApp.exe -test");
		}
		
		public static void Main(string[] args)
		{
			string inputCSVFile = null;
			string outputCSVFile = null;
			bool doTest = false;
			
			if (args.Length == 0) {
				PrintUsage();
				return;
			} else if (args.Length == 1) {
				if (args[0].Contains("test")) {
					doTest = true;
				}
			} else if (args.Length == 2) {
				inputCSVFile = args[0];
				outputCSVFile = args[1];
			}
			
			if (doTest) {
				TestDoubleMethaphone();
				return;
			}
			
			if (!File.Exists(inputCSVFile)) {
				Console.Error.WriteLine("Could not find specified input file");
				return;
			}
			
			// define a list that can hold entries with identical keys
			var nameResults = new List<KeyValuePair<string, string>>();
			String text = File.ReadAllText(inputCSVFile, Encoding.Default);
			var stringSeparators = new string[] { "\r\n" };
			var namesArray = text.Split(stringSeparators, StringSplitOptions.None);
			
			// encode each name
			foreach (var name in namesArray) {
				EncodeAndStore(name, nameResults);
			}
			
			// sort names by encoded value
			var sortedList = from s in nameResults
				orderby s.Key
				select s;

			// build jagged array that can easily be written as csv
			var output = new object[sortedList.Count()][];
			int counter = 0;
			foreach (var name in sortedList) {
				output[counter] = new object[2];
				output[counter][0] = name.Key;
				output[counter][1] = name.Value;
				counter++;
			}
			
			CSVWriter csvWriter = new CommonUtils.CSVWriter(outputCSVFile, ',');
			csvWriter.Write(output);
		}
		
		private static void TestDoubleMethaphone() {
			Console.WriteLine("Testing Double Methaphone!");
			
			EncodeAndPrint("Didrik");
			EncodeAndPrint("Didrich");
			
			EncodeAndPrint("Anbjør");
			EncodeAndPrint("Anbiør");
			
			EncodeAndPrint("Kolbjørn");
			EncodeAndPrint("Gulbiør");
			
			EncodeAndPrint("ØRJAN");
			EncodeAndPrint("ÖRJAN");
			
			EncodeAndPrint("Kristjan");
			EncodeAndPrint("Kristian");
			EncodeAndPrint("Christian");
			EncodeAndPrint("Kristen");
			EncodeAndPrint("Christen");
			EncodeAndPrint("Xsten");
			EncodeAndPrint("Xstian");
			
			EncodeAndPrint("Tomas");
			EncodeAndPrint("Tomes");
			EncodeAndPrint("Tommes");
			EncodeAndPrint("Thomas");
			EncodeAndPrint("Thomes");
			EncodeAndPrint("Thommes");
			
			EncodeAndPrint("Thor");
			EncodeAndPrint("Thora");
			EncodeAndPrint("Thore");
			EncodeAndPrint("Thorre");
			EncodeAndPrint("Thurre");
			EncodeAndPrint("Tor");
			EncodeAndPrint("Tora");
			EncodeAndPrint("Tore");
			EncodeAndPrint("Torre");
			EncodeAndPrint("Turre");
			
			EncodeAndPrint("Ottho");
			EncodeAndPrint("Otthe");
			EncodeAndPrint("Otto");
			EncodeAndPrint("Otte");

			EncodeAndPrint("Kristine");
			EncodeAndPrint("Chirstine");
			
			EncodeAndPrint("Chasper");
			EncodeAndPrint("Kasper");
			
			EncodeAndPrint("Wilhelm");
			EncodeAndPrint("Vilhelm");
			EncodeAndPrint("Vellem");
			EncodeAndPrint("Vellum");
			
			EncodeAndPrint("Siw");
			EncodeAndPrint("Siv");
			
			EncodeAndPrint("Effuind");
			EncodeAndPrint("Effvind");
			EncodeAndPrint("Effuen");
			EncodeAndPrint("Øyvind");
			EncodeAndPrint("Eyvind");
			EncodeAndPrint("Eivind");
			
			EncodeAndPrint("Amun");
			EncodeAndPrint("Amund");
			EncodeAndPrint("Omund");
			
			EncodeAndPrint("Åsmund");
			EncodeAndPrint("Asmun");
			EncodeAndPrint("Edmund");
			EncodeAndPrint("Edmun");
		}
		
		public static void EncodeAndPrint (string fullName) {
			string firstName = GetFirstNameOnly(fullName);

			Console.WriteLine("{0}\t{1}", fullName, dm.Encode(firstName));
		}
		
		public static void EncodeAndStore(string fullName, List<KeyValuePair<string, string>> nameResults) {
			string firstName = GetFirstNameOnly(fullName);
			
			// build new name list
			var element = new KeyValuePair<string, string>(dm.Encode(firstName), fullName);
			nameResults.Add(element);
		}
		
		/// <summary>
		/// Return only the first name if the full name contains more than one name
		/// </summary>
		/// <param name="fullName">the full name</param>
		/// <returns>only the first name</returns>
		private static string GetFirstNameOnly(string fullName) {
			
			// only keep the first name if more than one
			var names = fullName.Split(' ');
			string firstName = names[0];
			if (names.Length > 1) {
				string lastName = names[1];
			}

			return firstName;
		}
	}
}