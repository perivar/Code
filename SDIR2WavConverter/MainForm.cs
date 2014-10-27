using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

using CommonUtils.Audio.NAudio;
using CommonUtils;

namespace SDIR2WavConverter
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		const string _version = "1.0.1";
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			labelVersion.Text = "Version: " + _version;
			
			this.AllowDrop = true;
			this.DragEnter += new DragEventHandler(MainForm_DragEnter);
			this.DragDrop += new DragEventHandler(MainForm_DragDrop);
		}
		
		void MainForm_DragEnter(object sender, DragEventArgs e) {
			if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
		}

		void MainForm_DragDrop(object sender, DragEventArgs e) {
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			foreach (string inputFilePath in files) {
				
				// check if its a file or directory
				if (IOUtils.IsDirectory(inputFilePath)) {
					// directory
					IEnumerable<string> filesInDir = IOUtils.GetFiles(inputFilePath, "\\.sdir", SearchOption.AllDirectories);
					if (!filesInDir.Any()) {
						textBox1.AppendText(String.Format("No SDIR files found in directory {0}\n", inputFilePath));
					} else {
						foreach (string fileInDir in filesInDir) {
							SDIR2WavConvert(fileInDir);
						}
					}
					
				} else {
					// single file
					string fileExtension = Path.GetExtension(inputFilePath).ToLower();
					
					if (fileExtension.Equals(".sdir")) {
						SDIR2WavConvert(inputFilePath);
					} else {
						textBox1.AppendText(String.Format("Skipping file {0}\n", inputFilePath));
					}
				}
			}
		}
		
		private void SDIR2WavConvert(string inputFilePath, string outputFilePath = null) {
			
			string directoryName = Path.GetDirectoryName(inputFilePath);
			string fileName = Path.GetFileNameWithoutExtension(inputFilePath);
			
			if (outputFilePath == null) {
				outputFilePath = directoryName + Path.DirectorySeparatorChar + fileName + ".wav";
			}
			
			SdirPreset sdir = SdirPreset.ReadSdirPreset(inputFilePath);
			if (sdir != null) {
				AudioUtilsNAudio.CreateWaveFile(sdir.WaveformData, outputFilePath, new NAudio.Wave.WaveFormat(sdir.SampleRate, sdir.BitsPerSample, sdir.Channels));
				textBox1.AppendText(String.Format("Converted {0}\n", inputFilePath));
			}
			
		}
		
	}
}
